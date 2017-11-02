// Native.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "amcameraexholder.hpp"



BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
			{
				break;
			}
		case DLL_PROCESS_DETACH:
			{
			}
    }
	return true;
}

__declspec(dllexport) __int32 /*WINAPI*/ CreateAMCamera()
{
	AMCameraExHolder * holder = new AMCameraExHolder();
	return reinterpret_cast<__int32>(holder);
}

__declspec(dllexport) void /*WINAPI*/ ReleaseAMCamera(__int32 holder)
{
	if (holder)
	{
		delete reinterpret_cast<AMCameraExHolder *>(holder);
	}
}

__declspec(dllexport) __int32 /*WINAPI*/ InitEx(__int32 holder, __int32 rotationType)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> init(rotationType);
	}

	return -1;
}

__declspec(dllexport) void /*WINAPI*/ ReleaseEx(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> release();
	}
}

__declspec(dllexport) void /*WINAPI*/ StopEx(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> stop();
	}
}

__declspec(dllexport) __int32 /*WINAPI*/ RunEx(__int32 holder, HWND owner)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> run(owner) ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ ResizeEx(__int32 holder, HWND owner, __int32 width, __int32 height)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> resize(owner, width, height) ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ FlashOn(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> flashOn() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ FlashOff(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> flashOff() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ AutoFocusOn(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> autoFocusOn() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ AutoFocusOff(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> autoFocusOff() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ FocusPlus(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> focusPlus() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ FocusMinus(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> focusMinus() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ ZoomIn(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> zoomIn() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ ZoomOut(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> zoomOut() ? 1 : 0;
	}

	return 0;
}

__declspec(dllexport) void /*WINAPI*/ GetRectEx(__int32 holder, __int32 * width, __int32 * height)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> getRect(width, height);
	}
}

__declspec(dllexport) void /*WINAPI*/ ApplyEffect(__int32 holder, __int32 effect)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> applyEffect(effect);
	}
}

__declspec(dllexport) void * /*WINAPI*/ GrabFrame(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> grabFrame();
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ GrabFrame2(__int32 holder, void * buffer)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> grabFrame(buffer) ? 1 : 0;
	}

	return 0;
}
__declspec(dllexport) void /*WINAPI*/ DrawBitmap(__int32 holder, void * ptr, __int32 id, __int32 x, __int32 y, 
		__int32 width, __int32 height, __int32 transparent, 
		__int32 r, __int32 g, __int32 b)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> drawBitmap(ptr, id, x, y, width, height, transparent, r, g, b);
	}
}

__declspec(dllexport) void /*WINAPI*/ FixPreviewEx(__int32 holder, __int32 fix)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> fixPreview(fix);
	}
}

__declspec(dllexport) void /*WINAPI*/ EraseBitmap(__int32 holder, __int32 id)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> eraseBitmap(id);
	}
}

__declspec(dllexport) __int32 /*WINAPI*/ GetTypesCount(__int32 holder)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> getTypesCount();
	}

	return 0;
}

__declspec(dllexport) __int32 /*WINAPI*/ GetTypeEx(__int32 holder, __int32 index)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		return camera -> getType(index);
	}

	return 0;
}

__declspec(dllexport) void /*WINAPI*/ SetTypeEx(__int32 holder, __int32 index)
{
	if (holder)
	{
		AMCameraExHolder * camera = reinterpret_cast<AMCameraExHolder *>(holder);
		camera -> setType(index);
	}
}