using System;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Imports;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;

namespace NullPlayer
{
    /*
         public class NullPlayer : IDisposable
    {
        [DllImport("DirectShowNETCF.Native.dll")]
        private static extern IntPtr GetNullRenderer();

        [DllImport("DirectShowNETCF.Native.dll")]
        private static extern void DeleteNullRenderer(IntPtr grabber);

        IGraphBuilder graph = null;
        IntPtr renderer = IntPtr.Zero;
        IBaseFilter src = null;
        INullGrabber grabber = null;
        int width_ = 0;
        int height_ = 0;
        int index_ = 0;

        private void FireEvent(Bitmap bmp)
        {
            if (GotFrame != null)
            {
                GotFrame(this, new FrameEventArgs(bmp));
            }
        }

        public NullPlayer(string path, TimeSpan position)
        {
            object obj = null;
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            graph = (IGraphBuilder)obj;
            obj = null;

            src = null;

            if (graph.AddSourceFilter(path, "SrcFilter", out src) < 0)
            {
                DestroyGraph(graph);
                FireEvent(null);
                return;
            }

            renderer = GetNullRenderer();
            if (renderer == IntPtr.Zero)
            {
                DestroyGraph(graph);
                FireEvent(null);
                return;
            }

            object tmp = Marshal.GetTypedObjectForIUnknown(renderer, typeof(IBaseFilter));
            IBaseFilter nullRenderer = (IBaseFilter)tmp;
            if (nullRenderer == null)
            {
                DestroyGraph(graph);
                FireEvent(null);
                return;
            }

            if (graph.AddFilter(nullRenderer, "Null Renderer") < 0)
            {
                DestroyGraph(graph);
                FireEvent(null);
                return;
            }

            if (!DirectShowHelper.ConnectPins(graph, src, nullRenderer))
            {
                DestroyGraph(graph);
                FireEvent(null);
                return;
            }

            long stopPos = 0;
            long current = position.Ticks;

            (graph as IMediaControl).Pause();
            (graph as IMediaSeeking).SetPositions(ref current, (uint)AmSeeking.AbsolutePositioning, ref stopPos, (uint)AmSeeking.NoPositioning);

            grabber = (INullGrabber)nullRenderer;

            grabber.getRect(out width_, out height_);
        }

        public event EventHandler<FrameEventArgs> GotFrame;

        public void grabBitmap()
        {
            index_ = 0;
            grabber.registerCallback(OnFrame);
            if (graph != null)
            {
                (graph as IMediaControl).Run();
            }
        }

        public void Dispose()
        {
            if (graph != null)
            {
                (graph as IMediaControl).Stop();
                DestroyGraph(graph);
            }
        }

        private void DestroyGraph(IGraphBuilder graph)
        {
            try
            {
                DirectShowHelper.clearGraph(graph, null);
                Marshal.ReleaseComObject(graph);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
            }
            catch (Exception)
            {
            }
            catch
            {
            }

            if (src != null)
            {
                Marshal.ReleaseComObject(src);
                src = null;
            }

            if (renderer != IntPtr.Zero)
            {
                DeleteNullRenderer(renderer);
                renderer = IntPtr.Zero;
            }

            graph = null;
        }

        public void OnFrame(IntPtr ptr)
        {
            ++index_;
            byte[] buff = new byte[width_ * height_ * 2];
            Marshal.Copy(ptr, buff, 0, buff.Length);
            Bitmap bmp = new Bitmap(width_, height_);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(new Rectangle(0, 0, width_, height_),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
            Marshal.Copy(buff, 0, data.Scan0, buff.Length);
            bmp.UnlockBits(data);
            //(graph as IMediaControl).Pause();
            FireEvent(bmp);

            if (index_ > 30)
                (graph as IMediaControl).Pause();
        }
    }
     */
    public class NullPlayer
    {
        [DllImport("NullRenderer.Native.dll")]
        private static extern IntPtr GetNullRenderer();

        [DllImport("NullRenderer.Native.dll")]
        private static extern void DeleteNullRenderer(IntPtr grabber);

        IGraphBuilder graph = null;
        IntPtr renderer = IntPtr.Zero;
        IBaseFilter src = null;
        INullGrabber grabber = null;
        int width_ = 0;
        int height_ = 0;
        int index_ = 0;

        private void FireEvent(IntPtr bmp)
        {
            if (GotFrame != null)
            {
                GotFrame(this, new FrameEventArgs(bmp));
            }
        }

        public NullPlayer()
        {

        }

        bool init(string path)
        {
            object obj = null;
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            graph = (IGraphBuilder)obj;
            obj = null;

            src = null;

            if (graph.AddSourceFilter(path, "SrcFilter", out src) < 0)
            {
                DestroyGraph(graph);
                return false;
            }

            renderer = GetNullRenderer();
            if (renderer == IntPtr.Zero)
            {
                DestroyGraph(graph);
                return false;
            }

            object tmp = Marshal.GetTypedObjectForIUnknown(renderer, typeof(IBaseFilter));
            IBaseFilter nullRenderer = (IBaseFilter)tmp;
            if (nullRenderer == null)
            {
                DestroyGraph(graph);
                return false;
            }

            if (graph.AddFilter(nullRenderer, "Null Renderer") < 0)
            {
                DestroyGraph(graph);
                return false;
            }

            if (!DirectShowHelper.ConnectPins(graph, src, nullRenderer))
            {
                DestroyGraph(graph);
                return false;
            }
            /*
                        long stopPos = 0;
                        long current = position.Ticks;

                        (graph as IMediaControl).Pause();
                        (graph as IMediaSeeking).SetPositions(ref current, (uint)AmSeeking.AbsolutePositioning, ref stopPos, (uint)AmSeeking.NoPositioning);
            */
            grabber = (INullGrabber)nullRenderer;

            grabber.getRect(out width_, out height_);

            return true;
        }

        public event EventHandler<FrameEventArgs> GotFrame;

        public bool start()
        {
            if (graph != null)
            {
                grabber.registerCallback(OnFrame);
                (graph as IMediaControl).Run();
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            release();
        }

        public void release()
        {
            if (graph != null)
            {
                (graph as IMediaControl).Stop();
                DestroyGraph(graph);
            }
        }

        private void DestroyGraph(IGraphBuilder graph)
        {
            try
            {
                DirectShowHelper.clearGraph(graph, null);
                Marshal.ReleaseComObject(graph);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
            }
            catch (Exception)
            {
            }
            catch
            {
            }

            if (src != null)
            {
                Marshal.ReleaseComObject(src);
                src = null;
            }

            if (renderer != IntPtr.Zero)
            {
                DeleteNullRenderer(renderer);
                renderer = IntPtr.Zero;
            }

            graph = null;
        }

        public void OnFrame(IntPtr ptr)
        {
            FireEvent(ptr);
        }
    }
}
