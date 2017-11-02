using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Utils;
using DirectShowNETCF.PInvoke;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.Helper;

namespace DirectShowNETCF
{
    public class NullCamera : IDisposable
    {
        #region PInvoke

        [DllImport("NullRenderer.dll")]
        private static extern IntPtr GetNullRenderer();

        [DllImport("NullRenderer.dll")]
        private static extern void DeleteNullRenderer(IntPtr grabber);
        
        #endregion

        #region Declarations

        private IBaseFilter video = null;
        private IBaseFilter renderer = null;
        private ICaptureGraphBuilder2 capGraph = null;
        private IGraphBuilder graph = null;
        private IMediaControl control = null;
        private INullGrabber grabber = null;

        private IntPtr nullRenderer = IntPtr.Zero;

        private int width_ = 0;
        private int height_ = 0;

        #endregion

        #region Events

        public event EventHandler<FrameEventArgs> GotFrame;

        #endregion

        public NullCamera()
        {
            object obj = null;
            #region FilterGraph
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            graph = (IGraphBuilder)obj;
            if (graph == null)
            {
                return;
            }
            #endregion
            obj = null;
            #region CaptureGraph
            clsid = CLSID_.CaptureGraphBuilder;
            riid = IID_.ICaptuGraphBuilder2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            capGraph = (ICaptureGraphBuilder2)obj;
            if (capGraph == null)
            {
                Marshal.ReleaseComObject(graph);
                graph = null;
                return;
            }
            #endregion
            obj = null;

            int hr = capGraph.SetFiltergraph(graph);
            if (hr < 0)
            {
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                return;
            }

            if (!getVideoCaptureFilter())
            {
                Marshal.ReleaseComObject(video);
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                video = null;
                return;
            }

            hr = graph.AddFilter(video, "Video Capture");
            if (hr < 0)
            {
                Marshal.ReleaseComObject(video);
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                video = null;
                return;
            }
        }

        public bool init()
        {
            if (video == null)
            {
                return false;
            }

            int hr = 0;

            if (!getNullRenderer())
            {
                return false;
            }

            hr = graph.AddFilter(renderer, "Video Renderer");
            if (hr < 0)
            {
                return false;
            }

            hr = capGraph.RenderStream(null, null, video, null, renderer);
            if (hr < 0)
            {
                return false;
            }

            control = (IMediaControl)graph;
            width_ = grabber.getWidth();//
            height_ = grabber.getHeight(); //grabber.getRect(out width_, out height_);
            grabber.registerCallback(OnFrame);
            return true;
        }

        public bool run()
        {
            if (control == null)
            {
                return false;
            }

            return control.Run() > -1;
        }

        public void stop()
        {
            if (control != null)
            {
                control.Stop();
            }
        }

        public void release()
        {
            if (control != null)
            {
                control.Stop();
            }

            if (graph != null)
            {
                DirectShowHelper.clearGraph(graph, null);
            }

            if (capGraph != null)
            {
                Marshal.ReleaseComObject(capGraph);
                capGraph = null;
            }

            if (graph != null)
            {
                Marshal.ReleaseComObject(graph);
                graph = null;
                control = null;
            }

            if (video != null)
            {
                Marshal.ReleaseComObject(video);
                video = null;
            }

            DeleteNullRenderer(nullRenderer);
            nullRenderer = IntPtr.Zero;
            renderer = null;
            grabber = null;
        }

        public void Dispose()
        {
            release();
        }

        private bool getVideoCaptureFilter()
        {
            object obj = null;
            Guid clsid = CLSID_.VideoCapture;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            if (obj == null)
            {
                return false;
            }
            video = (IBaseFilter)obj;
            obj = null;
            string name_ = "";
            if (!getName(ref name_))
            {
                return false;
            }

            IPersistPropertyBag propBag = (IPersistPropertyBag)video;
            if (propBag == null)
            {
                return false;
            }

            CPropertyBag bag = new CPropertyBag();
            object oname = name_;
            bag.Write("VCapName", ref oname);
            int hr = propBag.Load(bag, null);
            if (hr < 0)
            {
                return false;
            }

            return true;

        }

        private bool getNullRenderer()
        {
            nullRenderer = GetNullRenderer();

            if (nullRenderer == IntPtr.Zero)
            {
                return false;
            }

            object tmp = Marshal.GetTypedObjectForIUnknown(nullRenderer, typeof(IBaseFilter));
            renderer = (IBaseFilter)tmp;
            tmp = Marshal.GetTypedObjectForIUnknown(nullRenderer, typeof(INullGrabber));
            grabber = (INullGrabber)tmp;
            return true;
        }

        private bool getName(ref string name)
        {
            IntPtr handle = IntPtr.Zero;
            IntPtr guid = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Guid)));
            Marshal.StructureToPtr(CLSID_.Camera, guid, false);
            DEVMGR_DEVICE_INFORMATION di = new DEVMGR_DEVICE_INFORMATION();
            di.dwSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(DEVMGR_DEVICE_INFORMATION));
            handle = PInvokes.FindFirstDevice(3, guid, ref di);
            Marshal.FreeHGlobal(guid);
            if ((handle == IntPtr.Zero) || (di.hDevice == IntPtr.Zero))
            {
                return false;
            }

            PInvokes.FindClose(handle);
            name = di.szLegacyName;
            return true;
        }

        public void OnFrame(IntPtr ptr)
        {
            if (GotFrame != null)
            {
                GotFrame(this, new FrameEventArgs(ptr, width_, height_));
            }
        }
    }
}
