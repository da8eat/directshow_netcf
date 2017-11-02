#pragma once

#include <windows.h>
#include <streams.h>
#include <initguid.h>
#include "inullgrabber.hpp"
#include "frameformat.hpp"

typedef void (WINAPI * SoundLevel)(__int32);

struct ISoundLevel : public IUnknown
{
	virtual HRESULT SetCallback(SoundLevel soundLevel) = 0;
};

struct IFileRecorder : public IUnknown
{
	virtual HRESULT StartRecord(BSTR destPath) = 0;
	virtual HRESULT StopRecord() = 0;
};


class CWaveDumper : 
	public CBaseRenderer,
	public ISoundLevel, 
	public IFileRecorder
{
public:
DECLARE_IUNKNOWN;

	static CWaveDumper * WINAPI CreateInstance(IUnknown * unk, HRESULT * hr);
	~CWaveDumper();

	// IUnknown --
	STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void **ppv);

	// CBaseRenderer overrides
	HRESULT CheckMediaType(const CMediaType * pmt);
	HRESULT SetMediaType(const CMediaType * pmt);
	HRESULT DoRenderSample(IMediaSample * mediaSample);

	//ISoundLevel
	HRESULT SetCallback(SoundLevel soundLevel);

	//IFileRecorder
	HRESULT StartRecord(BSTR destPath);
	HRESULT StopRecord();

private:
	SoundLevel soundLevel_;

};