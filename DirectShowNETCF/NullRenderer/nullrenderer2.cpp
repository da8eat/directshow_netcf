#include "stdafx.h"
#include "nullrenderer2.hpp"
//#include "trace.hpp"

// 2B21766C-D405-4E27-A51C-A4812BE0CE4C
DEFINE_GUID(CLSID_NullRenderer, 
			0x2B21766C, 0xD405, 0x4E27, 0xA5, 0x1C, 0xA4, 0x81, 0x2B, 0xE0, 0xCE, 0x4C);
//DIVX return it (!!!) kinda of YUV (4:2:0) format
DEFINE_GUID(MediaType_YX12,
			0x59580F8E, 0xAF00, 0x4856, 0x94, 0x78, 0xEE, 0xAE, 0x26, 0x22, 0x01, 0x2F);

static bool doTrace = false;

static void trace_(const char * msg)
{
	if (doTrace)
	{
		FILE * f = fopen("trace.txt", "a");
		fprintf(f, msg);
		fprintf(f, "\n");
		fflush(f);
		fclose(f);
	}
}

CNullRenderer * WINAPI CNullRenderer::CreateInstance(LPUNKNOWN punk, HRESULT *phr)
{
	trace_(__FUNCTION__);
	ASSERT(phr);
	CNullRenderer *pNewObject = new CNullRenderer(punk, phr);
    if (pNewObject == 0)
    {
        if (phr)
            *phr = E_OUTOFMEMORY;
    }

	pNewObject -> NonDelegatingAddRef();

    return pNewObject;
}


CNullRenderer::CNullRenderer(LPUNKNOWN pUnk, HRESULT *phr) : 
	CBaseRenderer(CLSID_NullRenderer, TEXT("NullRenderer"), pUnk, phr), width_(0), 
	height_(0), buff_(0), size_(0), callback_(0)
{	
	trace_(__FUNCTION__);
	//DoTimeLimit();
}

CNullRenderer::~CNullRenderer()
{
	trace_(__FUNCTION__);
	if (buff_)
	{
		free(buff_);
	}
}

STDMETHODIMP CNullRenderer::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
	trace_(__FUNCTION__);
	GUID IID_INullGrabber = {0x2b21766c, 0xd405, 0x4e27, 0xa5, 0x1c, 0xa4, 0x81, 0x2b, 0xe0, 0xce, 0x4c};
	CheckPointer(ppv, E_POINTER);

    if (IsEqualGUID(riid, IID_INullGrabber))
	{
		HRESULT res = E_POINTER;
		INullGrabber * grabber = static_cast<INullGrabber *>(const_cast<CNullRenderer *>(this));
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
		HRESULT hr = CBaseRenderer::NonDelegatingQueryInterface(riid, ppv);
		return hr;
	}
}

__int32 CNullRenderer::getWidth() const
{
	trace_(__FUNCTION__);
	return width_;
}

__int32 CNullRenderer::getHeight() const
{
	trace_(__FUNCTION__);
	return height_;
}
/*
HRESULT CNullRenderer::getRect(int * width, int * height)
{
	FILE * f = fopen("\\Hard Disk\\log\\log.txt", "a");
	fprintf(f, "getwidth");
	fflush(f);
	*width = width_;
	fprintf(f, "getheight");
	fflush(f);
	*height = height_;
	fprintf(f, "return");
	fflush(f);
	fclose(f);
	return S_OK;
}
*/
inline void CNullRenderer::getRGB565(void * ptr)
{
	trace_(__FUNCTION__);
	if (buff_ && ptr)
	{
		if (format_ == RGB565)
		{
			memcpy(buff_, ptr, size_);
		}
		else if (format_ == YV12)
		{
			unsigned char * Y = static_cast<unsigned char *>(ptr);
			unsigned char * V = static_cast<unsigned char *>(ptr) + width_ * height_;
			unsigned char * U = V + (width_ * height_ >> 2);

			unsigned short *result = static_cast<unsigned short*>(buff_);

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

					result[j * width_ + i] = (((r) << 8 & 0xF800) | 
											 ((g << 3) & 0x07E0)  | ((b >> 3)));
				}
			}
		}
		else if (format_ == YUY2)
		{
			struct yuy2
			{
				unsigned char y1;
				unsigned char u;
				unsigned char y2;
				unsigned char v;
			};

			unsigned length = (width_ * height_) >> 1;
			unsigned short * result = static_cast<unsigned short*>(buff_);
			yuy2 * buffer = static_cast<yuy2 *>(ptr);

			for (unsigned i = 0; i < length; ++i)
			{
				int C = buffer -> y1 - 16;
				int D = buffer -> u - 128;
				int E = buffer -> v - 128;

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

				*result++ = (((r) << 8 & 0xF800) | 
							((g << 3) & 0x07E0)  | ((b >> 3)));

				C = buffer -> y2 - 16;
				D = buffer -> u - 128;
				E = buffer -> v - 128;

				r = (298 * C           + 409 * E + 128) >> 8;
				g = (298 * C - 100 * D - 208 * E + 128) >> 8;
				b = (298 * C + 516 * D           + 128) >> 8;
	
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

				*result++ = (((r) << 8 & 0xF800) | 
							((g << 3) & 0x07E0)  | ((b >> 3)));

				++buffer;
			}
		}
	}
	//drawDemoBuffer(static_cast<unsigned char *>(buff_));
}

