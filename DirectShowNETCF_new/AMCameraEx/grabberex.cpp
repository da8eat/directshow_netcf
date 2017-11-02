#include "stdafx.h"
#include "grabberex.hpp"
#include "trace.hpp"


// {2A8A57CC-45C7-4a5c-9D64-4B17949C554A}
DEFINE_GUID(CLSID_GrabberEx, 
			0x2a8a57cc, 0x45c7, 0x4a5c, 0x9d, 0x64, 0x4b, 0x17, 0x94, 0x9c, 0x55, 0x4a);

// Constructor
CGrabberEx::CGrabberEx(LPUNKNOWN pUnk, HRESULT *phr) : 
	CTransformFilter(TEXT("ColorConvertor"), pUnk, CLSID_GrabberEx), effects_(0), 
	rotationType_(0), buff(0), outputType(0)
{
	//DoTimeLimit();
	InitializeCriticalSection(&cs);
}

CGrabberEx * WINAPI CGrabberEx::CreateInstance(LPUNKNOWN punk, HRESULT *phr)
{
	//trace_(__FUNCTION__);
	ASSERT(phr);
	CGrabberEx *pNewObject = new CGrabberEx(punk, phr); //new CSampleGrabber(punk, phr);
    if (!pNewObject)
    {
        if (phr)
            *phr = E_OUTOFMEMORY;
    }

	pNewObject -> NonDelegatingAddRef();

    return pNewObject;
}


STDMETHODIMP CGrabberEx::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
	//trace_(__FUNCTION__);
	GUID IID_IGrabberEx = {0x2b21655b, 0xd405, 0x4e27, 0xa5, 0x1c, 0xa4, 0x81, 0x2b, 0xe0, 0xce, 0x4c};
	CheckPointer(ppv, E_POINTER);

    if (IsEqualGUID(riid, IID_IGrabberEx))
	{
		HRESULT res = E_POINTER;
		IGrabberEx * grabber = dynamic_cast<IGrabberEx *>(const_cast<CGrabberEx *>(this));
		if (grabber)
		{
			res = S_OK;
			*ppv = static_cast<void *>(grabber);
			NonDelegatingAddRef();
		}
        return res;
	}
	else
	{
		return CTransformFilter::NonDelegatingQueryInterface(riid, ppv);
	}
}

