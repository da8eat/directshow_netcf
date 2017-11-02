using System;
using System.Collections.Generic;
using System.Text;
using DirectShow;
using System.Runtime.InteropServices;

using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Imports;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;

namespace DirectShow
{
    public class PlayerModel : IDisposable
    {
        #region private declarations

        private IGraphBuilder _graphBuilder = null;
        private IVideoWindow _videoWindow = null;
        private IMediaControl _mediaControl = null;
        private IBaseFilter _sourceFilter = null;
        private IBaseFilter _renderer = null;
        private IMediaEventEx _mediaEventEx = null;
        private IMediaSeeking _mediaSeeking = null;
        private IBasicAudio _basicAudio = null;
        private IBasicVideo _basicVideo = null;
        private IBaseFilter _decoder = null;
        private object _locker = new object();

        #endregion

        #region Events

        public event EventHandler MediaFailed;

        #endregion

        public PlayerModel()
        {
        }

        public void Dispose()
        {
            Clear(true);
        }

        public void LoadFile(string filePath)
        {
            Clear(true);
            InitGraph();

            if (_graphBuilder == null)
            {
                OnMediaFailed();
                return;
            }
            
            //for wmv/wma/mp3 and some network files graph can find soure filter
            //without problem by it self, otherwise we will ask it to render files as we want
            do
            {
                int hr = 0;
                hr = _graphBuilder.RenderFile(filePath, null);
                if (hr > -1)
                {
                    LoggerDSCF.Logger.Instance.WriteLog("Rendered correct");
                    _videoWindow = (IVideoWindow)_graphBuilder;
                    break;
                }
                LoggerDSCF.Logger.Instance.WriteLog("Lets try render file manualy");
                Marshal.ReleaseComObject(_graphBuilder);
                _graphBuilder = null;
                InitGraph();

                InitSource();
                hr = _graphBuilder.AddFilter(_sourceFilter, filePath);
                if (hr < 0)
                {
                    LoggerDSCF.Logger.Instance.WriteLog("Cannot add source to graph...");
                    OnMediaFailed();
                    return;
                }

                LoggerDSCF.Logger.Instance.WriteLog("Filter Added");
                hr = (_sourceFilter as IFileSourceFilter).Load(filePath, null);
                if (hr < 0)
                {
                    LoggerDSCF.Logger.Instance.WriteLog("Cannot load file...");
                    OnMediaFailed();
                    return;
                }

                /*
                InitRenderer();

                hr = _graphBuilder.AddFilter(_renderer, "VideoRenderer");
                if (hr < 0)
                {
                    OnMediaFailed();
                    return;
                }
                */
                bool res_ = DirectShowHelper.RenderPins(_graphBuilder, _sourceFilter); //DirectShowHelper.ConnectPins(_graphBuilder, _sourceFilter, _splitter);

                if (!res_)
                {
                    LoggerDSCF.Logger.Instance.WriteLog("Cannot render pin :(");
                  //  OnMediaFailed();
                  //  return;
                }

                if (filePath.ToUpper().IndexOf(".MP3") != -1)
                {
                    LoggerDSCF.Logger.Instance.WriteLog("Load dmo decoder");
                    InitDecoder();
                    if (_decoder == null)
                    {
                        LoggerDSCF.Logger.Instance.WriteLog("cannot init dmo decoder");
                        OnMediaFailed();
                        return;
                    }

                    LoggerDSCF.Logger.Instance.WriteLog("inited");

                    hr = _graphBuilder.AddFilter(_decoder, "DMO_Decoder");
                    if (hr < 0)
                    {
                        LoggerDSCF.Logger.Instance.WriteLog("Cannot add decoder to graph...");
                        OnMediaFailed();
                        return;
                    }

                    LoggerDSCF.Logger.Instance.WriteLog("decoder added to graph...");

                    if (!DirectShowHelper.ConnectPins(_graphBuilder, _sourceFilter, _decoder))
                    {
                        LoggerDSCF.Logger.Instance.WriteLog("Cannot connect source filter and decoder");
                        OnMediaFailed();
                        return;
                    }

                    LoggerDSCF.Logger.Instance.WriteLog("decoder and source connected");

                    if (!DirectShowHelper.RenderPins(_graphBuilder, _decoder))
                    {
                        LoggerDSCF.Logger.Instance.WriteLog("Cannot connect decoder pins");
                        OnMediaFailed();
                        return;
                    }
                    LoggerDSCF.Logger.Instance.WriteLog("Everything done correct");

                }
            }
            while (false);

            if (_videoWindow == null)
            {
                CFilterList filters = new CFilterList();
                filters.Assign(_graphBuilder);
                for (int i = 0; i < filters.Count; ++i)
                {
                    if (filters[i] is IVideoWindow)
                    {
                        _videoWindow = (IVideoWindow)filters[i];
                    }
                }

                filters.Free();
                filters = null;
            }

            _mediaEventEx = (IMediaEventEx)_graphBuilder;
            _mediaControl = (IMediaControl)_graphBuilder;
            _mediaSeeking = (IMediaSeeking)_graphBuilder;
            _basicAudio = (IBasicAudio)_graphBuilder;
            _basicVideo = (IBasicVideo)_graphBuilder;

            Seek(TimeSpan.Zero);
        }

