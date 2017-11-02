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

namespace DirectShowNETCF.Camera.AMCamera
{
    public enum TargetType : int
    {
        RECTANGLE = 0x00000002,
        TARGET = 0x00000004,
        CROSS = 0x00000008
    }

    public enum AMResult
    {
        OK,
        CameraFilterError,
        GetGrabberError,
        AddGrabberError,
        ConnectGrabberError_1,
        ConnectGrabberError_2,
        VideoRendererError
    }

    public class AMCamera
    {
        private IBaseFilter video = null;
        private IBaseFilter renderer = null;
        private IntPtr nullRenderer_ = IntPtr.Zero;
        private IBaseFilter nullRenderer = null;
        private IBaseFilter AMGrabber = null;
        private ICaptureGraphBuilder2 capGraph = null;
        private IGraphBuilder graph = null;
        private IVideoWindow window = null;
        private IMediaControl control = null;
        private IAMCameraControl camControl = null;
        private IGetFrame frmGrabber = null;
        private IntPtr grabber = IntPtr.Zero;
        private int width_ = 0;
        private int height_ = 0;
        private RawFrameFormat format_ = RawFrameFormat.Unknown;
        private CEnumMediaTypes enummt = null;
        private AMMediaType current = null;
        private bool preview = true;
        private bool nullPreview_ = false;

        private CAMCameraControlAdapter control_ = null;

        [DllImport("AMCamera.Native.dll")]
        private static extern IntPtr GetBaseFilter();

        [DllImport("AMCamera.Native.dll")]
        private static extern void DeleteBaseFilter(IntPtr grabber);

        [DllImport("NullRenderer.dll")]
        private static extern IntPtr GetNullRenderer();

        [DllImport("NullRenderer.dll")]
        private static extern void DeleteNullRenderer(IntPtr grabber);

        public AMCamera()
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

            enummt = new CEnumMediaTypes();
            enummt.Assign(capGraph, video, CLSID_.PIN_CATEGORY_PREVIEW);
            if (enummt.Count > 0)
            {
                current = enummt[0];
            }
            else
            {
                preview = false;
                enummt.Assign(capGraph, video, CLSID_.PIN_CATEGORY_CAPTURE);
                if (enummt.Count > 0)
                {
                    current = enummt[enummt.Count - 1];
                }
            }
        }

        ~AMCamera()
        {
            release();

            if (control_ != null)
            {
                control_.Dispose();
                control_ = null;
            }
        }

