// Native.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "grabber.hpp"

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

__declspec(dllexport) IBaseFilter *GetBaseFilter()
{
	HRESULT hr = S_OK;
	return dynamic_cast<IBaseFilter *>(CGrabber::CreateInstance(0, &hr));
}

__declspec(dllexport) void DeleteBaseFilter(IBaseFilter *baseFilter)
{
	CGrabber * grabber = static_cast<CGrabber *>(baseFilter);
	delete grabber;	
}