        public void Play()
        {
            if (_mediaControl == null)
            {
                OnMediaFailed();
                return;
            }

            int hr = _mediaControl.Run();

            if (hr < 0)
            {
                OnMediaFailed();
                return;
            }
        }

        public void Pause()
        {
            if (_mediaControl != null)
            {
                _mediaControl.Pause();
            }
        }

        public void Stop()
        {
            Clear(true);
        }

        public void Seek(TimeSpan position)
        {
            if (_mediaSeeking == null)
            {
                return;
            }

            long stopPos = 0;
            long current = position.Ticks;
            lock (_locker)
            {
                _mediaSeeking.SetPositions(ref current, (uint)AmSeeking.AbsolutePositioning, ref stopPos, (uint)AmSeeking.NoPositioning);
            }
        }

        public int GetVolume()
        {
            int result = 0;
            if (_basicAudio != null)
            {
                _basicAudio.get_Volume(out result);
                result += 10000;
            }
            return result;
        }

        public void SetVolume(int volume)
        {
            if (_basicAudio != null)
            {
                _basicAudio.put_Volume(volume - 10000);
            }
        }

        public int GetBalance()
        {
            int result = 0;
            if (_basicAudio != null)
            {
                _basicAudio.get_Balance(out result);
            }
            return result;
        }

        public void SetBalance(int balance)
        {
            if (_basicAudio != null)
            {
                _basicAudio.put_Balance(balance);
            }
        }

        public int GetWidth()
        {
            int result = 0;
            if (_basicVideo != null)
            {
                _basicVideo.get_VideoWidth(out result);
            }
            return result;
        }

        public int GetHeight()
        {
            int result = 0;
            if (_basicVideo != null)
            {
                _basicVideo.get_VideoHeight(out result);
            }
            return result;
        }

        public int GetBitRate()
        {
            int result = 0;
            if (_basicVideo != null)
            {
                _basicVideo.get_BitRate(out result);
            }
            return result;
        }

        public TimeSpan GetCurrentPosition()
        {
            TimeSpan currentPosition = TimeSpan.Zero;

            lock (_locker)
            {
                if (_mediaSeeking != null)
                {
                    long current;
                    int hr = _mediaSeeking.GetCurrentPosition(out current);
                    if (hr > -1)
                    {
                        currentPosition = new TimeSpan(current);
                    }
                }
            }

            return currentPosition;
        }

        public void SetEventHendler(IntPtr handle)
        {
            _mediaEventEx.SetNotifyWindow(handle, (int)NotifyMessages.WM_GRAPHNOTIFY, IntPtr.Zero);
        }

        public bool SetVideoWindow(IntPtr handle, int x, int y, int width, int height)
        {
            if (_videoWindow != null)
            {
                int hr =_videoWindow.put_Owner(handle);
                hr = _videoWindow.SetWindowPosition(x, y, width, height);
                hr = _videoWindow.put_WindowStyle(0x40000000 | 0x02000000);
                hr = _videoWindow.put_MessageDrain(handle);
                hr = _videoWindow.put_Visible(-1);
                return hr > -1;
            }
            return false;
            
            //filters.Free();
            //filters = null;
        }

