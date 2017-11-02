using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.PInvoke;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.Helper;

namespace DirectShowNETCF.Camera.AMCameraEx
{
    public enum AMResultEx
    {
        OK,
        CameraFilterError,
        GetGrabberError,
        AddGrabberError,
        ConnectGrabberError_1,
        ConnectGrabberError_2,
        VideoRendererError
    }

    public enum Effets
    {
        None = 0,
        GrayScale = 1,
        Binarization = 2,
        Sepia = 4,
        EdgeDetection = 8,
        CropOval = 16
    }

    public enum RotationType
    {
        None = 0,
        Auto = 1,
        Degree90 = 2,
        Degree90LeftRight = 3
    }

    public class AMCameraEx
    {
        private IBaseFilter video = null;
        private IBaseFilter renderer = null;
        private IBaseFilter AMGrabber = null;
        private ICaptureGraphBuilder2 capGraph = null;
        private IGraphBuilder graph = null;
        private IVideoWindow window = null;
        private IMediaControl control = null;
        private IAMCameraControl camControl = null;
        private IEffectEx frmGrabber = null;
        private IntPtr grabber = IntPtr.Zero;
        private int width_ = 0;
        private int height_ = 0;
        private CEnumMediaTypes enummt = null;
        private AMMediaType current = null;
        private bool preview = true;

        private CAMCameraControlAdapter control_ = null;

        [DllImport("AMCameraEx.Native.dll")]
        private static extern IntPtr GetGrabberEx();

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void DeleteGrabberEx(IntPtr grabber);

        public AMCameraEx()
        {
            LoggerDSCF.Logger.Instance.WriteLog("Begin");
            object obj = null;
            #region FilterGraph
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            graph = (IGraphBuilder)obj;
            if (graph == null)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Create FilterGraph");
                return;
            }
            LoggerDSCF.Logger.Instance.WriteLog("FilterGraph - OK");
            #endregion
            obj = null;
            #region CaptureGraph
            clsid = CLSID_.CaptureGraphBuilder;
            riid = IID_.ICaptuGraphBuilder2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            capGraph = (ICaptureGraphBuilder2)obj;
            if (capGraph == null)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Create GraphBuilder");
                Marshal.ReleaseComObject(graph);
                graph = null;
                return;
            }
            LoggerDSCF.Logger.Instance.WriteLog("GraphBuilder - OK");
            #endregion
            obj = null;

