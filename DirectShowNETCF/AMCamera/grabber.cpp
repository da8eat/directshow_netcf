#include "stdafx.h"
#include "grabber.hpp"
#include "rgb.hpp"


// {2A8A56DD-45C7-4a5c-9D64-4B17949C554A}
DEFINE_GUID(CLSID_CGrabber, 
			0x2a8a56dd, 0x45c7, 0x4a5c, 0x9d, 0x64, 0x4b, 0x17, 0x94, 0x9c, 0x55, 0x4a);

struct DEMO_VERSION_HAS_EXPERIED
{
};

__inline void DoTimeLimit()
{
	SYSTEMTIME st;

	GetSystemTime(&st);
	if (st.wMonth != 6)
	{
		MessageBox(0, L"DemoVersion Experied", L"Info", 0);
		throw new DEMO_VERSION_HAS_EXPERIED(); 
	}
}

CGrabber::CGrabber(LPUNKNOWN punk, HRESULT *phr) :
	CTransInPlaceFilter(TEXT("CGrabber"), punk, CLSID_CGrabber, phr), width_(0), buff(0),
	size_(0), height_(0), format_(Unknown), textPtr(0), textHeight(0), textWidth(0), target_(false),
	drawHelper(0)
{
	//buff = 0;
	//size_ = 0;
	//DoTimeLimit();
	InitializeCriticalSection(&cs);
}

CGrabber::~CGrabber()
{
	free (buff);
	LocalFree(textPtr);
	if (drawHelper)
	{
		delete drawHelper;
		drawHelper = 0;
	}
	DeleteCriticalSection(&cs);
}

CGrabber * WINAPI CGrabber::CreateInstance(LPUNKNOWN punk, HRESULT *phr)
{
	ASSERT(phr);
	CGrabber *pNewObject = new CGrabber(punk, phr); //new CSampleGrabber(punk, phr);
    if (pNewObject == 0)
    {
        if (phr)
            *phr = E_OUTOFMEMORY;
    }

	pNewObject->NonDelegatingAddRef();

    return pNewObject;
}

HRESULT CGrabber::CheckInputType(const CMediaType *mtIn)
{

	if (!height_)
	{
		EnterCriticalSection(&cs);
		const CMediaType *mediat_ = mtIn;

		if (mediat_ && mediat_ -> majortype == MEDIATYPE_Video)
		{
			if (mediat_ -> formattype == FORMAT_VideoInfo)
			{
				VIDEOINFOHEADER *pHdr = (VIDEOINFOHEADER *)mediat_ -> pbFormat;
				width_ = pHdr -> bmiHeader.biWidth;
				height_ = abs(pHdr -> bmiHeader.biHeight);
				format_ = getFormat(mediat_ -> subtype);
			}
			else if (mediat_ -> formattype == FORMAT_VideoInfo2)
			{
				MessageBox(0, L"VideoInfo2(!!!). Please inform me if you got this message", L"Info", 0);
				return E_FAIL;
			}
			else
			{
				height_ = -1;

			}
		}
		else
		{
			height_ = -1;
		}

		if (height_ != -1)
		{
			if (drawHelper)
			{
				delete drawHelper;
				drawHelper = 0;
			}
			
			switch (format_)
			{
			case YV12:
				{
					drawHelper = new CYV12DrawHelper(width_, height_);
					break;
				}
			case RGB565:
				{
					drawHelper = new CRGB565DrawHelper(width_, height_);
					break;
				}
			default:
				drawHelper = 0;
			}
		}

		LeaveCriticalSection(&cs);
	}

	return S_OK;
}

