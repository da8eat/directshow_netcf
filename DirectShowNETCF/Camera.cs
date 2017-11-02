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

namespace DirectShowNETCF.Camera
{
    public enum CaptureType
    {
        Preview,
        Still,
        PreviewStill
    }

    public class Camera
    {
        private IBaseFilter video = null;
        private IAMCameraControl camControl = null;
        private IBaseFilter renderer = null;
        private ICaptureGraphBuilder2 capGraph = null;
        private IGraphBuilder graph = null;
        private IVideoWindow window = null;
        private IMediaControl control = null;
        private IBaseFilter imageStill = null;
        private IImageSinkFilter sink = null;
        private IFileSinkFilter fileSink = null;
        private CaptureType type_ = CaptureType.Preview;
        private CEnumMediaTypes typesEnumerator = new CEnumMediaTypes();
        private AMMediaType current;

        private CAMCameraControlAdapter control_ = null;

        public Camera()
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

            typesEnumerator.Assign(capGraph, video, CLSID_.PIN_CATEGORY_STILL);
            if (typesEnumerator.Count > 0)
            {
                current = typesEnumerator[0];
            }
        }

        ~Camera()
        {
            release();

            if (control_ != null)
            {
                control_.Dispose();
                control_ = null;
            }
        }

        public CaptureType CapType
        {
            get
            {
                return type_;
            }
            set
            {
                type_ = value;
            }
        }

        public bool init()
        {
            if (video == null)
            {
                return false;
            }

            int hr = 0;

            if (type_ == CaptureType.Still || type_ == CaptureType.PreviewStill )
            {
                hr = DirectShowHelper.SetMediaType(capGraph, video, current, CLSID_.PIN_CATEGORY_STILL);
                if (hr < 0)
                {
                    return false;
                }
                
                if (!getImageStillFilter())
                {
                    return false;
                }
                if (graph.AddFilter(imageStill, "Still image filter") < 0)
                {
                    return false;
                }
                hr = capGraph.RenderStream(new CGuid(CLSID_.PIN_CATEGORY_STILL), new CGuid(CLSID_.MEDIATYPE_Video), video, null, imageStill);
                if (hr < 0)
                {
                    return false;
                }
            }

            if (type_ == CaptureType.Preview || type_ == CaptureType.PreviewStill)
            {
                if (!getVideoRenderer())
                {
                    return false;
                }

                hr = graph.AddFilter(renderer, "Video Renderer");
                if (hr < 0)
                {
                    return false;
                }
                hr = capGraph.RenderStream(new CGuid(CLSID_.PIN_CATEGORY_PREVIEW), new CGuid(CLSID_.MEDIATYPE_Video),
                    video, null, renderer);
                if (hr < 0)
                {
                    return false;
                }
            }

            control = (IMediaControl)graph;
            return true;
        }

        public void release()
        {
            if (control != null)
            {
                control.Stop();
            }

            if (window != null)
            {
                window.put_Visible(0);
                window.put_Owner(IntPtr.Zero);
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

            if (renderer != null)
            {
                Marshal.ReleaseComObject(renderer);
                renderer = null;
                window = null;
            }

            if (imageStill != null)
            {
                Marshal.ReleaseComObject(imageStill);
                sink = null;
                fileSink = null;
                imageStill = null;
            }
        }

        public void stop()
        {
            if (window != null)
            {
                window.put_Visible(0);
                window.put_Owner(IntPtr.Zero);
            }

            if (control != null)
            {
                control.Stop();
            }

        }

        public bool run(IntPtr owner)
        {
            if (window != null)
            {
                Rect rc = new Rect();
                PInvokes.GetClientRect(owner, out rc);
                window.put_Owner(owner);
                window.SetWindowPosition(rc.Left, rc.Top, rc.Right, rc.Bottom);
                window.put_WindowStyle(0x40000000 | 0x02000000);
                window.put_Visible(-1);
            }
            else
            {
                return false;
            }

            if (control == null)
            {
                return false;
            }

            return control.Run() > -1;
        }

        public bool resize(IntPtr owner, int width, int height)
        {
            try
            {
                if (window != null)
                {
                    window.put_Owner(owner);
                    window.SetWindowPosition(0, 0, width, height);
                    window.put_WindowStyle(0x40000000 | 0x02000000);
                    window.put_Visible(-1);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool run()
        {
            if (control == null)
            {
                return false;
            }

            return control.Run() > -1;
        }

        /// <summary>
        /// Turns on flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOn()
        {
            return control_.flashOn();
        }

        /// <summary>
        /// Turns off flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOff()
        {
            return control_.flashOff();
        }

        /// <summary>
        /// turns On autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOn()
        {
            return control_.autoFocusOn();
        }

        /// <summary>
        /// turns Off autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOff()
        {
            return control_.autoFocusOff();
        }

        /// <summary>
        /// Focus +, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusPlus()
        {
            return control_.focusPlus();
        }

        /// <summary>
        /// Focus -, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusMinus()
        {
            return control_.focusMinus();
        }

        /// <summary>
        /// Zooms +
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomIn()
        {
            return control_.zoomIn();
        }

        /// <summary>
        /// Zooms -
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomOut()
        {
            return control_.zoomOut();
        }

        public bool stillImage(string tmpPath)
        {
            if (type_ == CaptureType.Preview || fileSink == null)
            {
                return false;
            }

            IPin pin_ = null;
            IAMVideoControl videoControl = null;
            if (fileSink.SetFileName(tmpPath, null) < 0)
            {
                return false;
            }
            
            CGuid guid = new CGuid(CLSID_.PIN_CATEGORY_STILL);
            CGuid guid2 = new CGuid(CLSID_.MEDIATYPE_Video);
            if (capGraph.FindPin(video, PinDirection.Output, guid, guid2, false, 0, out pin_) < 0)
            {
                return false;
            }
            
            videoControl = (IAMVideoControl)video;
            if (videoControl.SetMode(pin_, 8) < 0)
            {
                videoControl = null;
                return false;
            }
            videoControl = null;
            Marshal.ReleaseComObject(pin_);
            return true;
        }

        public void setMediaType(int index)
        {
            current = typesEnumerator[index];
            stop();
            DirectShowHelper.clearGraph(graph, video);
            //clearGraph(video);
            //stop graph, destroy graph, so on...
        }

        public List<string> getMediaTypes()
        {
            List<string> res_ = new List<string>();
            for (int i = 0; i < typesEnumerator.Count; i++)
            {
                res_.Add(typesEnumerator.GetMediaDescription(i));
            }
            return res_;
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
            camControl = (IAMCameraControl)video;
            control_ = new CAMCameraControlAdapter(camControl);
            return true;

        }

        private bool getVideoRenderer()
        {
            object obj = null;
            Guid clsid = CLSID_.VideoRenderer;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            if (obj == null)
            {
                return false;
            }
            renderer = (IBaseFilter)obj;
            window = (IVideoWindow)renderer;
            return true;
        }

        private bool getImageStillFilter()
        {
            object obj = null;
            Guid clsid = CLSID_.IMGSinkFilter;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            if (obj == null)
            {
                return false;
            }
            imageStill = (IBaseFilter)obj;
            sink = (IImageSinkFilter)imageStill;
            fileSink = (IFileSinkFilter)imageStill;
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
            if ((handle == IntPtr.Zero)||(di.hDevice == IntPtr.Zero))
            {
                return false;
            }

            PInvokes.FindClose(handle);
            name = di.szLegacyName;
            return true;
        }
    }
}
