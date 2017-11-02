#include "stdafx.h"
#include "amcameraexholder.hpp"
#include "propertybag.h"


static void trace__(char * msg)
{
	FILE * ff = fopen("ttext.txt", "a");
	fprintf(ff, "%s\n", msg);
	fclose(ff);
}

CMediaType AMCameraExHolder::empty_;

AMCameraExHolder::AMCameraExHolder() : cca_(0)
{
	CoInitialize(0);
	CoCreateInstance(CLSID_FilterGraph, 0, CLSCTX_INPROC_SERVER, IID_IFilterGraph2, graph.getVoidRef());
	if (!graph.isValid())
	{
		return;
	}
	CoCreateInstance(CLSID_CaptureGraphBuilder, 0, CLSCTX_INPROC_SERVER, IID_ICaptureGraphBuilder2, capGraph.getVoidRef());
	if (!capGraph.isValid())
	{
		graph.clear();
		return;
	}
	if (FAILED(capGraph -> SetFiltergraph(graph.get())))
	{
		graph.clear();
		capGraph.clear();
		return;
	}

	if (!getVideoCaptureFilter())
	{
		video.clear();
		graph.clear();
		capGraph.clear();

		return;
	}

	if (FAILED(graph -> AddFilter(video.get(), _T("VideoCapture"))))
	{
		video.clear();
		graph.clear();
		capGraph.clear();
		return;
	}

	enumMt_.assign(capGraph.get(), video.get(), PIN_CATEGORY_PREVIEW);
	if (enumMt_.count() > 0)
	{
		current_ = enumMt_[0];
	}
	else
	{
		preview = false;
		enumMt_.assign(capGraph.get(), video.get(), PIN_CATEGORY_CAPTURE);
		if (enumMt_.count() > 0)
		{
			current_ = enumMt_[0];
		}
	}
}

AMCameraExHolder::~AMCameraExHolder()
{
	release();
	CoUninitialize();
}


__int32 AMCameraExHolder::init(__int32 rotationType)
{
	if (!video.isValid())
	{
		return AMRE_CameraFilterError;
	}

	GUID category = GUID_NULL;

	if (preview)
	{
		category = PIN_CATEGORY_PREVIEW;
	}
	else
	{
		category = PIN_CATEGORY_CAPTURE;
	}

	if (current_ != empty_)
	{
	    ComPtr<IPin> reqPin;

		capGraph -> FindPin(video.get(), PINDIR_OUTPUT, &category, &MEDIATYPE_Video, 1, 0, reqPin.getRef());
		
		if (reqPin.isValid())
		{
			ComPtr<IAMStreamConfig> config;
			reqPin -> QueryInterface(IID_IAMStreamConfig, config.getVoidRef());
			if (config.isValid())
			{
				config -> SetFormat(&current_);
			}
		}
	}
	if (!getGrabber())
	{
		return AMRE_GetGrabberError;
	}

	if (FAILED(graph -> AddFilter(AMGrabber.get(), _T("Grabber"))))
	{
		return AMRE_AddGrabberError;
	}

	GUID IID_IGrabberEx = {0x2b21655b, 0xd405, 0x4e27, 0xa5, 0x1c, 0xa4, 0x81, 0x2b, 0xe0, 0xce, 0x4c};
	AMGrabber -> QueryInterface(IID_IGrabberEx, frmGrabber.getVoidRef());

	frmGrabber -> doRotate(rotationType);
	
	if (FAILED(capGraph -> RenderStream(&category, &MEDIATYPE_Video, video.get(), 0, AMGrabber.get())))
	{
		return AMRE_ConnectGrabberError_1;
	}

	CoCreateInstance(CLSID_VideoRenderer, 0, CLSCTX_INPROC_SERVER, IID_IBaseFilter, renderer.getVoidRef());
	if (!renderer.isValid())
	{
		return AMRE_VideoRendererError;
	}
	
	if (FAILED(graph -> AddFilter(renderer.get(), _T("VideoRenderer"))))
	{
		return AMRE_VideoRendererError;
	}

	if (FAILED(capGraph -> RenderStream(0, 0, AMGrabber.get(), 0, renderer.get())))
	{
		return AMRE_ConnectGrabberError_2;
	}

	graph -> QueryInterface(IID_IMediaControl, control.getVoidRef());
	frmGrabber -> getRect(&width_, &height_);
	if (height_ < 0)
		height_ *= -1;
	
	return AMRE_OK;
}


void AMCameraExHolder::stop()
{
	window.clear();
	if (renderer.isValid())
	{
		renderer -> QueryInterface(IID_IVideoWindow, window.getVoidRef());
	}
	if (window.isValid())
	{
		window -> put_Visible(OAFALSE);
		window -> put_Owner(0);
	}
	if (control.isValid())
	{
		control -> Stop();
	}
}

bool AMCameraExHolder::run(HWND owner)
{
	
	if (!control.isValid())
	{
		return false;
	}

	window.clear();
	renderer -> QueryInterface(IID_IVideoWindow, window.getVoidRef());
	if (window.isValid())
	{
		RECT rc;
		GetClientRect(owner, &rc);
		window -> put_Owner((OAHWND)owner);
		window -> SetWindowPosition(rc.left, rc.top, rc.right, rc.bottom);
		window -> put_WindowStyle(0x40000000 | 0x02000000);
		window -> put_Visible(OATRUE);
		window -> put_MessageDrain((OAHWND)owner);
	}
	else
	{
		return false;
	}

	bool result = SUCCEEDED(control -> Run()); 
	return result;
}

