// NullRenderer.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "nullrenderer2.hpp"

BOOL __stdcall DllMain( HANDLE hModule, 
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

__declspec(dllexport) IBaseFilter * GetNullRenderer()
{
	HRESULT hr = S_OK;
	return static_cast<IBaseFilter *>(CNullRenderer::CreateInstance(0, &hr));
}

__declspec(dllexport) void DeleteNullRenderer(IBaseFilter *nullrenderer)
{
	CBaseFilter * renderer = static_cast<CNullRenderer *>(nullrenderer);
	delete renderer;
}