#include "stdafx.h"
#include "directshowhelper.hpp"

//Filter List
CFilterList::CFilterList()
{
}

CFilterList::~CFilterList()
{
	filters_.clear();
}

void CFilterList::assign(IGraphBuilder * graph)
{
	ComPtr<IEnumFilters> enumFt;
	graph -> EnumFilters(enumFt.getRef());

	if (enumFt.isValid())
	{
		HRESULT hr = S_OK;
		do
		{
			ComPtr<IBaseFilter> filter;
			ULONG fetched = 0;
			hr = enumFt -> Next(1, filter.getRef(), &fetched);
			if (filter.isValid())
			{
				filters_.push_back(filter);
			}
		}
		while (hr = S_OK);
	}
}

IBaseFilter * CFilterList::operator [] (int index)
{
	ASSERT(index < filters_.size());

	return filters_[index].get();
}

//PinList
CPinList::CPinList()
{
}

CPinList::~CPinList()
{
	pins_.clear();
}

void CPinList::assign(IBaseFilter * filter)
{
	ComPtr<IEnumPins> enumPn;
	filter -> EnumPins(enumPn.getRef());

	if (enumPn.isValid())
	{
		HRESULT hr = S_OK;
		do
		{
			ComPtr<IPin> pin;
			ULONG fetched = 0;
			hr = enumPn -> Next(1, pin.getRef(), &fetched);
			if (pin.isValid())
			{
				pins_.push_back(pin);
			}
		}
		while (hr = S_OK);
	}
}

IPin * CPinList::operator [] (int index)
{
	ASSERT(index < pins_.size());

	return pins_[index].get();
}

//DirectShowHelper
void CDSHelper::clearGraph(IGraphBuilder * graph, IBaseFilter * filter)
{
	CFilterList filters;
	filters.assign(graph);
	for (size_t i = 0; i < filters.count(); ++i)
	{
		CPinList pins;
		pins.assign(filters[i]);

		for (size_t j = 0; j < pins.count(); ++j)
		{
			ComPtr<IPin> pin;
			HRESULT hr = pins[j] -> ConnectedTo(pin.getRef());
			if (pin.isValid() && SUCCEEDED(hr))
			{
				pins[j] -> Disconnect();
			}
		}
		if (filter != filters[i])
		{
			graph -> RemoveFilter(filters[i]);
		}
	}
}

CAMCameraControlAdapter::CAMCameraControlAdapter(IAMCameraControl * control) : 
	control_(control), handle_(INVALID_HANDLE_VALUE)
{
	handle_ = CreateFile(_T("CIF1:"), 0x80000000, 0, 0, 3, 0, 0);
    if (!IsValidHandle())
    {
		handle_ = CreateFile(_T("CAM1:"), 0x80000000, 0, 0, 3, 0, 0);
    }
}

bool CAMCameraControlAdapter::IsValidHandle()
{
	return !!handle_ && handle_ != INVALID_HANDLE_VALUE;
}

bool CAMCameraControlAdapter::setFocus(__int32 v)
{
    __int32 propdat[2];

	propdat[0] = 0x0f;
    propdat[1] = v;

    DWORD r = 0;
    BOOL res = DeviceIoControl(handle_, 0x90002018, &propdat[0], 8, 0, 0, &r, 0);

	return res != 0;
}

bool CAMCameraControlAdapter::setFlash(__int32 v)
{
	DWORD r = 0;
    BOOL res = DeviceIoControl(handle_, 0x90002024, &v, 4, 0, 0, &r, 0);

	return res != 0;
}

CAMCameraControlAdapter::~CAMCameraControlAdapter()
{
	if (IsValidHandle())
	{
		CloseHandle(handle_);
		handle_ = INVALID_HANDLE_VALUE;
	}
}

bool CAMCameraControlAdapter::flashOn()
{
	if (IsValidHandle() && setFlash(1))
    {
        return true;
    }

    if (!control_)
    {
        return false;
    }
    else
    {
        long min, max, delta, def, flags;
		HRESULT hr = control_ -> GetRange(CameraControl_Flash, &min, &max, &delta, &def, &flags);

        if (FAILED(hr))
        {
            return false;
        }

		hr = control_ -> Set(CameraControl_Flash, max, CameraControl_Flags_Manual);

        if (FAILED(hr))
        {
            return false;
        }
        return true;
    }
}

bool CAMCameraControlAdapter::flashOff()
{
	if (IsValidHandle() && setFlash(2))
    {
        return true;
    }

    if (!control_)
    {
        return false;
    }
    else
    {
        long min, max, delta, def, flags;
		HRESULT hr = control_ -> GetRange(CameraControl_Flash, &min, &max, &delta, &def, &flags);

        if (FAILED(hr))
        {
            return false;
        }

		hr = control_ -> Set(CameraControl_Flash, min, CameraControl_Flags_Manual);

        if (FAILED(hr))
        {
            return false;
        }
        return true;
    }
}

bool CAMCameraControlAdapter::autoFocusOn()
{
    if (IsValidHandle() && setFocus(1))
    {
        return true;
    }

    if (!control_)
    {
        return false;
    }
    else
    {
        long min, max, delta, def, flags;
		HRESULT hr = control_ -> GetRange(CameraControl_Focus, &min, &max, &delta, &def, &flags);

        if (FAILED(hr))
        {
            return sphm840AutoFocusOn();
        }

		hr = control_ -> Set(CameraControl_Focus, def, CameraControl_Flags_Auto);

        if (FAILED(hr))
        {
            return sphm840AutoFocusOn();
        }
        return true;
    }
}

bool CAMCameraControlAdapter::autoFocusOff()
{
    if (IsValidHandle() && setFocus(2))
    {
        return true;
    }

    if (!control_)
    {
        return false;
    }
    else
    {
        long min, max, delta, def, flags;
		HRESULT hr = control_ -> GetRange(CameraControl_Focus, &min, &max, &delta, &def, &flags);

        if (FAILED(hr))
        {
            return sphm840AutoFocusOff();
        }

		hr = control_ -> Set(CameraControl_Focus, def, CameraControl_Flags_Manual);

        if (FAILED(hr))
        {
            return sphm840AutoFocusOff();
        }
        return true;
    }
}

bool CAMCameraControlAdapter::sphm840AutoFocusOff()
{
    HRESULT hr = control_ -> Set(CameraControl_Flash + 1, 0, CameraControl_Flags_Manual);
    if (FAILED(hr))
    {
        hr = control_ -> Set(CameraControl_Zoom, 704, CameraControl_Flags_Manual);
    }

	return SUCCEEDED(hr);
}

bool CAMCameraControlAdapter::sphm840AutoFocusOn()
{
    HRESULT hr = control_ -> Set(CameraControl_Flash + 1, 3, CameraControl_Flags_Manual);

    if (FAILED(hr))
    {
        hr = control_ -> Set(CameraControl_Zoom, 705, CameraControl_Flags_Auto);
    }

    return SUCCEEDED(hr);
}

bool CAMCameraControlAdapter::focusPlus()
{
	return false;
}

bool CAMCameraControlAdapter::focusMinus()
{
	return false;
}

bool CAMCameraControlAdapter::zoomIn()
{
	return false;
}

bool CAMCameraControlAdapter::zoomOut()
{
	return false;
}