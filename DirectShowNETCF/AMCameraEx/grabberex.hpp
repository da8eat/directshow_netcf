#pragma once

#include <windows.h>
#include <streams.h>
#include <initguid.h>
#include <vector>
#include "frameformat.hpp"
#include "drawcontainer.hpp"
#include "ieffect.hpp"
#include "effects.hpp"
#include "rgb.hpp"

class CGrabberEx : public CTransformFilter, public IGrabberEx
{
public:
	DECLARE_IUNKNOWN;

	// -- IUnknown --
	STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void **ppv);

	~CGrabberEx()
	{
		if (outputType)
		{
			delete outputType;
		}

		if (buff)
		{
			free(buff);
			buff = 0;
		}

		if (effects_)
		{
			delete effects_;
		}

		while (bitmaps.size())
		{
			CDrawContainer * container = *bitmaps.begin();
			bitmaps.erase(bitmaps.begin());

			delete container;
			container = 0;
		}

		DeleteCriticalSection(&cs);
	}

    // Overridden CTransformFilter methods
    HRESULT CheckInputType(const CMediaType *mtIn);
    HRESULT CheckTransform(const CMediaType *mtIn, const CMediaType *mtOut);
    HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pProp);
    HRESULT GetMediaType(int iPosition, CMediaType *pMediaType);
    HRESULT Transform(IMediaSample *pIn, IMediaSample *pOut);

    // Override this so we can grab the video format
    HRESULT SetMediaType(PIN_DIRECTION direction, const CMediaType *pmt);

    // Static object-creation method (for the class factory)
    static CGrabberEx * __stdcall CreateInstance(LPUNKNOWN punk, HRESULT *phr);

	//IGrabberEx Methods
	HRESULT getFrame(void * currentBuffer);
	HRESULT getRect(int * width, int * height);
	HRESULT applyEffect(int effect);
	HRESULT drawBitmap(void * ptr, int id, int x, int y, int width, int height, int transparent, int r, int g, int b);
	HRESULT doRotate(int rotationType);
	HRESULT eraseBitmap(int id);
	HRESULT doFixPreview(int fix);
private:
    // Constructor
    CGrabberEx(LPUNKNOWN pUnk, HRESULT *phr);

	bool isValidMediaType(const CMediaType *mtIn);
	RawFrameFormat getFormat(GUID subType);
	void drawDemoBuffer(unsigned char * pBufferOut);

	RawFrameFormat format_;
	CMediaType *outputType;
	int width_, height_;
	int rotationType_;

	std::vector<CDrawContainer*> bitmaps;

	void * buff;
	CRITICAL_SECTION cs;
	CEffects * effects_;

	void putBitmaps(unsigned char * pBufferOut);
private:
	enum
	{
		Blend100,
		Blend50,
		Blend25,
		Blend12,
		Blend6,
	};
};