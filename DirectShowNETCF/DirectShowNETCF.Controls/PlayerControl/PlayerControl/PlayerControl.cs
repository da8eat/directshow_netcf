using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DirectShow;
using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Imports;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;

namespace PlayerControl
{
   /* public class NullPlayer : IDisposable
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
    public partial class PlayerControl : UserControl
    {
        #region declarations

        private WndProcHooker.WndProcCallback wndProc = null;
        private PlayerModel _playerModel = null;
        private Timer _timer = null;
        private bool _loop = false;

        #endregion

        #region constants

        private const int ALL_MESSAGES = 0x00FFFFFF;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MBUTTONDBLCLK = 0x0209;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_RBUTTONDBLCLK = 0x0206;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;

        #endregion

        #region Events

        public event EventHandler MediaFailed;
        public event EventHandler MediaEnded;
        public event EventHandler<ProgressEventArgs> MediaProgress;

        #endregion

        #region Constructor

        public PlayerControl()
        {
            InitializeComponent();

            if (wndProc == null)
            {
                wndProc = new WndProcHooker.WndProcCallback(WndProc);
                WndProcHooker.HookWndProc(this, wndProc, ALL_MESSAGES);
            }

            _playerModel = new PlayerModel();
            _playerModel.MediaFailed += OnMediaFailed;

            InitTimer();
        }

        #endregion

        #region Properties

        public bool Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
            }
        }

        public int Volume
        {
            get
            {
                return _playerModel.GetVolume();
            }
            set
            {
                _playerModel.SetVolume(value);
            }
        }

        public int Balance
        {
            get
            {
                return _playerModel.GetVolume();
            }

            set
            {
                _playerModel.SetBalance(value);
            }
        }

        public int VideoWidth
        {
            get
            {
                return _playerModel.GetWidth();
            }
        }

        public int VideoHeight
        {
            get
            {
                return _playerModel.GetHeight();
            }
        }

        public int BitRate
        {
            get
            {
                return _playerModel.GetBitRate();
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Builds graph that allows to play required file
        /// </summary>
        /// <param name="filePath">Path to file to play</param>
        public void OpenFile(string filePath)
        {
            _playerModel.LoadFile(filePath);
            _playerModel.SetVideoWindow(this.Handle, 0, 0, this.Width, this.Height);
            _playerModel.SetEventHendler(this.Handle);
        }

        /// <summary>
        /// Starts play file
        /// </summary>
        public void Play()
        {
         //   MessageBox.Show("You are using demo version of Player control!");
            _playerModel.Play();
            _timer.Enabled = true;
        }

        /// <summary>
        /// Pauses player
        /// </summary>
        public void Pause()
        {
            _playerModel.Pause();
            _timer.Enabled = false;
        }

        /// <summary>
        /// Sets player to required position
        /// </summary>
        /// <param name="position"> Position of file</param>
        public void Seek(TimeSpan position)
        {
            _playerModel.Seek(position);
        }

        /// <summary>
        /// Stops playing, unloads all resources, and destroys graph
        /// </summary>
        public void Stop()
        {
            _timer.Enabled = false;
            _playerModel.Stop();
        }

        /// <summary>
        /// Gets Duration of media file
        /// </summary>
        /// <returns> Media duration, or TimeSpan.Zero if cannot get duration</returns>
        public TimeSpan GetDuration()
        {
            return _playerModel.GetDuration();
        }

        #endregion

        #region overrides

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            DestroyTimer();
            _playerModel.MediaFailed -= OnMediaFailed;

            _playerModel.Dispose();
            _playerModel = null;

            WndProcHooker.UnhookWndProc(this, false);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            if (_playerModel != null)
            {
                _playerModel.Resize(this.Width, this.Height);
            }

            base.OnResize(e);
        }

        #endregion

        #region private methods

        private static int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam, ref bool handled)
        {
            PlayerControl this_ = (PlayerControl)WndProcHooker.watchControl;
            handled = true;

            int x = lParam & 0x0000FFFF;
            int y = (lParam >> 16) & 0x0000FFFF;

            switch (msg)
            {
                case WM_KEYDOWN:
                case WM_KEYUP:
                    return 0;
                case WM_LBUTTONDBLCLK:
                    {
                        this_.OnDoubleClick(EventArgs.Empty);
                        return 0;
                    }
                case WM_LBUTTONDOWN:
                    {
                        this_.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
                        return 0;
                    }
                case WM_LBUTTONUP:
                    {
                        this_.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
                        return 0;
                    }
                case WM_MBUTTONDBLCLK:
                    {
                        this_.OnDoubleClick(EventArgs.Empty);
                        return 0;
                    }
                case WM_MBUTTONDOWN:
                    {
                        this_.OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 1, x, y, 0));
                        return 0;
                    }
                case WM_MBUTTONUP:
                    {
                        this_.OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 1, x, y, 0));
                        return 0;
                    }
                case WM_MOUSEMOVE:
                    {
                        return 0;
                    }
                case WM_RBUTTONDBLCLK:
                    {
                        this_.OnDoubleClick(EventArgs.Empty);
                        return 0;
                    }
                case WM_RBUTTONDOWN:
                    {
                        this_.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, x, y, 0));
                        return 0;
                    }
                case WM_RBUTTONUP:
                    {
                        this_.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, x, y, 0));
                        return 0;
                    }
                case (int)NotifyMessages.WM_GRAPHNOTIFY:
                    {
                        this_.HandleGraphEvent();
                        return 0;
                    }
            }
            handled = false;
            return 0;
        }

        private void HandleGraphEvent()
        {
            int evCode;
            int evParam1, evParam2;

            while (_playerModel.GetEvent(out evCode, out evParam1, out evParam2) == 0)
            {
                _playerModel.FreeEventParams(evCode, evParam1, evParam2);

                switch (evCode)
                {
                    case (int)NotifyMessages.EC_COMPLETE:
                        {
                            OnMediaEnded();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void InitTimer()
        {
            _timer = new Timer();
            _timer.Interval = 200;
            _timer.Tick += OnTimer;
        }

        private void DestroyTimer()
        {
            _timer.Enabled = false;
            _timer.Tick -= OnTimer;
            _timer.Dispose();
            _timer = null;
        }

        #endregion

        #region Event Handlers

        private void OnMediaEnded()
        {
            if (!Loop)
            {
                _timer.Enabled = false;

                if (MediaEnded != null)
                {
                    MediaEnded(this, EventArgs.Empty);
                }
            }
            else
            {
                Seek(TimeSpan.Zero);
            }
        }

        private void OnMediaFailed(object sender, EventArgs e)
        {
            _timer.Enabled = false;

            if (MediaFailed != null)
            {
                MediaFailed(this, e);
            }
        }

        private void OnMediaProgress(TimeSpan progress)
        {
            if (MediaProgress != null)
            {
                MediaProgress(this, new ProgressEventArgs(progress));
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            OnMediaProgress(_playerModel.GetCurrentPosition());
        }

        #endregion
    }
}