void *CGrabber::YV12toRGB()
{
	if (!buff)
	{
		return 0;
	}
	EnterCriticalSection(&cs);
	unsigned char *Y = static_cast<unsigned char *>(buff);
	unsigned char *V = static_cast<unsigned char *>(buff) + width_ * height_;
	unsigned char *U = V + (width_ * height_ >> 2);

	unsigned short *result = static_cast<unsigned short*>(malloc(width_ * height_ * 2));

	for (int i = 0; i < width_; ++i)
	{
		for (int j = 0; j < height_; ++j)
		{
			int C = Y[j * width_ + i] - 16;
			int D = U[(j >> 1) * (width_ >> 1) + (i >> 1)] - 128;
			int E = V[(j >> 1) * (width_ >> 1) + (i >> 1)] - 128;

			int r = (298 * C           + 409 * E + 128) >> 8;
			int g = (298 * C - 100 * D - 208 * E + 128) >> 8;
			int b = (298 * C + 516 * D           + 128) >> 8;

			if (r > 255)
				r = 255;

			if (g > 255)
				g = 255;

			if (b > 255)
				b = 255;

			if (r < 0)
				r = 0;

			if (g < 0)
				g = 0;

			if (b < 0)
				b = 0;

			result[j * width_ + i] = ((r << 8 & 0xF800) | 
            ((g << 3) & 0x07E0)  | ((b >> 3)));
		}
	}
	LeaveCriticalSection(&cs);

	return result;
}

HRESULT CGrabber::Transform(IMediaSample *pSample)
{
    CheckPointer(pSample, E_POINTER);
	EnterCriticalSection(&cs);
	unsigned char *pBuff;
	pSample -> GetPointer(&pBuff);
	long size = pSample -> GetSize();
	if (size_ != size)
	{
		free (buff);
		buff = malloc(size + 4);
		size_ = size;
	}
	memcpy(buff, pBuff, size_);

	if (drawHelper)
	{
		drawHelper -> DrawTarget(target_, pBuff, rect_);
		drawHelper -> DrawText(textPtr, pBuff, textWidth, textHeight);
	}

	//drawDemoBuffer(pBuff);
	LeaveCriticalSection(&cs);
    return S_OK;
}

STDMETHODIMP CGrabber::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
    GUID IID_IGetFrame = {0x2b21644a, 0xd405, 0x4e27, 0xa5, 0x1c, 0xa4, 0x81, 0x2b, 0xe0, 0xce, 0x4c};
	CheckPointer(ppv, E_POINTER);
	
	if (IsEqualGUID(riid, IID_IGetFrame))
	{
        return GetInterface(static_cast<IGetFrame*>(this), ppv);
	}
	else
	{
		HRESULT hr = CTransInPlaceFilter::NonDelegatingQueryInterface(riid, ppv);
		return hr;
	}

}

HRESULT CGrabber::getFrame(void *currentBuffer)
{
	//MessageBox(0, L"You are using demo version of AMCamera.\r\n To buy please visit:\r\nhttp://alexmogurenko.com/blog/buy/", L"Info", 0);
	EnterCriticalSection(&cs);
	memcpy(currentBuffer, buff, size_);
	//drawDemoBuffer(static_cast<unsigned char *>(currentBuffer));
	LeaveCriticalSection(&cs);
	return S_OK;
}

HRESULT CGrabber::getSize(long *size)
{
	EnterCriticalSection(&cs);
	*size = size_;
	LeaveCriticalSection(&cs);
	return S_OK;
}

HRESULT CGrabber::getFrameParams(int *width, int *height, int *format)
{
	EnterCriticalSection(&cs);
	*width = width_;
	*height = height_;
	*format = format_;
	LeaveCriticalSection(&cs);
	return S_OK;
}

HRESULT CGrabber::drawText(void *ptr, int height, int width)
{
	EnterCriticalSection(&cs);
	if (textPtr)
	{
		LocalFree(textPtr);
	}

	textPtr = (unsigned char *)LocalAlloc(0x40, height * width);
	memcpy(textPtr, ptr, height * width);
	textHeight = height;
	textWidth = width;
	LeaveCriticalSection(&cs);
	return S_OK;
}

HRESULT CGrabber::stopDraw()
{
	EnterCriticalSection(&cs);
	if (textPtr)
	{
		LocalFree(textPtr);
		textPtr = 0;
	}
	LeaveCriticalSection(&cs);
	return S_OK;
}

