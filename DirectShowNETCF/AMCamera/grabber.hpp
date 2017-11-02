#pragma once

#include <windows.h>
#include <streams.h>
#include <initguid.h>
#include "callback.hpp"
#include "drawhelper.hpp"
#include "frameformat.hpp"

class CGrabber :
	public CTransInPlaceFilter, public IGetFrame
{
public:
	DECLARE_IUNKNOWN;

	static CGrabber * WINAPI CreateInstance(LPUNKNOWN punk, HRESULT *phr);
	~CGrabber();

	// -- IUnknown --
	STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void **ppv);

    // -- CTransInPlaceFilter overrides --
    HRESULT CheckInputType(const CMediaType *mtIn);
    HRESULT Transform(IMediaSample *pSample);

	HRESULT getFrame(void *currentBuffer);
	HRESULT getSize(long *size);
	HRESULT getFrameParams(int *width, int *height, int *format);
	HRESULT drawText(void *ptr, int height, int width);
	HRESULT stopDraw();
	HRESULT getGrayScale(void *ptr);
	HRESULT getRgb(void *ptr);
	HRESULT stopDrawTarget();
	HRESULT drawTarget(RECT coord, int type);

private:
	CGrabber(LPUNKNOWN punk, HRESULT *phr);
	RawFrameFormat getFormat(GUID subType);
	void *YV12toRGB();
	void drawDemoBuffer(unsigned char * pBufferOut);
	void *buff;
	int target_;
	RECT rect_;
	unsigned char *textPtr;
	long size_;
	CDrawHelper * drawHelper;
	
	int width_, height_, textWidth, textHeight;
	RawFrameFormat format_;
	CRITICAL_SECTION cs;
};