using System;
using System.Runtime.InteropServices;

using DirectShowNETCF.Utils;
using DirectShowNETCF.Enums;
using DirectShowNETCF.Structs;

namespace DirectShowNETCF.Imports
{
    #region Com Imports
    [ComVisible(true), ComImport,
    Guid("56A868A9-0AD4-11ce-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IGraphBuilder
    {
        [PreserveSig]
        int AddFilter(
            [In] IBaseFilter pFilter,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName);

        [PreserveSig]
        int RemoveFilter([In] IBaseFilter pFilter);

        [PreserveSig]
        int EnumFilters([Out] out IEnumFilters ppEnum);

        [PreserveSig]
        int FindFilterByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName,
            [Out] out IBaseFilter ppFilter);

        [PreserveSig]
        int ConnectDirect(
            [In] IPin ppinOut,
            [In] IPin ppinIn,
            [In] IntPtr pmt);

        [PreserveSig]
        int Reconnect([In] IPin ppin);

        [PreserveSig]
        int Disconnect([In] IPin ppin);

        [PreserveSig]
        int SetDefaultSyncSource();

        [PreserveSig]
        int Connect(
            [In] IPin ppinOut,
            [In] IPin ppinIn);

        [PreserveSig]
        int Render([In] IPin ppinOut);

        [PreserveSig]
        int RenderFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFile,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrPlayList);

        [PreserveSig]
        int AddSourceFilter(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFileName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
            [Out] out IBaseFilter ppFilter);


        [PreserveSig]
        int SetLogFile([In] IntPtr hFile);

        [PreserveSig]
        int Abort();

        [PreserveSig]
        int ShouldOperationContinue();
    }

