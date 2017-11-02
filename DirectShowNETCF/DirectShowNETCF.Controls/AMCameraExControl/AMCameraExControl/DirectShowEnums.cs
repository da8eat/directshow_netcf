using System;
using System.Runtime.InteropServices;
using DirectShowNETCF;

namespace DirectShowNETCF.Enums
{
    public enum CLSCTX_ : uint
    {
        INPROC_SERVER = 1
    }

    public enum CameraControlFlags : int
    {
        Auto = 0x0001,
        Manual = 0x0002
    }

    public enum CameraControlProperty : int
    {
        Pan,
        Tilt,
        Roll,
        Zoom,
        Exposure,
        Iris,
        Focus,
        Flash
    }

    public enum RawFrameFormat : int
    {
        YVU9,
        Y411,
        Y41P,
        YUY2,
        YVYU,
        UYVY,
        Y211,
        YV12,
        MJPG,
        RGB565,
        RGB24,
        RGB32,
        Unknown
    }

    [ComVisible(false)]
    public enum PinDirection
    {
        Input,
        Output
    }
}
