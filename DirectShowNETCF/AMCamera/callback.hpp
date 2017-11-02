#pragma once

//GUID IID_IGetFrame = {0x2b21644a, 0xd405, 0x4e27, 0xa5, 0x1c, 0xa4, 0x81, 0x2b, 0xe0, 0xce, 0x4c};

DECLARE_INTERFACE_(IGetFrame, IUnknown)
{
    /*STDMETHOD(getFrame) (THIS_
        void *currentBuffer
        ) PURE;

	STDMETHOD(getSize) (THIS_
		LONG *size
		) PURE;

	STDMETHOD(getFrameParams) (THIS_
		int *width, int *height, int *format
		) PURE;

	STDMETHOD(drawText) (THIS_
		void *ptr, int height, int width
		) PURE;

	STDMETHOD(stopDraw) (THIS_
		) PURE;

	STDMETHOD(getGrayScale) (THIS_
		void *ptr) PURE;*/
	virtual HRESULT getFrame(void *currentBuffer) = 0;
	virtual HRESULT getSize(long *size) = 0;
	virtual HRESULT getFrameParams(int *width, int *height, int *format) = 0;
	virtual HRESULT drawText(void *ptr, int height, int width) = 0;
	virtual HRESULT stopDraw() = 0;
	virtual HRESULT getGrayScale(void *ptr) = 0;
	virtual HRESULT getRgb(void *ptr) = 0;
	virtual HRESULT drawTarget(RECT coord, int type) = 0;
	virtual HRESULT stopDrawTarget() = 0;
};