#pragma once

#include <streams.h>
#include <vector>
#include "comptr.hpp"

struct CFilterList
{
	CFilterList();
	~CFilterList();
	void assign(IGraphBuilder * graph);
	IBaseFilter * operator [] (int index);
	size_t count() const
	{
		return filters_.size();
	}
private:
	std::vector<ComPtr<IBaseFilter> > filters_;
};

struct CPinList
{
	CPinList();
	~CPinList();
	void assign(IBaseFilter * filter);
	IPin * operator [] (int index);
	size_t count() const
	{
		return pins_.size();
	}
private:
	std::vector<ComPtr<IPin> > pins_;
};

struct CDSHelper
{
	static void clearGraph(IGraphBuilder * graph, IBaseFilter * filter);
};

class CAMCameraControlAdapter
{
public:
	CAMCameraControlAdapter(IAMCameraControl * control);
	~CAMCameraControlAdapter();
public:
	bool flashOn();
	bool flashOff();
	bool autoFocusOn();
	bool autoFocusOff();
	bool focusPlus();
	bool focusMinus();
	bool zoomIn();
	bool zoomOut();
private:
	bool IsValidHandle();
	bool setFocus(__int32 v);
	bool setFlash(__int32 v);
	bool sphm840AutoFocusOn();
	bool sphm840AutoFocusOff();
private:
	IAMCameraControl * control_;
	HANDLE handle_;
};