            int hr = capGraph.SetFiltergraph(graph);
            if (hr < 0)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Set FilterGraph");
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                return;
            }
            LoggerDSCF.Logger.Instance.WriteLog("Set FIlterGraph - OK");
            if (!getVideoCaptureFilter())
            {
                Marshal.ReleaseComObject(video);
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                video = null;
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Init Camera Filter");
                return;
            }
            LoggerDSCF.Logger.Instance.WriteLog("Init Camera FIlter - OK");
            hr = graph.AddFilter(video, "Video Capture");
            if (hr < 0)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Add filter to graph");
                Marshal.ReleaseComObject(video);
                Marshal.ReleaseComObject(graph);
                Marshal.ReleaseComObject(capGraph);
                graph = null;
                capGraph = null;
                video = null;
                return;
            }
            LoggerDSCF.Logger.Instance.WriteLog("Add Filter - OK");
            enummt = new CEnumMediaTypes();
            enummt.Assign(capGraph, video, CLSID_.PIN_CATEGORY_PREVIEW);
            if (enummt.Count > 0)
            {
                current = enummt[enummt.Count - 1];
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
            LoggerDSCF.Logger.Instance.WriteLog("End");
        }

        ~AMCameraEx()
        {
            release();

            if (control_ != null)
            {
                control_.Dispose();
                control_ = null;
            }
        }

        public AMResultEx init(RotationType rotationType)
        {
            if (video == null)
            {
                return AMResultEx.CameraFilterError;
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
                return AMResultEx.GetGrabberError;
            }

            int hr = graph.AddFilter(AMGrabber, "Grabber");
            if (hr < 0)
            {
                return AMResultEx.AddGrabberError;
            }

            frmGrabber = (IEffectEx)AMGrabber;
            frmGrabber.doRotate((int)rotationType);

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
                return AMResultEx.ConnectGrabberError_1;
            }

            if (!getVideoRenderer())
            {
                return AMResultEx.VideoRendererError;
            }

            hr = graph.AddFilter(renderer, "Video Renderer");
            if (hr < 0)
            {
                return AMResultEx.VideoRendererError;
            }

            hr = capGraph.RenderStream(null, null, AMGrabber, null, renderer);
            if (hr < 0)
            {
                return AMResultEx.ConnectGrabberError_2;
            }

            control = (IMediaControl)graph;
            frmGrabber.getRect(out width_, out height_);
            height_ = (int)Math.Abs(height_);
            return AMResultEx.OK;
        }

        public void release()
        {
            if (control != null)
            {
                control.Stop();
            }

            if (window != null)
            {
                window.put_Visible(-1);
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
                //Marshal.ReleaseComObject(AMGrabber);
                DeleteGrabberEx(grabber);
                AMGrabber = null;
                grabber = IntPtr.Zero;
            }
        }

        public void stop()
        {
            if (window != null)
            {
                window.put_Visible(-1);
                window.put_Owner(IntPtr.Zero);
            }

            if (control != null)
            {
                control.Stop();
            }
        }

        public void fixPreview(bool fix)
        {
            if (fix)
                frmGrabber.doFixPreview(1);
            else
                frmGrabber.doFixPreview(0);
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
                window.put_Visible(0);
                window.put_MessageDrain(owner);
            }
            else
            {
                return false;
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
                    window.put_Visible(0);
                }
                return true;
            }
            catch
            {
                return false;
            }
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

        public void getRect(out int width, out int height)
        {
            width = width_;
            height = height_;
        }

        public void applyEffect(Effets effect)
        {
            frmGrabber.applyEffect((int)effect);
        }

        public IntPtr grabFrame()
        {
            IntPtr frame_ = PInvokes.LocalAlloc(0x40, 3 * width_ * height_);
            frmGrabber.getFrame(frame_);
            return frame_;
        }

        public bool grabFrame(IntPtr buffer)
        {
            return frmGrabber.getFrame(buffer) != -1;
        }

        /// <summary>
        /// Draws Bitmap on preview
        /// </summary>
        /// <param name="ptr">pointer that contains rgb24 raw bitmap</param>
        /// <param name="id">bitmap id will be used to erase bitmap</param>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <param name="width">bitmap width</param>
        /// <param name="height">bitmap height</param>
        public void drawBitmap(IntPtr ptr, int id, int x, int y, int width, int height)
        {
            frmGrabber.drawBitmap(ptr, id, x, y, width, height, 0, 0, 0, 0);
        }

        /// <summary>
        /// Draws bitmap exclude required colors
        /// </summary>
        /// <param name="ptr">pointer that contains rgb24 raw bitmap</param>
        /// <param name="id">bitmap id will be used to erase bitmap</param>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <param name="width">bitmap width</param>
        /// <param name="height">bitmap height</param>
        /// <param name="r">value of red color</param>
        /// <param name="g">value of greed color</param>
        /// <param name="b">value of blue color</param>
        public void drawTransparentBitmap(IntPtr ptr, int id, int x, int y, int width, int height, int r, int g, int b)
        {
            frmGrabber.drawBitmap(ptr, id, x, y, width, height, 1, r, g, b);
        }

        public void blendBitmap(IntPtr ptr, int id, int x, int y, int width, int height, int blend)
        {
            frmGrabber.drawBitmap(ptr, id, x, y, width, height, 0, blend, 0, 0);
        }
        /// <summary>
        /// Erases earlier drawn bitmap
        /// </summary>
        /// <param name="id">bitmap id</param>
        public void eraseBitmap(int id)
        {
            frmGrabber.eraseBitmap(id);
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
            grabber = GetGrabberEx();
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
            LoggerDSCF.Logger.Instance.WriteLog("Init Video Filter");
            object obj = null;
            Guid clsid = CLSID_.VideoCapture;
            Guid riid = IID_.IBaseFilter;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            if (obj == null)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Create IBaseFilter");
                return false;
            }
            LoggerDSCF.Logger.Instance.WriteLog("IBaseFilter - OK");
            video = (IBaseFilter)obj;
            obj = null;
            string name_ = "";
            if (!getName(ref name_))
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Get Name");
                return false;
            }

            LoggerDSCF.Logger.Instance.WriteLog("Get Name - OK");

            IPersistPropertyBag propBag = (IPersistPropertyBag)video;
            if (propBag == null)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Cannot Get PropertyBag");
                return false;
            }

            LoggerDSCF.Logger.Instance.WriteLog("PropertyBag - OK");

            CPropertyBag bag = new CPropertyBag();
            object oname = name_;
            bag.Write("VCapName", ref oname);
            int hr = propBag.Load(bag, null);
            if (hr < 0)
            {
                LoggerDSCF.Logger.Instance.WriteLog("Load failed");
                //return false;
            }

            LoggerDSCF.Logger.Instance.WriteLog("Load - OK");

            camControl = (IAMCameraControl)video;
            control_ = new CAMCameraControlAdapter(camControl);

            LoggerDSCF.Logger.Instance.WriteLog("Init END");
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
