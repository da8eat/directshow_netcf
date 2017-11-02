using System;
using System.Runtime.InteropServices;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Structs;

namespace DirectShowNETCF.Player
{
    public class Player
    {
        private IGraphBuilder graph = null;
        private IMediaControl control = null;
        private IVideoWindow window = null;

        public Player()
        {
            object obj = null;
            Guid clsid = CLSID_.FilterGraph;
            Guid riid = IID_.IFilterGraph2;
            PInvokes.CoCreateInstance(ref clsid, IntPtr.Zero, (uint)CLSCTX_.INPROC_SERVER, ref riid, out obj);
            graph = (IGraphBuilder)obj;
            control = (IMediaControl)graph;
            obj = null;
        }

        ~Player()
        {
            if (graph != null)
            {
                stop();

                if (window != null)
                {
                    window.put_Visible(0);
                    window.put_Owner(IntPtr.Zero);
                }

                DirectShowHelper.clearGraph(graph, null);

                Marshal.ReleaseComObject(graph);
                graph = null;
                control = null;
                window = null;
            }
        }

        public bool renderFile(string fileName)
        {
            if (graph == null)
            {
                return false;
            }

            stop();
            DirectShowHelper.clearGraph(graph, null);

            if (graph.RenderFile(fileName, null) < 0)
            {
                return false;
            }

            window = (IVideoWindow)graph;
            return true;
        }

        public bool play()
        {
            return ((control != null) && (control.Run() >= 0));
        }

        public bool stop()
        {
            return ((control != null) && (control.Stop() >= 0));
        }

        public bool pause()
        {
            return ((control != null) && (control.Pause() >= 0));
        }

        public bool setVideoWindow(IntPtr owner)
        {
            if (window != null)
            {
                Rect rc = new Rect();
                PInvokes.GetClientRect(owner, out rc);
                window.put_Owner(owner);
                window.SetWindowPosition(rc.Left, rc.Top, rc.Right, rc.Bottom);
                window.put_WindowStyle(0x40000000 | 0x02000000);
                window.put_Visible(-1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
