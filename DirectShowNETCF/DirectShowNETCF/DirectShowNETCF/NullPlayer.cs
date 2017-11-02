using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Enums;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Guids;
using DirectShowNETCF.PInvoke;

namespace DirectShowNETCF
{
    public class NullPlayer : IDisposable
    {
        #region PInvoke

        [DllImport("NullRenderer.dll")]
        private static extern IntPtr GetNullRenderer();

        [DllImport("NullRenderer.dll")]
        private static extern void DeleteNullRenderer(IntPtr grabber);

        #endregion

        #region Declarations

        private INullGrabber grabber = null;

        private IGraphBuilder _graphBuilder = null;
        private ICaptureGraphBuilder2 _capGraph = null; 
        private IMediaControl _mediaControl = null;
        private IBaseFilter _sourceFilter = null;
        private IBaseFilter _renderer = null;
        private IMediaSeeking _mediaSeeking = null;
        private IBasicAudio _basicAudio = null;
        private object _locker = new object();

        private IntPtr nullRenderer = IntPtr.Zero;

        private int width_ = 0;
        private int height_ = 0;

        #endregion

        #region Events

        public event EventHandler<FrameEventArgs> GotFrame;

        #endregion

        public NullPlayer()
        {
        }

        public bool loadFile(string filePath)
        {
            release();
            initGraph();

            int hr = 0;

            do
            {
                //try to load default source filter
                hr = _graphBuilder.AddSourceFilter(filePath, filePath, out _sourceFilter);

                if (hr > -1)
                {
                    break;
                }

                Marshal.ReleaseComObject(_graphBuilder);
                _graphBuilder = null;
                initGraph();
                initSourceFilter();

                hr = _graphBuilder.AddFilter(_sourceFilter, "SourceFilter");
                if (hr < 0)
                {
                    return false;
                }

                hr = (_sourceFilter as IFileSourceFilter).Load(filePath, null);
                if (hr < 0)
                {
                    return false;
                }
            }
            while (false);

            if (!getNullRenderer())
            {
                return false;
            }

            hr = _graphBuilder.AddFilter(_renderer, "Video Renderer");
            if (hr < 0)
            {
                return false;
            }

            hr = _capGraph.RenderStream(null, null, _sourceFilter, null, _renderer); //DirectShowHelper.ConnectPins(_graphBuilder, _sourceFilter, _renderer); //RenderPins(_graphBuilder, _sourceFilter);
            if (hr <0)
            {
                return false;
            }

            _mediaControl = (IMediaControl)_graphBuilder;
            _mediaSeeking = (IMediaSeeking)_graphBuilder;
            _basicAudio = (IBasicAudio)_graphBuilder;

            //grabber.getRect(out width_, out height_);
            grabber.registerCallback(OnFrame);

            seek(TimeSpan.Zero);

            return true;
        }

        public bool start()
        {
            if (_mediaControl != null)
            {
                return _mediaControl.Run() > -1;
            }
            return false;
        }

        public void stop()
        {
            if (_mediaControl != null)
            {
                _mediaControl.Stop();
            }
        }

        public void seek(TimeSpan position)
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

        public TimeSpan getDuration()
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

        public void release()
        {
            if (_mediaControl != null)
            {
                _mediaControl.Stop();
            }

            if (_graphBuilder != null)
            {
                DirectShowHelper.clearGraph(_graphBuilder, null);
            }

            if (_graphBuilder != null)
            {
                Marshal.ReleaseComObject(_graphBuilder);
                _graphBuilder = null;
            }

            if (_capGraph != null)
            {
                Marshal.ReleaseComObject(_capGraph);
                _capGraph = null;
            }

            if (_sourceFilter != null)
            {
                Marshal.ReleaseComObject(_sourceFilter);
                _sourceFilter = null;
            }

            if (_mediaSeeking != null)
            {
                Marshal.ReleaseComObject(_mediaSeeking);
                _mediaSeeking = null;
            }

            if (_basicAudio != null)
            {
                Marshal.ReleaseComObject(_basicAudio);
                _basicAudio = null;
            }

            if (_mediaControl != null)
            {
                Marshal.ReleaseComObject(_mediaControl);
                _mediaControl = null;
            }

            DeleteNullRenderer(nullRenderer);
            nullRenderer = IntPtr.Zero;
            _renderer = null;
            grabber = null;
        }

        public void Dispose()
        {
            release();
        }

        /// <summary>
        /// set value of volume
        /// </summary>
        /// <param name="volume">value min = 0 (mute), max = 10000</param>
        public void setVolume(int volume)
        {
            if (_basicAudio != null)
            {
                _basicAudio.put_Volume(volume - 10000);
            }
        }

        private bool getNullRenderer()
        {
            nullRenderer = GetNullRenderer();

            if (nullRenderer == IntPtr.Zero)
            {
                return false;
            }

            object tmp = Marshal.GetTypedObjectForIUnknown(nullRenderer, typeof(IBaseFilter));
            _renderer = (IBaseFilter)tmp;
            tmp = Marshal.GetTypedObjectForIUnknown(nullRenderer, typeof(INullGrabber));
            grabber = (INullGrabber)tmp;
            return true;
        }

        private void initGraph()
        {
            object obj = null;

            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _graphBuilder = (IGraphBuilder)obj;
           
            obj = null;

            clsid = CLSID_.CaptureGraphBuilder;
            riid = IID_.ICaptuGraphBuilder2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _capGraph = (ICaptureGraphBuilder2)obj;

            obj = null;

            _capGraph.SetFiltergraph(_graphBuilder);
        }

        private void initSourceFilter()
        {
            object obj = null;
            Guid clsid = CLSID_.FileSource;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            _sourceFilter = (IBaseFilter)obj;
            obj = null;
        }

        private void OnFrame(IntPtr ptr)
        {
            if (GotFrame != null)
            {
                GotFrame(this, new FrameEventArgs(ptr, width_, height_));
            }
        }
    }
}