    [ComVisible(true), ComImport,
    Guid("56A868B1-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMediaControl
    {
        [PreserveSig]
        int Run();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int GetState(
            [In] int msTimeout,
            [Out] out int pfs);

        [PreserveSig]
        int RenderFile([In] string strFilename);

        [PreserveSig]
        int AddSourceFilter(
            [In] string strFilename,
            [Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

        [PreserveSig]
        int get_FilterCollection(
            [Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

        [PreserveSig]
        int get_RegFilterCollection(
            [Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

        [PreserveSig]
        int StopWhenReady();
    }

    [ComVisible(true), ComImport,
    Guid("56A868B4-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IVideoWindow
    {
        [PreserveSig]
        int put_Caption([In] string caption);

        [PreserveSig]
        int get_Caption([Out] out string caption);

        [PreserveSig]
        int put_WindowStyle([In] int windowStyle);

        [PreserveSig]
        int get_WindowStyle([Out] out int windowStyle);

        [PreserveSig]
        int put_WindowStyleEx([In] int windowStyleEx);

        [PreserveSig]
        int get_WindowStyleEx(out int windowStyleEx);

        [PreserveSig]
        int put_AutoShow([In] int autoShow);

        [PreserveSig]
        int get_AutoShow([Out] out int autoShow);

        [PreserveSig]
        int put_WindowState([In] int windowState);

        [PreserveSig]
        int get_WindowState([Out] out int windowState);

        [PreserveSig]
        int put_BackgroundPalette([In] int backgroundPalette);

        [PreserveSig]
        int get_BackgroundPalette([Out] out int backgroundPalette);

        [PreserveSig]
        int put_Visible([In] int visible);

        [PreserveSig]
        int get_Visible([Out] out int visible);

        [PreserveSig]
        int put_Left([In] int left);

        [PreserveSig]
        int get_Left([Out] out int left);

        [PreserveSig]
        int put_Width([In] int width);

        [PreserveSig]
        int get_Width([Out] out int width);

        [PreserveSig]
        int put_Top([In] int top);

        [PreserveSig]
        int get_Top([Out] out int top);

        [PreserveSig]
        int put_Height([In] int height);

        [PreserveSig]
        int get_Height([Out] out int height);

        [PreserveSig]
        int put_Owner([In] IntPtr owner);

        [PreserveSig]
        int get_Owner([Out] out IntPtr owner);

        [PreserveSig]
        int put_MessageDrain([In] IntPtr drain);

        [PreserveSig]
        int get_MessageDrain([Out] out IntPtr drain);

        [PreserveSig]
        int get_BorderColor([Out] out int color);

        [PreserveSig]
        int put_BorderColor([In] int color);

        [PreserveSig]
        int get_FullScreenMode([Out] out int fullScreenMode);

        [PreserveSig]
        int put_FullScreenMode([In] int fullScreenMode);

        [PreserveSig]
        int SetWindowForeground([In] int focus);

        [PreserveSig]
        int NotifyOwnerMessage(
            [In] IntPtr hwnd,
            [In] int msg,
            [In] IntPtr wParam,
            [In] IntPtr lParam);

        [PreserveSig]
        int SetWindowPosition(
            [In] int left,
            [In] int top,
            [In] int width,
            [In] int height);

        [PreserveSig]
        int GetWindowPosition(
            [Out] out int left,
            [Out] out int top,
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int GetMinIdealImageSize(
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int GetMaxIdealImageSize(
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int GetRestorePosition(
            [Out] out int left,
            [Out] out int top,
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int HideCursor([In] int hideCursor);

        [PreserveSig]
        int IsCursorHidden([Out] out int hideCursor);

    }

    [ComVisible(true), ComImport,
    Guid("56A86895-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBaseFilter
    {
        [PreserveSig]
        int GetClassID([Out] out Guid pClassID);

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Run([In] long tStart);

        [PreserveSig]
        int GetState(
            [In] int dwMilliSecsTimeout,
            [Out] out int filtState);

        [PreserveSig]
        int SetSyncSource([In, MarshalAs(UnmanagedType.IUnknown)] object pClock);

        /// <summary>
        /// 
        /// </summary> 
        [PreserveSig]
        int GetSyncSource([Out, MarshalAs(UnmanagedType.IUnknown)] out object pClock);

        [PreserveSig]
        int EnumPins([Out] out IEnumPins ppEnum);

        [PreserveSig]
        int FindPin(
            [In, MarshalAs(UnmanagedType.LPWStr)] string Id,
           [Out] out IPin ppPin);

        [PreserveSig]
        int QueryFilterInfo([Out] IntPtr pInfo);

        [PreserveSig]
        int JoinFilterGraph(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pGraph,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pName);

        [PreserveSig]
        int QueryVendorInfo(
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string pVendorInfo);
    }


    [ComVisible(true), ComImport,
    Guid("0000010C-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersist
    {
        [PreserveSig]
        int GetClassID([Out] out Guid pClassID);
    }

    [ComVisible(true), ComImport,
    Guid("37D84F60-42CB-11CE-8135-00AA004BB851"),//Guid("5738E040-B67F-11d0-BD4D-00A0C911CE86"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistPropertyBag : IPersist
    {
        #region IPersist
        [PreserveSig]
        new int GetClassID([Out] out Guid pClassID);
        #endregion

        [PreserveSig]
        int InitNew();

        [PreserveSig]
        int Load(
            [In] IPropertyBag pPropBag,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pErrorLog
            );

        [PreserveSig]
        int Save(
            IPropertyBag pPropBag,
            [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty,
            [In, MarshalAs(UnmanagedType.Bool)] bool fSaveAllProperties
            );
    }

    [ComVisible(true), ComImport,
    Guid("55272A00-42CB-11CE-8135-00AA004BB851"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyBag
    {
        [PreserveSig]
        int Read(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
            [In, Out, MarshalAs(UnmanagedType.Struct)]	ref	object pVar,
            [In] IntPtr pErrorLog);

        [PreserveSig]
        int Write(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
            [In, MarshalAs(UnmanagedType.Struct)] ref object pVar);
    }

    [ComVisible(true), ComImport,
    Guid("93E5A4E0-2D50-11D2-ABFA-00A0C9C6E38D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICaptureGraphBuilder2
    {
        [PreserveSig]
        int SetFiltergraph([In] IGraphBuilder pfg);

        [PreserveSig]
        int GetFiltergraph([Out] out IGraphBuilder ppfg);

        [PreserveSig]
        int SetOutputFileName(
            [In] CGuid pType,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile,
            [Out] out IBaseFilter ppbf,
            [Out] out IFileSinkFilter ppSink);

        [PreserveSig]
        int FindInterface(
            [In] ref CGuid pCategory,
            [In] ref CGuid pType,
            [In] IBaseFilter pbf,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppint);

        [PreserveSig]
        int RenderStream(
            [In] CGuid PinCategory,
            [In] CGuid MediaType,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pSource,
            [In] IBaseFilter pfCompressor,
            [In] IBaseFilter pfRenderer
            );

        [PreserveSig]
        int ControlStream(
            [In] CGuid pCategory,
            [In] CGuid pType,
            [In] IBaseFilter pFilter,
            [In] long pstart,
            [In] long pstop,
            [In] short wStartCookie,
            [In] short wStopCookie);

        [PreserveSig]
        int AllocCapFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrFile,
            [In] long dwlSize);

        [PreserveSig]
        int CopyCaptureFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrOld,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpwstrNew,
            [In] int fAllowEscAbort,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pFilter);

        [PreserveSig]
        int FindPin(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pSource,
            [In] PinDirection pindir,
            [In] CGuid pCategory,
            [In] CGuid pType,
            [In, MarshalAs(UnmanagedType.Bool)] bool fUnconnected,
            [In] int num,
            [Out] out IPin ppPin);
    }

    [ComVisible(true), ComImport,
    Guid("A2104830-7C70-11CF-8BCE-00AA00A3F1A6"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileSinkFilter
    {
        [PreserveSig]
        int SetFileName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
            [In] AMMediaType pmt
            );

        [PreserveSig]
        int GetCurFile(
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string pszFileName,
            [Out] AMMediaType pmt
            );
    }

    [ComVisible(true), ComImport,
    Guid("7f12e45f-b224-449e-907a-a18fca14a579"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IImageSinkFilter
    {
        [PreserveSig]
        int SetQuality([In] uint dwQuality);

        [PreserveSig]
        int SetEncoderParameters(
            [In] uint dwCount,
            [In, MarshalAs(UnmanagedType.Struct)] ref IntPtr Parameter);

    }

    [ComVisible(true), ComImport,
    Guid("56A86892-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumPins
    {
        [PreserveSig]
        int Next(
            [In]						   int cPins,
            [Out]	                       out	IPin ppPins,
            [Out]						   out int pcFetched);

        [PreserveSig]
        int Skip([In] int cPins);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumPins ppEnum);
    }

    [ComVisible(true), ComImport,
    Guid("56A86891-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPin
    {
        [PreserveSig]
        int Connect(
            [In] IPin pReceivePin,
            [In] IntPtr pmt);

        [PreserveSig]
        int ReceiveConnection(
            [In] IPin pReceivePin,
            [In] IntPtr pmt);

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int ConnectedTo([Out] out IPin ppPin);

        [PreserveSig]
        int ConnectionMediaType([Out] out IntPtr pmt);

        [PreserveSig]
        int QueryPinInfo([Out] out PinInfo pInfo);

        [PreserveSig]
        int QueryDirection(out PinDirection pPinDir);

        [PreserveSig]
        int QueryId([Out, MarshalAs(UnmanagedType.LPWStr)] out string Id);

        [PreserveSig]
        int QueryAccept([In] IntPtr pmt);

        [PreserveSig]
        int EnumMediaTypes([Out] out IEnumMediaTypes ppEnum);

        [PreserveSig]
        int QueryInternalConnections(
            IntPtr apPin,
            [In, Out] ref int nPin);

        [PreserveSig]
        int EndOfStream();

        [PreserveSig]
        int BeginFlush();

        [PreserveSig]
        int EndFlush();

        [PreserveSig]
        int NewSegment(long tStart, long tStop, double dRate);
    }

    [ComVisible(true), ComImport,
    Guid("56A86893-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumFilters
    {
        [PreserveSig]
        int Next(
            [In] int cFilters,
            [Out] out	IBaseFilter ppFilter,
            [Out] out int pcFetched);

        [PreserveSig]
        int Skip([In] int cFilters);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumFilters ppEnum);
    }

    [ComVisible(true), ComImport,
    Guid("6A2E0670-28E4-11D0-A18C-00A0C9118956"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMVideoControl
    {
        [PreserveSig]
        int GetCaps(
            [In] IPin pPin,
            [Out] out uint pCapsFlags
            );

        [PreserveSig]
        int SetMode(
            [In] IPin pPin,
            [In] uint Mode
            );

        [PreserveSig]
        int GetMode(
            [In] IPin pPin,
            [Out] out uint Mode
            );

        [PreserveSig]
        int GetCurrentActualFrameRate(
            [In] IPin pPin,
            [Out] out long ActualFrameRate
            );

        [PreserveSig]
        int GetMaxAvailableFrameRate(
            [In] IPin pPin,
            [In] int iIndex,
            [In] Size Dimensions,
            [Out] out long MaxAvailableFrameRate
            );

        [PreserveSig]
        int GetFrameRateList(
            [In] IPin pPin,
            [In] int iIndex,
            [In] Size Dimensions,
            [Out] out int ListSize,
            [Out] out IntPtr FrameRates
            );
    }

    [ComVisible(true), ComImport,
    Guid("89C31040-846B-11CE-97D3-00AA0055595A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumMediaTypes
    {
        [PreserveSig]
        int Next(
            [In] int cMediaTypes,
            [Out] out IntPtr ppMediaTypes,
            [Out] out int pcFetched
            );

        [PreserveSig]
        int Skip([In] int cMediaTypes);

        /// <summary>
        /// процедура вовзращающая в начало списка
        /// </summary> 
        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out] out IEnumMediaTypes ppEnum);
    }

    [ComVisible(true), ComImport,
    Guid("C6E13340-30AC-11D0-A18C-00A0C9118956"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMStreamConfig
    {
        [PreserveSig]
        int SetFormat([In] AMMediaType pmt);

        [PreserveSig]
        int GetFormat([Out] out IntPtr pmt);

        [PreserveSig]
        int GetNumberOfCapabilities(
        [Out] out int piCount,
        [Out] out int piSize);

        [PreserveSig]
        int GetStreamCaps(
            [In] int iIndex,
            [Out] out IntPtr ppmt,
            [In] IntPtr pSCC
            );
    }

    [ComVisible(true), ComImport,
    Guid("C6E13370-30AC-11D0-A18C-00A0C9118956"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMCameraControl
    {
        [PreserveSig]
        int GetRange(
            [In] int Property,
            [Out] out int pMin,
            [Out] out int pMax,
            [Out] out int pSteppingDelta,
            [Out] out int pDefault,
            [Out] out int pCapsFlags);

        [PreserveSig]
        int Set(
            [In] int Property,
            [In] int lValue,
            [In] int Flags);

        [PreserveSig]
        int Get(
            [In] int Property,
            [Out] out int Value,
            [Out] out int Flags);
    }

    [ComVisible(true), ComImport,
    Guid("C6E13360-30AC-11d0-A18C-00A0C9118956"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMVideoProcAmp
    {
        [PreserveSig]
        int GetRange(
            [In] int Property,
            [Out] out int pMin,
            [Out] out int pMax,
            [Out] out int pSteppingDelta,
            [Out] out int pDefault,
            [Out] out int pCapsFlags
            );

        [PreserveSig]
        int Set(
            [In] int Property,
            [In] int lValue,
            [In] int Flags
            );

        [PreserveSig]
        int Get(
            [In] int Property,
            [Out] out int lValue,
            [Out] out int Flags
            );
    }

    [ComVisible(true), ComImport,
    Guid("52D6F586-9F0F-4824-8FC8-E32CA04930C2"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDMOWrapperFilter
    {
        [PreserveSig]
        int Init(
            [In] CGuid clsidDMO,
            [In] CGuid catDMO
            );
    }

    [ComVisible(true), ComImport,
    Guid("56A868B2-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaPosition
    {
        [PreserveSig]
        int get_Duration([Out] out double pLength);

        [PreserveSig]
        int put_CurrentPosition([In] double llTime);

        [PreserveSig]
        int get_CurrentPosition([Out] out double pllTime);

        [PreserveSig]
        int get_StopTime([Out] out double pllTime);

        [PreserveSig]
        int put_StopTime([In] double llTime);

        [PreserveSig]
        int get_PrerollTime([Out] out double pllTime);

        [PreserveSig]
        int put_PrerollTime([In] double llTime);

        [PreserveSig]
        int put_Rate([In] double dRate);

        [PreserveSig]
        int get_Rate([Out] out double pdRate);

        [PreserveSig]
        int CanSeekForward([Out] out int pCanSeekForward);

        [PreserveSig]
        int CanSeekBackward([Out] out int pCanSeekBackward);
    }

    [ComVisible(true), ComImport,
    Guid("2B21644A-D405-4E27-A51C-A4812bE0CE4C"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IGetFrame
    {
        [PreserveSig]
        int getFrame(IntPtr pBuff);

        [PreserveSig]
        int getSize([Out] out long size);

        [PreserveSig]
        int getFrameParams(
            [Out] out int width,
            [Out] out int height,
            [Out] out RawFrameFormat format);

        [PreserveSig]
        int drawText(
            [In] IntPtr ptr,
            [In] int height,
            [In] int width);

        [PreserveSig]
        int stopDraw();

        [PreserveSig]
        int getGrayScale(IntPtr ptr);

        [PreserveSig]
        int getRgb(IntPtr ptr);

        [PreserveSig]
        int drawTarget(Rect rect, int type);

        [PreserveSig]
        int stopDrawTarget();
    }

    [ComVisible(true), ComImport,
    Guid("2B21655B-D405-4E27-A51C-A4812bE0CE4C"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEffectEx
    {
        [PreserveSig]
        int getFrame(IntPtr buffer);

        [PreserveSig]
        int getRect(
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int applyEffect([In] int effect);

        [PreserveSig]
        int drawBitmap(
            IntPtr ptr,
            [In] int id,
            [In] int x,
            [In] int y,
            [In] int width,
            [In] int height,
            [In] int transparent,
            [In] int r,
            [In] int g,
            [In] int b);

        [PreserveSig]
        int eraseBitmap([In]int id);

        [PreserveSig]
        int doRotate([In]int rotationType);

        [PreserveSig]
        int saveJPEG(
            [In] int quality,
            [In, MarshalAs(UnmanagedType.LPStr)] string path);
    }

    [ComImport,
    Guid("56A868C0-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMediaEventEx
    {
        [PreserveSig]
        int GetEventHandle([Out] out IntPtr hEvent);

        [PreserveSig]
        int GetEvent(
            [Out] out int lEventCode,
            [Out] out int lParam1,
            [Out] out int lParam2,
            [In] int msTimeout);

        [PreserveSig]
        int WaitForCompletion(
            [In] int msTimeout,
            [Out] out int pEvCode);

        [PreserveSig]
        int CancelDefaultHandling([In] int lEvCode);

        [PreserveSig]
        int RestoreDefaultHandling([In] int lEvCode);

        [PreserveSig]
        int FreeEventParams(
            [In] int lEventCode,
            [In] int lParam1,
            [In] int lParam2);

        [PreserveSig]
        int SetNotifyWindow(
            [In] IntPtr hwnd,
            [In] int lMsg,
            [In] IntPtr lInstanceData);

        [PreserveSig]
        int SetNotifyWindow([In] int lNoNotifyFlags); //On - 0, Off - 1

        [PreserveSig]
        int GetNotifyFlags([Out] out int lNoNotifyFlags);
    }

    [ComImport,
    Guid("56A868B3-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IBasicAudio
    {
        [PreserveSig]
        int put_Volume([In] int lVolume);

        [PreserveSig]
        int get_Volume([Out] out int plVolume);

        [PreserveSig]
        int put_Balance([In] int lBalance);

        [PreserveSig]
        int get_Balance([Out] out int plBalance);
    }

    [ComImport,
    Guid("56A868B5-0AD4-11CE-B03A-0020AF0BA770"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IBasicVideo
    {
        [PreserveSig]
        int get_AvgTimePerFrame([Out] out double pAvgTimePerFrame);

        [PreserveSig]
        int get_BitRate([Out] out int pBitRate);

        [PreserveSig]
        int get_BitErrorRate([Out] out int pBitRate);

        [PreserveSig]
        int get_VideoWidth([Out] out int pVideoWidth);

        [PreserveSig]
        int get_VideoHeight([Out] out int pVideoHeight);

        [PreserveSig]
        int put_SourceLeft([In] int SourceLeft);

        [PreserveSig]
        int get_SourceLeft([Out] out int pSourceLeft);

        [PreserveSig]
        int put_SourceWidth([In] int SourceWidth);

        [PreserveSig]
        int get_SourceWidth([Out] out int pSourceWidth);

        [PreserveSig]
        int put_SourceTop([In] int SourceTop);

        [PreserveSig]
        int get_SourceTop([Out] out int pSourceTop);

        [PreserveSig]
        int put_SourceHeight([In] int SourceHeight);

        [PreserveSig]
        int get_SourceHeight([Out] out int pSourceHeight);

        [PreserveSig]
        int put_DestinationLeft([In] int DestinationLeft);

        [PreserveSig]
        int get_DestinationLeft([Out] out int pDestinationLeft);

        [PreserveSig]
        int put_DestinationWidth([In] int DestinationWidth);

        [PreserveSig]
        int get_DestinationWidth([Out] out int pDestinationWidth);

        [PreserveSig]
        int put_DestinationTop([In] int DestinationTop);

        [PreserveSig]
        int get_DestinationTop([Out] out int pDestinationTop);

        [PreserveSig]
        int put_DestinationHeight([In] int DestinationHeight);

        [PreserveSig]
        int get_DestinationHeight([Out] out int pDestinationHeight);

        [PreserveSig]
        int SetSourcePosition(
            [In] int left,
            [In] int top,
            [In] int width,
            [In] int height
            );

        [PreserveSig]
        int GetSourcePosition(
            [Out] out int left,
            [Out] out int top,
            [Out] out int width,
            [Out] out int height
            );

        [PreserveSig]
        int SetDefaultSourcePosition();

        [PreserveSig]
        int SetDestinationPosition(
            [In] int left,
            [In] int top,
            [In] int width,
            [In] int height
            );

        [PreserveSig]
        int GetDestinationPosition(
            [Out] out int left,
            [Out] out int top,
            [Out] out int width,
            [Out] out int height
            );

        [PreserveSig]
        int SetDefaultDestinationPosition();

        [PreserveSig]
        int GetVideoSize(
            [Out] out int pWidth,
            [Out] out int pHeight
            );

        [PreserveSig]
        int GetVideoPaletteEntries(
            [In] int StartIndex,
            [In] int Entries,
            [Out] out int pRetrieved,
            [Out] out int[] pPalette
            );

        [PreserveSig]
        int GetCurrentImage(
            [In, Out] ref int pBufferSize,
            [Out] IntPtr pDIBImage // int *
            );

        [PreserveSig]
        int IsUsingDefaultSource();

        [PreserveSig]
        int IsUsingDefaultDestination();
    }

    [ComVisible(true), ComImport,
    Guid("36B73880-C2C8-11CF-8B46-00805F6CEF60"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSeeking
    {
        [PreserveSig]
        int GetCapabilities([Out] out int pCapabilities);

        [PreserveSig]
        int CheckCapabilities([In, Out] ref int pCapabilities); //maybe ref?

        [PreserveSig]
        int IsFormatSupported([In] CGuid pFormat);

        [PreserveSig]
        int QueryPreferredFormat([Out] out Guid pFormat);

        [PreserveSig]
        int GetTimeFormat([Out] out Guid pFormat);

        [PreserveSig]
        int IsUsingTimeFormat([In] CGuid pFormat);

        [PreserveSig]
        int SetTimeFormat([In] CGuid pFormat);

        [PreserveSig]
        int GetDuration([Out] out long pDuration);

        [PreserveSig]
        int GetStopPosition([Out] out long pStop);

        [PreserveSig]
        int GetCurrentPosition([Out] out long pCurrent);

        [PreserveSig]
        int ConvertTimeFormat(
            [Out] out long pTarget,
            [In] CGuid pTargetFormat,
            [In] long Source,
            [In] CGuid pSourceFormat);

        [PreserveSig]
        int SetPositions(
            [In, Out] ref long pCurrent, //ref?
            [In] uint dwCurrentFlags,
            [In, Out] ref long pStop, //ref?
            [In] uint dwStopFlags);

        [PreserveSig]
        int GetPositions(
            [Out] out long pCurrent,
            [Out] out long pStop);

        [PreserveSig]
        int GetAvailable(
            [Out] out long pEarliest,
            [Out] out long pLatest);

        [PreserveSig]
        int SetRate([In] double dRate);

        [PreserveSig]
        int GetRate([Out] out double pdRate);

        [PreserveSig]
        int GetPreroll([Out] out long pllPreroll);
    }

    public delegate void FrameCallback(IntPtr frame);

    [ComVisible(true), ComImport,
    Guid("2B21766C-D405-4E27-A51C-A4812BE0CE4C"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INullGrabber
    {
        [PreserveSig]
        int getRect(
            [Out] out int width,
            [Out] out int height);

        [PreserveSig]
        int registerCallback(
            [MarshalAs(UnmanagedType.FunctionPtr)]
            FrameCallback callback);
    }

    #endregion
}