bool AMCameraExHolder::resize(HWND owner, __int32 width, __int32 height)
{
	window.clear();
	renderer -> QueryInterface(IID_IVideoWindow, window.getVoidRef());
	if (window.isValid())
	{
		window -> put_Owner((OAHWND)owner);
		window -> SetWindowPosition(0, 0, width, height);
		window -> put_WindowStyle(0x40000000 | 0x02000000);
		window -> put_Visible(OATRUE);
		window -> put_MessageDrain((OAHWND)owner);
		return true;
	}
	return false;
}

void AMCameraExHolder::release()
{
	if (cca_)
	{
		delete cca_;
		cca_ = 0;
	}
	if (control.isValid())
	{
		control -> Stop();
	}

	if (graph.isValid())
	{
		CDSHelper::clearGraph(graph.get(), 0);
	}
	video.clear();
	renderer.clear();
	AMGrabber.clear();
	window.clear();
	control.clear();
	frmGrabber.clear();
	camControl.clear();
	capGraph.clear();
	graph.clear();
}

void AMCameraExHolder::getRect(__int32 * width, __int32 * height)
{
	*width = width_;
	*height = height_;
}

void AMCameraExHolder::applyEffect(__int32 effect)
{
	frmGrabber -> applyEffect(effect);
}

void * AMCameraExHolder::grabFrame()
{
	void * frame = ::LocalAlloc(0x40, 3 * width_ * height_);
    frmGrabber -> getFrame(frame);
    return frame;
}

bool AMCameraExHolder::grabFrame(void * buffer)
{
	return frmGrabber -> getFrame(buffer) != -1;
}

void AMCameraExHolder::drawBitmap(void * ptr, __int32 id, __int32 x, __int32 y, 
		__int32 width, __int32 height, __int32 transparent, 
		__int32 r, __int32 g, __int32 b)
{
	frmGrabber -> drawBitmap(ptr, id, x, y, width, height, transparent, r, g, b);
}

void AMCameraExHolder::fixPreview(__int32 fix)
{
	frmGrabber -> doFixPreview(fix);
}

void AMCameraExHolder::eraseBitmap(__int32 id)
{
	frmGrabber -> eraseBitmap(id);
}

__int32 AMCameraExHolder::getTypesCount()
{
	return static_cast<__int32>(enumMt_.count());
}

__int32 AMCameraExHolder::getType(__int32 index)
{
	return enumMt_.getMediaDescription(index);
}

void AMCameraExHolder::setType(__int32 index)
{
	current_ = enumMt_[index];
	stop();
	if (graph.isValid())
	{
		CDSHelper::clearGraph(graph.get(), video.get());
	}

	renderer.clear();
	AMGrabber.clear();
	window.clear();
	control.clear();
	frmGrabber.clear();
	camControl.clear();
}

bool AMCameraExHolder::getGrabber()
{
	HRESULT hr = S_OK;
	ComPtr<IBaseFilter> tmp(CGrabberEx::CreateInstance(0, &hr));
	if (!tmp.isValid())
	{
		return false;
	}
	AMGrabber = tmp;
	return true;
}

bool AMCameraExHolder::getVideoCaptureFilter()
{
	CoCreateInstance(CLSID_VideoCapture, 0, CLSCTX_INPROC_SERVER, IID_IBaseFilter, video.getVoidRef());
	if (!video.isValid())
	{
		return false;
	}

	wchar_t deviceName[MAX_PATH] = L"\0";  
    getName(deviceName);
    CComVariant comName = deviceName;

	ComPtr<IPersistPropertyBag> propBag;
	video -> QueryInterface(IID_IPersistPropertyBag, propBag.getVoidRef());
	if (!propBag.isValid())
	{
		return false;
	}

	CPropertyBag bag;
	bag.Write(_T("VCapName"), &comName);
	if (FAILED(propBag -> Load(&bag, 0)))
	{
		return false;
	}

	video -> QueryInterface(IID_IAMCameraControl, camControl.getVoidRef());
	if (cca_)
	{
		delete cca_;
		cca_ = 0;
	}

	cca_ = new CAMCameraControlAdapter(camControl.get());

	return true;
}

void AMCameraExHolder::getName(wchar_t * deviceName)
{
    HRESULT hr = S_OK;
    HANDLE handle = 0;
    DEVMGR_DEVICE_INFORMATION di;
    GUID guidCamera = { 0xCB998A05, 0x122C, 0x4166, 0x84, 0x6A, 0x93, 0x3E, 
        0x4D, 0x7E, 0x3C, 0x86 };

    di.dwSize = sizeof(di);

    handle = FindFirstDevice(DeviceSearchByGuid, &guidCamera, &di);
    StringCchCopy(deviceName, MAX_PATH, di.szLegacyName);
	FindClose(handle);
}

//not implemented yet

bool AMCameraExHolder::flashOn()
{
	return cca_ -> flashOn();
}

bool AMCameraExHolder::flashOff()
{
	return cca_ -> flashOff();
}

bool AMCameraExHolder::autoFocusOn()
{
	return cca_ -> autoFocusOn();
}

bool AMCameraExHolder::autoFocusOff()
{
	return cca_ -> autoFocusOff();
}

bool AMCameraExHolder::focusPlus()
{
	return cca_ -> focusPlus();
}

bool AMCameraExHolder::focusMinus()
{
	return cca_ -> focusMinus();
}

bool AMCameraExHolder::zoomIn()
{
	return cca_ -> zoomIn();
}

bool AMCameraExHolder::zoomOut()
{
	return cca_ -> zoomOut();
}
