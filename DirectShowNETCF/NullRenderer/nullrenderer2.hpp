#pragma once

#include <windows.h>
#include <streams.h>
#include <initguid.h>
#include "inullgrabber.hpp"
#include "frameformat.hpp"

class CNullRenderer : public CBaseRenderer, public INullGrabber
{
public:
	DECLARE_IUNKNOWN;

	static CNullRenderer * WINAPI CreateInstance(LPUNKNOWN punk, HRESULT *phr);
	~CNullRenderer();

	// IUnknown --
	STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void **ppv);

	// CNullRenderer overrides
	HRESULT CheckMediaType(const CMediaType * pmt);
	HRESULT SetMediaType(const CMediaType * pmt);
	HRESULT DoRenderSample(IMediaSample * mediaSample);

	// INullGrabber overrides
	//HRESULT getRect(int * width, int * height);
	__int32 getWidth() const;
	__int32 getHeight() const;
	HRESULT registerCallback(FrameCallback callback)
	{
		callback_ = callback;
		return S_OK;
	}
private:
    // Constructor
    CNullRenderer(LPUNKNOWN pUnk, HRESULT *phr);

	bool isValidMediaType(const CMediaType *mtIn);
	RawFrameFormat getFormat(GUID subType);
	inline void getRGB565(void * ptr);
	void drawDemoBuffer(unsigned char * pBufferOut);
private:
	CRITICAL_SECTION cs;
	RawFrameFormat format_;
	__int32 width_;
	__int32 height_;
	__int32 size_;
	void * buff_;
	FrameCallback callback_;
};