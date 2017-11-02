#pragma once

DECLARE_INTERFACE_(IGrabberEx, IUnknown)
{
	virtual HRESULT getFrame(void *currentBuffer) = 0;
	virtual HRESULT getRect(int *width, int *height) = 0;
	virtual HRESULT applyEffect(int effect) = 0;
	virtual HRESULT drawBitmap(void * ptr, int id, int x, int y, int width, int height, int transparent, int r, int g, int b) = 0;
	virtual HRESULT eraseBitmap(int id) = 0;
	virtual HRESULT doRotate(int rotationType) = 0;
	virtual HRESULT doFixPreview(int fix) = 0;
};