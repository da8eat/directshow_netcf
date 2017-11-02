#include <stdafx.h>
#include "enummt.hpp"

CEnumMT::CEnumMT()
{
}

CEnumMT::~CEnumMT()
{
	clear();
}

void CEnumMT::assign(ICaptureGraphBuilder2 * device, IBaseFilter * filter, GUID category)
{
	clear();
	ComPtr<IPin> pin;
	HRESULT hr = device -> FindPin(filter, PINDIR_OUTPUT, &category, &MEDIATYPE_Video, true, 0, pin.getRef());
	if (SUCCEEDED(hr))
	{
		ComPtr<IEnumMediaTypes> enumMt;
		pin -> EnumMediaTypes(enumMt.getRef());
		if (!enumMt.isValid())
		{
			return;
		}

		ULONG fetched = 0;

		while (hr == S_OK)
		{
			fetched = 0;
			AM_MEDIA_TYPE * mt = 0;
            
			hr = enumMt -> Next(1, &mt, &fetched);
			if (hr == S_OK)
			{
				types_.push_back(CMediaType(*mt));
			}
		}
	}
}

CMediaType & CEnumMT::operator [] (int index)
{
	ASSERT(index < types_.size());
	return types_[index];
}

__int32 CEnumMT::getMediaDescription(__int32 index)
{
	ASSERT(index < types_.size());
	__int32 result;
	if (IsEqualGUID(types_[index].formattype, FORMAT_VideoInfo))
	{
		VIDEOINFOHEADER * vih = reinterpret_cast<VIDEOINFOHEADER *>(types_[index].pbFormat);
		result = (vih -> bmiHeader.biWidth & 0xFFFF) << 16;
		result += (vih -> bmiHeader.biHeight & 0xFFFF);
	}

	return result;
}

size_t CEnumMT::count()
{
	return types_.size();
}

void CEnumMT::clear()
{
	types_.clear();
}