bool CGrabberEx::isValidMediaType(const CMediaType *mtIn)
{
	//trace_(__FUNCTION__);
	if (!mtIn || (!IsEqualGUID(mtIn -> majortype, MEDIATYPE_Video)))
	{
		return false;
	}

	if (IsEqualGUID(mtIn -> formattype, FORMAT_VideoInfo))
	{

		int screenWidth = GetSystemMetrics(SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SM_CYSCREEN);
		bool rotate = false;

		VIDEOINFOHEADER *pHdr = reinterpret_cast<VIDEOINFOHEADER *>(mtIn -> pbFormat);
		
		width_ = pHdr -> bmiHeader.biWidth;
		height_ = pHdr -> bmiHeader.biHeight;
		if (height_ < 0)
			height_ = -height_;

		format_ = getFormat(mtIn -> subtype);

		if ((format_ != YV12) && (format_ != RGB565))
		{
			return false;
		}

		if (outputType)
		{
			delete outputType;
			outputType = 0;
		}

		outputType = new CMediaType(*mtIn);

		outputType -> subtype = MEDIASUBTYPE_RGB24;

		VIDEOINFOHEADER *outHdr = reinterpret_cast<VIDEOINFOHEADER *>(outputType -> pbFormat);
		outHdr -> bmiHeader.biBitCount = 24;
		outHdr -> bmiHeader.biCompression = BI_RGB; 
		outHdr -> bmiHeader.biSizeImage = 3 * width_ * height_;
		outHdr -> bmiHeader.biHeight = height_;
		outHdr -> bmiHeader.biWidth = width_;
		outHdr -> bmiHeader.biPlanes = 1;
		outHdr -> bmiHeader.biXPelsPerMeter = 0;
		outHdr -> bmiHeader.biYPelsPerMeter = 0;
		outHdr -> bmiHeader.biClrUsed = 0;
		outHdr -> bmiHeader.biClrImportant = 0;

		outHdr -> rcSource.bottom = outHdr -> rcTarget.bottom = height_;
		outHdr -> rcSource.right = outHdr -> rcTarget.right = width_;
		outHdr -> rcSource.top = outHdr -> rcTarget.top = 0;
		outHdr -> rcSource.left = outHdr -> rcTarget.left = 0;

		free(buff);
		buff = 0;
		buff = malloc(3 * width_ * height_);

		if (effects_)
		{
			delete effects_;
			effects_ = 0;
		}
		

		if ((screenHeight * width_ != screenWidth * height_ && screenHeight * height_ == screenWidth * width_ && rotationType_ == 1) ||
			(rotationType_ == 2) || (rotationType_ == 3) || (screenHeight == 800 && screenWidth == 480 && rotationType_ == 1))
		{
			outHdr -> bmiHeader.biWidth = height_;
			outHdr -> bmiHeader.biHeight = width_;
			outHdr -> rcSource.bottom = outHdr -> rcTarget.bottom = width_;
			outHdr -> rcSource.right = outHdr -> rcTarget.right = height_;
			outHdr -> rcSource.top = outHdr -> rcTarget.top = 0;
			outHdr -> rcSource.left = outHdr -> rcTarget.left = 0;

			rotate = true;
		}


		effects_ = new CEffects(format_ == YV12, width_, height_, rotate);

		if (rotate)
		{
			std::swap(width_, height_);
		}

		if ((screenHeight == 800 && screenWidth == 480) || (rotationType_ == 3))
		{
			effects_ -> setOmniaFlag();
		}

	}
	else if (IsEqualGUID(mtIn -> formattype, FORMAT_VideoInfo2))
	{
		MessageBox(0, L"VideoInfo2(!!!). Please inform me if you got this message", L"Info", 0);
		/*VIDEOINFOHEADER2 *pHdr = static_cast<VIDEOINFOHEADER2 *>(mtIn -> pbFormat);
		width_ = pHdr -> bmiHeader.biWidth;
		height_ = pHdr -> bmiHeader.biHeight;
		format_ = getFormat(mediat_ -> subtype);*/
		return false;
	}
	else
	{
		return false;
	}

	return true;
}

RawFrameFormat CGrabberEx::getFormat(GUID subType)
{
	//trace_(__FUNCTION__);
	if (subType == MEDIASUBTYPE_YVU9)
	{
		return YVU9;
	}
	else if (subType == MEDIASUBTYPE_Y411)
	{
		return Y411;
	}
	else if (subType == MEDIASUBTYPE_Y41P)
	{
		return Y41P;
	}
	else if (subType == MEDIASUBTYPE_YUY2)
	{
		return YUY2;
	}
	else if (subType == MEDIASUBTYPE_YVYU)
	{
		return YVYU;
	}
	else if (subType == MEDIASUBTYPE_UYVY)
	{
		return UYVY;
	}
	else if (subType == MEDIASUBTYPE_Y211)
	{
		return Y211;
	}
	else if (subType == MEDIASUBTYPE_YV12)
	{
		return YV12;
	}
	else if (subType == MEDIASUBTYPE_MJPG)
	{
		return MJPG;
	}
	else if (subType == MEDIASUBTYPE_RGB565)
	{
		return RGB565;
	}
	else if (subType == MEDIASUBTYPE_RGB24)
	{
		return RGB24;
	} 
	else if (subType == MEDIASUBTYPE_RGB32)
	{
		return RGB32;
	}
	else
	{
		return Unknown;
	}
}

HRESULT CGrabberEx::CheckTransform(const CMediaType *mtIn, const CMediaType *mtOut)
{
	//trace_(__FUNCTION__);
	return S_OK;
}

HRESULT CGrabberEx::CheckInputType(const CMediaType *pmt)
{
	//trace_(__FUNCTION__);
	if (isValidMediaType(pmt))
    {
        return S_OK;
    }
    else
    {
        return VFW_E_TYPE_NOT_ACCEPTED;
    }
}