        public void Resize(int width, int height)
        {
            if (_videoWindow != null)
            {
                _videoWindow.SetWindowPosition(0, 0, width, height);
            }
        }

        public void SetVisible(bool visible)
        {
            if (_videoWindow != null)
            {
                _videoWindow.put_Visible(visible ? 0 : -1);
            }
        }

        public int GetEvent(out int evCode, out int evParam1, out int evParam2)
        {
            if (_mediaEventEx == null)
            {
                evCode = 0;
                evParam1 = 0;
                evParam2 = 0;
                return -1;
            }

            return _mediaEventEx.GetEvent(out evCode, out evParam1, out evParam2, 0);
        }

        public int FreeEventParams(int evCode, int evParam1, int evParam2)
        {
            return _mediaEventEx.FreeEventParams(evCode, evParam1, evParam2);
        }

        public TimeSpan GetDuration()
        {
            long duration;
            int hr = _mediaSeeking.GetDuration(out duration);
            if (hr < 0)
            {
                return TimeSpan.Zero;
            }
            else
            {
                return new TimeSpan(duration);
            }
        }

        #region private methods

        private void InitGraph()
        {
            object obj = null;
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _graphBuilder = (IGraphBuilder)obj;
            obj = null;
        }

        private void InitRenderer()
        {
            object obj = null;
            Guid clsid = CLSID_.VideoRenderer;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _renderer = (IBaseFilter)obj;

            obj = null;
        }

        private void InitSource()
        {
            object obj = null;
            Guid clsid = CLSID_.FileSource;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _sourceFilter = (IBaseFilter)obj;

            obj = null;
        }

        private void InitDecoder()
        {
            object obj = null;
            Guid clsid = CLSID_.DMOWrapperFilter;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            if (obj != null)
            {
                _decoder = (IBaseFilter)obj;
                IDMOWrapperFilter wrapper = (IDMOWrapperFilter)_decoder;

                wrapper.Init(new CGuid(CLSID_.DMO_Mp3), new CGuid(CLSID_.DMOCATEGORY_AUDIO_DECODER));
            }
            obj = null;
        }

        private void OnMediaFailed()
        {
            if (MediaFailed != null)
            {
                MediaFailed(this, EventArgs.Empty);
            }
        }

        private void Clear(bool dispose)
        {
            if (_graphBuilder != null)
            {
                if (_videoWindow != null)
                {
                    _videoWindow.put_Visible(0);
                    _videoWindow.put_Owner(IntPtr.Zero);
                }
                
                if (_mediaControl != null)
                {
                    _mediaControl.Stop();
                }

                if (_mediaEventEx != null)
                {
                    _mediaEventEx.SetNotifyWindow(IntPtr.Zero, (int)NotifyMessages.WM_GRAPHNOTIFY, IntPtr.Zero);
                }

                if (_graphBuilder != null)
                {
                    DirectShowNETCF.Helper.DirectShowHelper.clearGraph(_graphBuilder, null);
                }

                if (_sourceFilter != null)
                {
                    Marshal.ReleaseComObject(_sourceFilter);
                    _sourceFilter = null;
                }

                if (_decoder != null)
                {
                    Marshal.ReleaseComObject(_decoder);
                    _decoder = null;
                }

                if (_renderer != null)
                {
                    Marshal.ReleaseComObject(_renderer);
                    _renderer = null;
                }

                ReleaseInterfaces(dispose);

                if (_graphBuilder != null)
                {
                    Marshal.ReleaseComObject(_graphBuilder);
                    _graphBuilder = null;
                }
            }
        }

        private void ReleaseInterfaces(bool dispose)
        {
            if (dispose)
            {
                Marshal.ReleaseComObject(_videoWindow);
                _videoWindow = null;
                Marshal.ReleaseComObject(_mediaControl);
                _mediaControl = null;
                Marshal.ReleaseComObject(_mediaEventEx);
                _mediaEventEx = null;
                Marshal.ReleaseComObject(_mediaSeeking);
                _mediaSeeking = null;
                Marshal.ReleaseComObject(_basicAudio);
                _basicAudio = null;
                Marshal.ReleaseComObject(_basicVideo);
                _basicVideo = null;
            }
        }

        #endregion
    }
}