HRESULT CNullRenderer::CheckMediaType(const CMediaType * pmt)
{
	trace_(__FUNCTION__);
	if (isValidMediaType(pmt))
    {
        return S_OK;
    }
    else
    {
        return VFW_E_TYPE_NOT_ACCEPTED;
    }
}

bool CNullRenderer::isValidMediaType(const CMediaType *mtIn)
{
	trace_(__FUNCTION__);
	if (!mtIn || (mtIn -> majortype != MEDIATYPE_Video))
	{
		return false;
	}

	if (IsEqualGUID(mtIn -> formattype, FORMAT_VideoInfo))
	{

		VIDEOINFOHEADER *pHdr = reinterpret_cast<VIDEOINFOHEADER *>(mtIn -> pbFormat);
		
		width_ = pHdr -> bmiHeader.biWidth;
		height_ = abs(pHdr -> bmiHeader.biHeight);
		format_ = getFormat(mtIn -> subtype);
		
		if (buff_)
		{
			free(buff_);
			buff_ = 0;
		}

		buff_ = malloc(width_ * height_ * 2);
		size_ = width_ * height_ * 2;

	}
	else if (IsEqualGUID(mtIn -> formattype, FORMAT_VideoInfo2))
	{
		MessageBox(0, L"VideoInfo2(!!!). Please inform me if you got this message", L"Info", 0);
		VIDEOINFOHEADER2 *pHdr = reinterpret_cast<VIDEOINFOHEADER2 *>(mtIn -> pbFormat);
		width_ = pHdr -> bmiHeader.biWidth;
		height_ = abs(pHdr -> bmiHeader.biHeight);
		format_ = getFormat(mtIn -> subtype);

		if (buff_)
		{
			free(buff_);
			buff_ = 0;
		}

		buff_ = malloc(width_ * height_ * 2);
		size_ = width_ * height_ * 2;
	//	return false;
	}

	if ((format_ != YV12) && (format_ != RGB565) && (format_ != YUY2))
	{
			return false;
	}

	return true;
}

HRESULT CNullRenderer::SetMediaType(const CMediaType * pmt)
{
	trace_(__FUNCTION__);
	return S_OK;
}

HRESULT CNullRenderer::DoRenderSample(IMediaSample * mediaSample)
{
	trace_(__FUNCTION__);
	if (callback_)
	{
		unsigned char * buff = 0;
		mediaSample ->GetPointer(&buff);
		getRGB565(buff);
		callback_(static_cast<unsigned char *>(buff_));
	}

	return S_OK;
}

RawFrameFormat CNullRenderer::getFormat(GUID subType)
{
	trace_(__FUNCTION__);
	if (subType == MEDIASUBTYPE_YVU9)
	{
		return YVU9;
	}
	else if (subType == MediaType_YX12)
	{
		return YV12;
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

__inline void CNullRenderer::drawDemoBuffer(unsigned char * pBufferOut)
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