HRESULT CGrabberEx::GetMediaType(int iPosition, CMediaType *pMediaType)
{
	//trace_(__FUNCTION__);
	// The output pin calls this method only if the input pin is connected.
    ASSERT(m_pInput -> IsConnected());

    // There is only one output type that we want, which is the input type.

    if (iPosition < 0)
    {
        return E_INVALIDARG;
    }
    else if (!iPosition)
    {
		*pMediaType = *outputType;
		return S_OK;
    }
    return VFW_S_NO_MORE_ITEMS;
}

HRESULT CGrabberEx::drawBitmap(void * ptr, int id, int x, int y, int width, int height, int transparent, int r, int g, int b)
{
	//trace_(__FUNCTION__);
	//MessageBox(0, L"You are using demo version of AMCameraEx control.\r\n To buy please visit:\r\nhttp://alexmogurenko.com/blog/buy/", L"Info", 0);
	EnterCriticalSection(&cs);

	if (!(bitmaps.size() ^ 10))
	{
		LeaveCriticalSection(&cs);
		return E_FAIL;
	}

	CDrawContainer * container = 0;

	if (!transparent)
	{
		container = new CDrawContainer(id, x, y, width, height, 0, ptr);
		if (r)
			container -> setBlendValue(r);
	}
	else
	{
		rgb24 color = {0};
		color.b = b;
		color.g = g;
		color.r = r;

		container = new CDrawContainer(id, x, y, width, height, &color, ptr);
	}

	bitmaps.push_back(container);

	LeaveCriticalSection(&cs);
	
	return S_OK;
}

HRESULT CGrabberEx::eraseBitmap(int id)
{
	//trace_(__FUNCTION__);
	EnterCriticalSection(&cs);
	do
	{
		std::vector<CDrawContainer *>::iterator it;
		it = bitmaps.begin();


		while (it != bitmaps.end())
		{
			CDrawContainer * container = *it;
			if (container -> isEqualId(id))
			{
				bitmaps.erase(it);
				delete container;
				container = 0;
				break;
			}

			it++;
		}
	}
	while (false);

	LeaveCriticalSection(&cs);

	return S_OK;
}


HRESULT CGrabberEx::doFixPreview(int fix)
{
	if (effects_)
	{
		effects_ -> FixPreview(fix > 0);
		return S_OK;
	}
	return -1;
}

HRESULT CGrabberEx::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pProp)
{
	//trace_(__FUNCTION__);
	// Make sure the input pin is connected.
    if (!m_pInput->IsConnected()) 
    {
        return E_UNEXPECTED;
    }

    // Our strategy here is to use the upstream allocator as the guideline,
    // but also defer to the downstream filter's request 
    // when it's compatible with us.

    // First, find the upstream allocator...
    ALLOCATOR_PROPERTIES InputProps;

    IMemAllocator *pAllocInput = 0;
    HRESULT hr = m_pInput -> GetAllocator(&pAllocInput);

    if (FAILED(hr))
    {
        return hr;
    }

    // ...now get the properties.

    hr = pAllocInput -> GetProperties(&InputProps);
    pAllocInput -> Release();

    if (FAILED(hr)) 
    {
        return hr;
    }

    // Buffer alignment should be non-zero [zero alignment makes no sense!].
    if (!pProp -> cbAlign)
    {
        pProp -> cbAlign = 1;
    }

    // Number of buffers must be non-zero.
    if (!pProp -> cbBuffer)
    {
        pProp -> cBuffers = 1;
    }

    // For buffer size, find the maximum of the upstream size and 
    // the downstream filter's request.
    pProp->cbBuffer = 3 * width_ * height_;//max(InputProps.cbBuffer, pProp->cbBuffer);

    // Now set the properties on the allocator that was given to us.
    ALLOCATOR_PROPERTIES Actual;
    hr = pAlloc->SetProperties(pProp, &Actual);
    if (FAILED(hr)) 
    {
        return hr;
    }
    
    // Even if SetProperties succeeds, the actual properties might be
    // different than what we asked for. We check the result, but we look
    // at only the properties that we care about. The downstream filter
    // will look at them when NotifyAllocator is called.
    
    if (InputProps.cbBuffer > Actual.cbBuffer) 
    {
        return E_FAIL;
    }
    
    return S_OK;
}

