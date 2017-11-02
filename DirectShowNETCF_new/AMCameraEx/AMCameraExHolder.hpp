#pragma once

#include <streams.h>
#include "grabberex.hpp"
#include "enummt.hpp"
#include "comptr.hpp" 
#include "directshowhelper.hpp"

enum AMResultEx
{
    AMRE_OK,
    AMRE_CameraFilterError,
    AMRE_GetGrabberError,
    AMRE_AddGrabberError,
    AMRE_ConnectGrabberError_1,
    AMRE_ConnectGrabberError_2,
    AMRE_VideoRendererError
};

class AMCameraExHolder
{
public:
	AMCameraExHolder();
	~AMCameraExHolder();
public:
	__int32 init(__int32 rotationType);
	void release();
	void stop();
	bool run(HWND owner);
	bool resize(HWND owner, __int32 width, __int32 height);
	bool flashOn();
	bool flashOff();
	bool autoFocusOn();
	bool autoFocusOff();
	bool focusPlus();
	bool focusMinus();
	bool zoomIn();
	bool zoomOut();

	void getRect(__int32 * width, __int32 * height);
    void applyEffect(__int32 effect);
	void * grabFrame();
    bool grabFrame(void * buffer);
	void drawBitmap(void * ptr, __int32 id, __int32 x, __int32 y, 
		__int32 width, __int32 height, __int32 transparent, 
		__int32 r, __int32 g, __int32 b);
	void fixPreview(__int32 fix);
	void eraseBitmap(__int32 id);

	__int32 getTypesCount();
	__int32 getType(__int32 index);
	void setType(__int32 index);
private:
	bool getVideoCaptureFilter();
	bool getGrabber();
	void getName(wchar_t * deviceName);
private: //Com Objects
	ComPtr<IBaseFilter> video;
    ComPtr<IBaseFilter> renderer;
    ComPtr<IBaseFilter> AMGrabber;
    ComPtr<ICaptureGraphBuilder2> capGraph;
    ComPtr<IGraphBuilder> graph;
    ComPtr<IVideoWindow> window;
    ComPtr<IMediaControl> control;
    ComPtr<IAMCameraControl> camControl;
    ComPtr<IGrabberEx> frmGrabber;
private:
    __int32 width_;
    __int32 height_;
    //private CEnumMediaTypes enummt = null;
	CMediaType current_;
	static CMediaType empty_;
	CAMCameraControlAdapter * cca_;
	CEnumMT enumMt_;
    bool preview;
};