        public AMResult init(bool nullPreview)
        {
            nullPreview_ = nullPreview;
            if (video == null)
            {
                return AMResult.CameraFilterError;
            }

            if (current != null)
            {
                if (preview)
                {
                    DirectShowHelper.SetMediaType(capGraph, video, current, CLSID_.PIN_CATEGORY_PREVIEW);
                }
                else
                {
                    DirectShowHelper.SetMediaType(capGraph, video, current, CLSID_.PIN_CATEGORY_CAPTURE);
                }
            }

            if (!getGrabber())
            {
                return AMResult.GetGrabberError;
            }

            int hr = graph.AddFilter(AMGrabber, "Grabber");
            if (hr < 0)
            {
                return AMResult.AddGrabberError;
            }

            frmGrabber = (IGetFrame)AMGrabber;

            if (preview)
            {
                hr = capGraph.RenderStream(new CGuid(CLSID_.PIN_CATEGORY_PREVIEW), new CGuid(CLSID_.MEDIATYPE_Video),
                    video, null, AMGrabber); //DirectShowEx.ConnectPins(graph, capGraph, video, AMGrabber, CLSID_.PIN_CATEGORY_STILL);
            }
            else
            {
                hr = capGraph.RenderStream(new CGuid(CLSID_.PIN_CATEGORY_CAPTURE), new CGuid(CLSID_.MEDIATYPE_Video),
                    video, null, AMGrabber); //DirectShowEx.ConnectPins(graph, capGraph, video, AMGrabber, CLSID_.PIN_CATEGORY_STILL);
            }
            if (hr < 0)
            {
                return AMResult.ConnectGrabberError_1;
            }


            if (!getVideoRenderer())
            {
                return AMResult.VideoRendererError;
            }

            if (!nullPreview_)
            {
                hr = graph.AddFilter(renderer, "Video Renderer");
                if (hr < 0)
                {
                    return AMResult.VideoRendererError;
                }

                hr = capGraph.RenderStream(null, null, AMGrabber, null, renderer);
                if (hr < 0)
                {
                    return AMResult.ConnectGrabberError_2;
                }
            }
            else
            {
                hr = graph.AddFilter(nullRenderer, "Video Renderer");
                if (hr < 0)
                {
                    return AMResult.VideoRendererError;
                }

                hr = capGraph.RenderStream(null, null, AMGrabber, null, nullRenderer);
                if (hr < 0)
                {
                    return AMResult.ConnectGrabberError_2;
                }
            }


            control = (IMediaControl)graph;
            frmGrabber.getFrameParams(out width_, out height_, out format_);
            return AMResult.OK;
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

            if (AMGrabber != null)
            {
                Marshal.ReleaseComObject(AMGrabber);
                DeleteBaseFilter(grabber);
                AMGrabber = null;
                frmGrabber = null;
                grabber = IntPtr.Zero;
            }

            if (nullRenderer_ != IntPtr.Zero)
            {
                DeleteNullRenderer(nullRenderer_);
                nullRenderer_ = IntPtr.Zero;
                nullRenderer = null;
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
            if (!nullPreview_)
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
            }

            if (control == null)
            {
                return false;
            }

            bool res_ = control.Run() > -1;
            return res_;
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

        public bool drawTarget(Rect rect, int type)
        {
            if (frmGrabber == null)
            {
                return false;
            }

            return frmGrabber.drawTarget(rect, type) == 0;
        }

        public bool stopDrawTarget()
        {
            if (frmGrabber == null)
            {
                return false;
            }

            return frmGrabber.stopDrawTarget() == 0;
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

        public IntPtr grabFrame(ref long bufSize)
        {
            IntPtr res_ = IntPtr.Zero;
            long size = 0;
            frmGrabber.getSize(out size);
            bufSize = size;
            res_ = PInvokes.LocalAlloc(0x40, (int)size);
            frmGrabber.getFrame(res_);
            return res_;
        }

        public void getParams(out int width, out int height, out RawFrameFormat format)
        {
            width = width_;
            height = height_;
            format = format_;
        }

        public bool getGrayScaleImage(IntPtr scan0)
        {
            return frmGrabber.getGrayScale(scan0) == 0;
        }

        public bool getRgb565(IntPtr scan0)
        {
            return frmGrabber.getRgb(scan0) == 0;
        }

        public void startDrawText(string text)
        {
            ABC.CABC abc = new DirectShowNETCF.ABC.CABC(text);
            frmGrabber.drawText(abc.getText(), abc.Height, abc.Width);
            abc.Dispose();
            abc = null;
        }

        public void stopDrawText()
        {
            frmGrabber.stopDraw();
        }

        public List<string> getMediaTypes()
        {
            List<string> res_ = new List<string>();
            for (int i = 0; i < enummt.Count; i++)
            {
                res_.Add(enummt.GetMediaDescription(i));
            }
            return res_;
        }

        public void setMediaType(int index)
        {
            current = enummt[index];
            stop();
            DirectShowHelper.clearGraph(graph, video);
            //clearGraph(video);
            //stop graph, destroy graph, so on...
        }

        private bool getGrabber()
        {
            grabber = GetBaseFilter();
            if (grabber == IntPtr.Zero)
            {
                return false;
            }

            object tmp = Marshal.GetTypedObjectForIUnknown(grabber, typeof(IBaseFilter));
            AMGrabber = (IBaseFilter)tmp;//Marshal.PtrToStructure(grabber, typeof(IBaseFilter));
            if (AMGrabber == null)
            {
                return false;
            }

            return true;
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
            if (!nullPreview_)
            {
                Guid clsid = CLSID_.VideoRenderer;
                Guid riid = IID_.IBaseFilter;
                PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
                if (obj == null)
                {
                    return false;
                }
                renderer = (IBaseFilter)obj;
                window = (IVideoWindow)renderer;
            }
            else
            {
                // *** null renderer *** //
                nullRenderer_ = GetNullRenderer();

                if (nullRenderer_ == IntPtr.Zero)
                {
                    return false;
                }

                object tmp = Marshal.GetTypedObjectForIUnknown(nullRenderer_, typeof(IBaseFilter));
                nullRenderer = (IBaseFilter)tmp;
            }
            // ********** end *********
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
    }
}