HRESULT CGrabberEx::Transform(IMediaSample *pSource, IMediaSample *pDest)
{
	//trace_(__FUNCTION__);
	// Get the addresses of the actual buffers.
    unsigned char *pBufferIn = 0, *pBufferOut = 0;

    pSource->GetPointer(&pBufferIn);
    pDest->GetPointer(&pBufferOut);

	if (pBufferIn && pBufferOut)
	{
		long cbByte = 3 * width_ * height_;

		EnterCriticalSection(&cs);
		effects_ -> Execute(pBufferIn, pBufferOut, buff);
		putBitmaps(pBufferOut);
		LeaveCriticalSection(&cs);

		ASSERT(pDest->GetSize() >= cbByte);

		pDest->SetActualDataLength(cbByte);
	}

	return S_OK;
}

HRESULT CGrabberEx::SetMediaType(PIN_DIRECTION direction, const CMediaType *pmt)
{
	//trace_(__FUNCTION__);
	return S_OK;
}

HRESULT CGrabberEx::getFrame(void *currentBuffer)
{
	//trace_(__FUNCTION__);
	//MessageBox(0, L"You are using demo version of AMCameraEx control.\r\n To buy please visit:\r\nhttp://alexmogurenko.com/blog/buy/", L"Info", 0);
	if (buff)
	{
		EnterCriticalSection(&cs);
		
		unsigned char * dst = static_cast<unsigned char *>(currentBuffer);
		unsigned char * src = static_cast<unsigned char *>(buff);
		
		src += width_ * 3 * (height_ - 1);
		int strinde = 3 * width_;


		for (int i = 0; i < height_; i++)
		{
			memcpy(dst, src, strinde);
			src -= strinde;
			dst += strinde;
		}

		//memcpy(currentBuffer, buff, 3 * width_ * height_);
		LeaveCriticalSection(&cs);
		return S_OK;
	}
	
	return -1;
}

HRESULT CGrabberEx::getRect(int *width, int *height)
{
	//trace_(__FUNCTION__);
	*width = width_; 
	*height = height_;
	return S_OK;
}

HRESULT CGrabberEx::doRotate(int rotationType)
{
	//trace_(__FUNCTION__);
	rotationType_ = rotationType;
	return S_OK;
}

HRESULT CGrabberEx::applyEffect(int effect)
{
	effects_ -> SetType(effect);
	return S_OK;
}

void CGrabberEx::putBitmaps(unsigned char * pBufferOut)
{
	//trace_(__FUNCTION__);
	std::vector<CDrawContainer *>::iterator it = bitmaps.begin();

	while (it != bitmaps.end())
	{
		CDrawContainer * container = *it;
		effects_ -> putBitmap(pBufferOut,  container -> getBuff(), 
			container -> getWidth(), container -> getHeight(), 
			container -> getX(), container -> getY(), container -> getColor(), 
			container -> getBlend());

		++it;
	}
	//drawDemoBuffer(pBufferOut);
}

//#include "demo.hpp"
//
//__inline void CGrabberEx::drawDemoBuffer(unsigned char * pBufferOut)
//{
//	int demoWidth_ = width_ > demoWidth ? demoWidth : width_;
//	int demoHeight_ = height_ > demoHeight ? demoHeight : height_;
//	int x = (width_ - demoWidth_) >> 1;
//	int y = (height_ - demoHeight_) >> 1;
//
//	for (int i = y; i < y + demoHeight_; ++i)
//	{
//		for (int j = x; j < x + demoWidth_; ++j)
//		{
//			unsigned char r = pBufferOut[3 * i * width_ + j * 3] >> 1;
//			unsigned char g = pBufferOut[3 * i * width_ + j * 3 + 1] >> 1;
//			unsigned char b = pBufferOut[3 * i * width_ + j * 3 + 2] >> 1;
//			 
//
//			pBufferOut[3 * i * width_ + j * 3] = r + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].r;
//			pBufferOut[3 * i * width_ + j * 3 + 1] = g + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].g;
//			pBufferOut[3 * i * width_ + j * 3 + 2] = b + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].b;
//		}
//	}
//}