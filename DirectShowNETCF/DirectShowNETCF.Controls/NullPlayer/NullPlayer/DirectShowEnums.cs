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

    public enum AmSeeking : uint
    {
        NoPositioning = 0x00,
        AbsolutePositioning = 0x01,
        RelativePositioning = 0x02,
        IncrementalPositioning = 0x03,
        PositioningBitsMask = 0x03,
        SeekToKeyFrame = 0x04,
        ReturnTime = 0x08,
        Segment = 0x10,
        NoFlush = 0x20
    }

    public enum NotifyMessages : int
    {
        WM_GRAPHNOTIFY = 0x8000 + 1,
        EC_COMPLETE = 0x01
    }
}
