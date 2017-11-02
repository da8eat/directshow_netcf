#pragma once

#include <streams.h>
#include <vector>
#include "comptr.hpp"

class CEnumMT
{
public:
	CEnumMT();
	~CEnumMT();
public:
	void assign(ICaptureGraphBuilder2 * device, IBaseFilter * filter, GUID category);
	CMediaType & operator [] (int index);
	size_t count();
	void clear();
	__int32 getMediaDescription(__int32 index);
private:
	std::vector<CMediaType> types_;
};