using System;
using System.Runtime.InteropServices;

using DirectShowNETCF.Structs;

namespace DirectShowNETCF.PInvoke
{
    public static class PInvokes
    {
        [DllImport("ole32.dll")]
        public static extern int CoCreateInstance(
            [In] ref Guid rclsid,
            [In] IntPtr pUnkOuter,
            [In] uint dwClsContext,
            [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object pv);

        [DllImport("coredll.dll")]
        public static extern int GetClientRect(
            [In] IntPtr hWnd,
            [Out] out Rect lpRect);

        [DllImport("coredll.dll")]
        public static extern IntPtr FindFirstDevice(
            [In] int searchType,
            [In] IntPtr searchParam,
            [In, Out] ref DEVMGR_DEVICE_INFORMATION pdi);

        [DllImport("coredll.dll", EntryPoint = "FindFirstDevice")]
        public static extern IntPtr FindFirstDevice2(
            [In] int searchType,
            [In, MarshalAs(UnmanagedType.LPWStr)] string searchParam,
            [In, Out] ref DEVMGR_DEVICE_INFORMATION pdi);

        [DllImport("coredll.dll")]
        public static extern int FindClose([In] IntPtr hFindFile);

        [DllImport("coredll.dll")]
        public static extern void LocalFree(IntPtr ptr);

        [DllImport("coredll.dll")]
        public static extern IntPtr LocalAlloc(
            int flags,
            int size);

        [DllImport("coredll", SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        public static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            byte[] lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("coredll")]
        public static extern int CloseHandle(IntPtr Hwnd);
    }
}
