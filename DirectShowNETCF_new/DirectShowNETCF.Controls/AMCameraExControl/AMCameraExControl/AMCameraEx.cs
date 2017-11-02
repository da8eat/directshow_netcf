using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Camera.AMCameraEx
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
        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 CreateAMCamera();

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void ReleaseAMCamera(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 InitEx(Int32 holder, Int32 rotationType);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void ReleaseEx(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void StopEx(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 RunEx(Int32 holder, IntPtr owner);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 ResizeEx(Int32 holder, IntPtr owner, Int32 width, Int32 height);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 FlashOn(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 FlashOff(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 AutoFocusOn(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 AutoFocusOff(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 FocusPlus(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 FocusMinus(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 ZoomIn(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 ZoomOut(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void GetRectEx(Int32 holder, out Int32 width, out Int32 height);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void ApplyEffect(Int32 holder, Int32 effect);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern IntPtr GrabFrame(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 GrabFrame2(Int32 holder, IntPtr buffer);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void DrawBitmap(Int32 holder, IntPtr ptr, Int32 id, Int32 x, Int32 y,
                                              Int32 width, Int32 height, Int32 transparent,
                                              Int32 r, Int32 g, Int32 b);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void FixPreviewEx(Int32 holder, Int32 fix);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void EraseBitmap(Int32 holder, Int32 id);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 GetTypesCount(Int32 holder);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern Int32 GetTypeEx(Int32 holder, Int32 index);

        [DllImport("AMCameraEx.Native.dll")]
        private static extern void SetTypeEx(Int32 holder, Int32 index);

        Int32 holder_;

        public AMCameraEx()
        {
            holder_ = CreateAMCamera();
        }

        ~AMCameraEx()
        {
            ReleaseAMCamera(holder_);
        }

        public AMResultEx init(RotationType rotationType)
        {
            return (AMResultEx)InitEx(holder_, (Int32)rotationType);
        }

        public void release()
        {
            ReleaseEx(holder_);
        }

        public void stop()
        {
            StopEx(holder_);
        }

        public bool run(IntPtr owner)
        {
            return RunEx(holder_, owner) == 1;
        }

        public bool resize(IntPtr owner, int width, int height)
        {
            return ResizeEx(holder_, owner, width, height) == 1;
        }

        /// <summary>
        /// Turns on flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOn()
        {
            return FlashOn(holder_) == 1;
        }

        /// <summary>
        /// Turns off flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOff()
        {
            return FlashOff(holder_) == 1;
        }

        /// <summary>
        /// turns On autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOn()
        {
            return AutoFocusOn(holder_) == 1;
        }

        /// <summary>
        /// turns Off autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOff()
        {
            return AutoFocusOff(holder_) == 1;
        }

        /// <summary>
        /// Focus +, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusPlus()
        {
            return FocusPlus(holder_) == 1;
        }

        /// <summary>
        /// Focus -, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusMinus()
        {
            return FocusMinus(holder_) == 1;
        }

        /// <summary>
        /// Zooms +
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomIn()
        {
            return ZoomIn(holder_) == 1;
        }

        /// <summary>
        /// Zooms -
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomOut()
        {
            return ZoomOut(holder_) == 1;
        }

        public void getRect(out Int32 width, out Int32 height)
        {
            GetRectEx(holder_, out width, out height);
        }

        public void applyEffect(Effets effect)
        {
            ApplyEffect(holder_, (Int32)effect);
        }

        public IntPtr grabFrame()
        {
            return GrabFrame(holder_);
        }

        public bool grabFrame(IntPtr buffer)
        {
            return GrabFrame2(holder_, buffer) == 1;
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
            DrawBitmap(holder_, ptr, id, x, y, width, height, 0, 0, 0, 0);
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
        public void drawTransparentBitmap(IntPtr ptr, Int32 id, Int32 x, Int32 y, Int32 width, Int32 height, Int32 r, Int32 g, Int32 b)
        {
            DrawBitmap(holder_, ptr, id, x, y, width, height, 1, r, g, b);
        }

        public void blendBitmap(IntPtr ptr, Int32 id, Int32 x, Int32 y, Int32 width, Int32 height, Int32 blend)
        {
            DrawBitmap(holder_, ptr, id, x, y, width, height, 0, blend, 0, 0);
        }

        public void fixPreview(bool fix)
        {
            if (fix)
                FixPreviewEx(holder_, 1);
            else
                FixPreviewEx(holder_, 0);
        }

        /// <summary>
        /// Erases earlier drawn bitmap
        /// </summary>
        /// <param name="id">bitmap id</param>
        public void eraseBitmap(Int32 id)
        {
            EraseBitmap(holder_, id);
        }

        public List<string> getMediaTypes()
        {
            List<string> res_ = new List<string>();

            Int32 count = GetTypesCount(holder_);
            for (Int32 i = 0; i < count; ++i)
            {
                Int32 value = GetTypeEx(holder_, i);
                res_.Add((value >> 16).ToString() + "X" + (value & 0xffff).ToString());
            }
            return res_;
        }

        public void setMediaType(Int32 index)
        {
            SetTypeEx(holder_, index);
        }
    }
}
