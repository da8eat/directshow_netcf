using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;
using Camera.AMCameraEx;

namespace AMCameraExControl
{


    public partial class AMCameraExControl : UserControl
    {
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

        private AMCameraEx _camera = null;
        private AMResultEx _result = AMResultEx.AddGrabberError;
        private int width_ = 0;
        private int height_ = 0;
        private Effets effect_ = Effets.None;
        private WndProcHooker.WndProcCallback wndProc = null;
        private bool started_ = false;

        public AMCameraExControl()
        {
            InitializeComponent();
            _camera = new AMCameraEx();
        }

        public bool init()
        {
            if (_camera == null)
            {
                _camera = new AMCameraEx();
            }

            _result = _camera.init(RotationType.Degree90);
          
            return _result == AMResultEx.OK;
        }

        public void shutdown()
        {
            _camera.release();
            _camera = null;
            started_ = false;
        }

        public bool Start()
        {
            if (!started_)
            {
                init();

                if (_result != AMResultEx.OK)
                {
                    return false;
                }

                started_ = _camera.run(Handle);
            }

            return started_;
        }

        public void Stop()
        {
            _camera.stop();
            shutdown();
        }

        public Bitmap GrabFrame()
        {
            if (_result != Camera.AMCameraEx.AMResultEx.OK)
            {
                return null;
            }

            if (width_ * height_ == 0)
            {
                _camera.getRect(out width_, out height_);
            }

            Bitmap bmp = new Bitmap(width_, height_);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width_, height_),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            bool grabOK = _camera.grabFrame(data.Scan0);
            bmp.UnlockBits(data);

            if (!grabOK)
            {
                bmp.Dispose();
                bmp = null;
            }

            return bmp;
            
        }

        public void Overlay(Bitmap bmp, int id, int x, int y)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            _camera.drawBitmap(data.Scan0, id, x, y, bmp.Width, bmp.Height);
            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Blend Preview and passed Bitmap
        /// </summary>
        /// <param name="bmp">Bitmap</param>
        /// <param name="id">will be used to delete bitmap when you need</param>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <param name="blend">blend value 0 - 255</param>
        public void BlendBitmap(Bitmap bmp, int id, int x, int y, byte blend)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            _camera.blendBitmap(data.Scan0, id, x, y, bmp.Width, bmp.Height, blend);

            bmp.UnlockBits(data);
        }

        /// <summary>
        /// Some device (especially Toshiba and LG got incorrect buffer ordering for Chroma
        /// thats why we need this function
        /// </summary>
        /// <param name="needFix">set true if you got broken preview</param>
        public void FixPreview(bool needFix)
        {
            _camera.fixPreview(needFix);
        }

        public void OverlayTransparent(Bitmap bmp, int id, int x, int y, Color transparentColor)
        {
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            _camera.drawTransparentBitmap(data.Scan0, id, x, y, bmp.Width, bmp.Height,
                transparentColor.R, transparentColor.G, transparentColor.B);
            
            bmp.UnlockBits(data);
        }

        public void eraseBitmap(int id)
        {
            _camera.eraseBitmap(id);
        }

        /// <summary>
        /// Turns on flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool FlashOn()
        {
            return _camera.flashOn();
        }

        /// <summary>
        /// Turns off flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool FlashOff()
        {
            return _camera.flashOff();
        }

        /// <summary>
        /// turns On autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool AutoFocusOn()
        {
            return _camera.autoFocusOn();
        }

        /// <summary>
        /// turns Off autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool AutoFocusOff()
        {
            return _camera.autoFocusOff();
        }

        /// <summary>
        /// Focus +, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool FocusPlus()
        {
            return _camera.focusPlus();
        }

        /// <summary>
        /// Focus -, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool FocusMinus()
        {
            return _camera.focusMinus();
        }

        /// <summary>
        /// Zooms +
        /// </summary>
        /// <returns>true if successed</returns>
        public bool ZoomIn()
        {
            return _camera.zoomIn();
        }

        /// <summary>
        /// Zooms -
        /// </summary>
        /// <returns>true if successed</returns>
        public bool ZoomOut()
        {
            return _camera.zoomOut();
        }

        public Effets Effect
        {
            get
            {
                return effect_;
            }
            set
            {
                effect_ = value;
                _camera.applyEffect(effect_);
            }
        }

        public List<string> getMediaTypes()
        {
            return _camera.getMediaTypes();
        }

        public void setMediaType(Int32 index)
        {
            _camera.setMediaType(index);
            if (started_)
            {
                started_ = false;
                Start();
            }
        }

        public bool Started
        {
            get
            {
                return started_;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                WndProcHooker.UnhookWndProc(this, ALL_MESSAGES);
            }
            catch (Exception)
            {
            }

            wndProc = null;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            if (wndProc == null)
            {
                wndProc = new WndProcHooker.WndProcCallback(WndProc);
                WndProcHooker.HookWndProc(this, wndProc, ALL_MESSAGES);
            }
            //if (_result == AMResultEx.OK)
            //{
            //    _camera.resize(this.Handle, this.Width, this.Height);
            //}
           // base.OnResize(e);
        }

        private static int WndProc(IntPtr hwnd, uint msg, uint wParam, int lParam, ref bool handled)
        {
            AMCameraExControl this_ = (AMCameraExControl)WndProcHooker.watchControl;
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
            }
            handled = false;
            return 0;
        }
    }
}
