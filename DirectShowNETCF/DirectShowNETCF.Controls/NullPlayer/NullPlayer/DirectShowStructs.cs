using System;
using System.Runtime.InteropServices;
using DirectShowNETCF.Enums;

namespace DirectShowNETCF.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Size
    {
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public class VideoInfoHeader
    {
        public Rect SrcRect;
        public Rect TagRect;
        public int BitRate;
        public int BitErrorRate;
        public long AvgTimePerFrame;
        public PBITMAPINFOHEADER BmiHeader;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public class VideoInfoHeader2
    {
        public Rect SrcRect;
        public Rect TargetRect;
        public int BitRate;
        public int BitErrorRate;
        public long AvgTimePerFrame;
        public int InterlaceFlags;
        public int CopyProtectFlags;
        public int PictAspectRatioX;
        public int PictAspectRatioY;
        public int ControlFlags;
        public int Reserved2;
        public PBITMAPINFOHEADER BmiHeader;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct PBITMAPINFOHEADER
    {
        public int Size;
        public int Width;
        public int Height;
        public short Planes;
        public short BitCount;
        public int Compression;
        public int ImageSize;
        public int XPelsPerMeter;
        public int YPelsPerMeter;
        public int ClrUsed;
        public int ClrImportant;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DEVMGR_DEVICE_INFORMATION
    {
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr hParentDevice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string szLegacyName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDeviceKey;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szBusName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class VideoStreamConfigCaps
    {
        public Guid guid;
        public int VideoStandard;
        public Size InputSize;
        public Size MinCroppingSize;
        public Size MaxCroppingSize;
        public int CropGranularityX;
        public int CropGranularityY;
        public int CropAlignX;
        public int CropAlignY;
        public Size MinOutputSize;
        public Size MaxOutputSize;
        public int OutputGranularityX;
        public int OutputGranularityY;
        public int StretchTapsX;
        public int StretchTapsY;
        public int ShrinkTapsX;
        public int ShrinkTapsY;
        public long MinFrameInterval;
        public long MaxFrameInterval;
        public int MinBitsPerSecond;
        public int MaxBitsPerSecond;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct PinInfo
    {
        public IntPtr filter;
        public PinDirection dir;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class AMMediaType
    {
        public Guid majorType;
        public Guid subType;
        public int fixedSizeSamples;
        public int temporalCompression;
        public int sampleSize;
        public Guid formatType;
        public IntPtr unkPtr;
        public int formatSize;
        public IntPtr formatPtr;
    }
}
