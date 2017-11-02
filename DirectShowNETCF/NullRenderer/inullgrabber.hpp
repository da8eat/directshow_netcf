#pragma once

typedef void (__stdcall *FrameCallback)(unsigned char *);

interface INullGrabber : public IUnknown
{
	//virtual HRESULT getRect(int * width, int * height) = 0;
	virtual __int32 getWidth() const = 0;
	virtual __int32 getHeight() const = 0;
	virtual HRESULT registerCallback(FrameCallback callback) = 0;
};