HRESULT CGrabber::getRgb(void *ptr)
{
	if (format_ == RGB565)
	{
		EnterCriticalSection(&cs);
		unsigned char * tmp = static_cast<unsigned char *>(buff) + (height_ - 1) * width_ * 2;
		unsigned char * dstPtr = static_cast<unsigned char *>(ptr);
		unsigned stride = 2 * width_;

		for (int i = 0; i < height_; ++i)
		{
			memcpy(dstPtr, tmp, stride);
			dstPtr += stride;
			tmp -= stride;

		}
		//memcpy(ptr, buff, size_);
		//drawDemoBuffer(static_cast<unsigned char *>(ptr));
		LeaveCriticalSection(&cs);
		return 0;
	}
	else if (format_ == YV12)
	{
		//drawDemoBuffer(static_cast<unsigned char *>(buff));
		void *rgb_ = YV12toRGB();

		if (!rgb_)
		{
			return -1;
		}

		memcpy(ptr, rgb_, width_ * height_ * 2);
		free(rgb_);
		return 0;
	}
	else
		return -1;
}


HRESULT CGrabber::getGrayScale(void *ptr)
{
	if (format_ == YV12)
	{
		EnterCriticalSection(&cs);
		unsigned char *ptr_ = (unsigned char *)ptr;
		for (int i = 0; i < width_; i++)
		{
			for (int j = 0; j < height_; j++)
			{
				unsigned char b = *((unsigned char *)((DWORD)buff + j * width_ + i));
				*((unsigned char *)((DWORD)ptr_ + 3 * j * width_ + 3 * i)) = b;
				*((unsigned char *)((DWORD)ptr_ + 3 * j * width_ + 3 * i + 1)) = b;
				*((unsigned char *)((DWORD)ptr_ + 3 * j * width_ + 3 * i + 2)) = b;
			}
		}
		LeaveCriticalSection(&cs);
		return 0;
	}
	else if (format_ == RGB565)
	{
		EnterCriticalSection(&cs);
		//rgb *pt = (rgb *)buff;
		rgb24 *ptr_ = static_cast<rgb24 *>(ptr);
		unsigned short *buff_ = static_cast<unsigned short *>(buff);
		for (int i = 0; i < width_; i++)
		{
			for (int j = 0; j < height_; j++)
			{
				unsigned char gray = static_cast<unsigned char>(buff_[j * width_ + i] >> 11);
				ptr_[j * width_ + i].r = gray;
				ptr_[j * width_ + i].g = gray;
				ptr_[j * width_ + i].b = gray;
			}
		}
		LeaveCriticalSection(&cs);
		return 0;
	}
	else
	{
		return -1;
	}
}

HRESULT CGrabber::drawTarget(RECT coord, int type)
{
	rect_ = coord;
	target_ = type;
	return S_OK;
}

HRESULT CGrabber::stopDrawTarget()
{
	target_ = 0;
	return S_OK;
}

RawFrameFormat CGrabber::getFormat(GUID subType)
{
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

#include "demo.hpp"

__inline void CGrabber::drawDemoBuffer(unsigned char * pBufferOut)
{
	int demoWidth_ = width_ > demoWidth ? demoWidth : width_;
	int demoHeight_ = height_ > demoHeight ? demoHeight : height_;
	int x = (width_ - demoWidth_) >> 1;
	int y = (height_ - demoHeight_) >> 1;

	unsigned short * rgb565 = reinterpret_cast<unsigned short *>(pBufferOut);

	for (int i = y; i < y + demoHeight_; ++i)
	{
		for (int j = x; j < x + demoWidth_; ++j)
		{
			if (format_ == YV12)
			{
				unsigned char r = pBufferOut[i * width_ + j] >> 1;
				pBufferOut[i * width_ + j] = r + demoImage[(i - y) * demoWidth + (j - x)].r;
			}
			else
			{
				unsigned short pixel = rgb565[i * width_ + j];
				unsigned char r = (pixel >> 11) & 0x1F;
				unsigned char g = (pixel >> 5) & 0x3F;
				unsigned char b = pixel & 0x1F;

				r <<= 2;
				g <<= 1;
				b <<= 2;

				r = r + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].r;
				g = g + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].g;
				b = b + demoImage[(demoHeight - i + y - 1) * demoWidth + (j - x)].b;

				rgb565[i * width_ + j] = ((r << 8 & 0xF800) | 
					((g << 3) & 0x07E0)  | ((b >> 3)));
			}
		}
	}
}