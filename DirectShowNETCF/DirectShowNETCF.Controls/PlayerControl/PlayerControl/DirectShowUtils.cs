using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Helper;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;
using DirectShowNETCF.PInvoke;

namespace DirectShowNETCF.Utils
{
    public class CPropertyBag : IPropertyBag
    {
        private object pVar_ = null;

        public CPropertyBag()
        {
            pVar_ = new object();
        }

        ~CPropertyBag()
        {
            pVar_ = null;
        }

        public int Read(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
            [In, Out, MarshalAs(UnmanagedType.Struct)]	ref	object pVar,
            [In] IntPtr pErrorLog)
        {
            pVar = pVar_;
            return 0;
        }

        public int Write(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
            [In, MarshalAs(UnmanagedType.Struct)] ref object pVar)
        {
            pVar_ = pVar;
            return 0;
        }
    }

    public class CFilterList
    {
        private List<IBaseFilter> Filters;
        private IEnumFilters enumFilters = null;
        public CFilterList()
        {
            Filters = new List<IBaseFilter>();
        }
        ~CFilterList()
        {
            Free();
            Filters = null;
        }
        public void Assign(IGraphBuilder fg)
        {
            Clear();
            fg.EnumFilters(out enumFilters);
            IBaseFilter Filter;
            int fetched = 0;

            while (enumFilters.Next(1, out Filter, out fetched) == 0)
            {
                Filters.Add(Filter);
            }

            Marshal.ReleaseComObject(enumFilters);
            enumFilters = null;
        }
        public int Count
        {
            get 
            { 
                return Filters.Count; 
            }
        }
        public IBaseFilter this[int index]
        {
            get
            {
                return Filters[index];
            }
        }
        public void Free()
        {
            Clear();
        }
        private void Clear()
        {
            while (Filters.Count > 0)
            {
                Marshal.ReleaseComObject(Filters[0]);
                Filters[0] = null;
                Filters.RemoveAt(0);
            }

            Filters.Clear();
        }
    }

    public class CPinList
    {
        private List<IPin> Pins = null;
        private IEnumPins enumPins = null;

        public CPinList()
        {
            Pins = new List<IPin>();
        }

        ~CPinList()
        {
            Free();
            Pins = null;
        }

        public int Count
        {
            get 
            { 
                return Pins.Count; 
            }
        }

        public IPin this[int index]
        {
            get
            {
                return Pins[index];
            }
        }

        public void Assign(IBaseFilter filter)
        {
            Clear();
            filter.EnumPins(out enumPins);

            int hr = 0;
            do
            {
                IPin pin;
                int fetched = 0;

                hr = enumPins.Next(1, out pin, out fetched);
                if ((hr < 0) || (fetched != 1))
                    break;

                Pins.Add(pin);
            }
            while (true);

            Marshal.ReleaseComObject(enumPins);
            enumPins = null;

        }

        private void Clear()
        {
            while (Pins.Count > 0)
            {
                Marshal.ReleaseComObject(Pins[0]);
                Pins[0] = null;
                Pins.RemoveAt(0);
            }
            Pins.Clear();
        }

        public void Free()
        {
            Clear();
        }
    }

    public class CEnumMediaTypes
    {
        private List<AMMediaType> FList;
        private IntPtr Mt;
        public void Free()
        {
            Clear();
        }

        public CEnumMediaTypes()
        {
            FList = new List<AMMediaType>();
        }

        private void Clear()
        {
            for (int i = 0; i < FList.Count; i++)
            {
                DirectShowHelper.FreeMediaType(FList[i]);
                FList[0] = null;
                FList.RemoveAt(0);
            }

            FList.Clear();
        }

        public int Count
        {
            get
            {
                return FList.Count;
            }
        }


        public void Assign(ICaptureGraphBuilder2 device, IBaseFilter filter, Guid category)
        {
            Clear();

            IPin pin_ = null;
            CGuid guid = new CGuid(category);
            CGuid guid2 = new CGuid(CLSID_.MEDIATYPE_Video);
            int res = device.FindPin(filter, PinDirection.Output, guid, guid2, true, 0, out pin_);

            if (res > -1)
            {
                /*IAMStreamConfig strm = (IAMStreamConfig)pin_;
                if (strm != null)
                {
                    int cnt = 0;
                    int size = 0;
                    strm.GetNumberOfCapabilities(out cnt, out size);
                    IntPtr ptr = PInvokes.LocalAlloc(0x40, size);
                    //cnt = 1;
                    for (int i = 0; i < cnt; i++)
                    {
                        strm.GetStreamCaps(i, out Mt, ptr);
                        AMMediaType mt = (AMMediaType)Marshal.PtrToStructure(Mt, typeof(AMMediaType));
                        FList.Add(mt);
                    }

                    PInvokes.LocalFree(ptr);
                }*/

                IEnumMediaTypes enumMediaTypes;
                pin_.EnumMediaTypes(out enumMediaTypes);
                if (enumMediaTypes == null)
                {
                    return;
                }

                int fetced = 0;

                int ttt = enumMediaTypes.Next(1, out Mt, out fetced);
                while (ttt == 0)
                {
                    fetced = 0;
                    ttt = enumMediaTypes.Next(1, out Mt, out fetced);
                    AMMediaType mt = (AMMediaType)Marshal.PtrToStructure(Mt, typeof(AMMediaType));
                    FList.Add(mt);
                }

                Marshal.ReleaseComObject(enumMediaTypes);
                //strm = null;
                Marshal.ReleaseComObject(pin_);
            }
        }

        private string getFourCC(int value)
        {
            IntPtr ptr = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(ptr, value);
            byte[] bt = new byte[4];
            Marshal.Copy(ptr, bt, 0, 4);
            Marshal.FreeHGlobal(ptr);
            char[] ch = new char[4];
            for (int i = 0; i < 4; i++)
            {
                ch[i] = (char)bt[i];
            }
            bt = null;
            return new string(ch);
        }

        public string GetMediaDescription(int index)
        {
            string result = "";

            AMMediaType tempType = FList[index];
            if (tempType.formatType == CLSID_.VideoInfo)
            {
                result += ((VideoInfoHeader)Marshal.PtrToStructure(tempType.formatPtr, typeof(VideoInfoHeader))).BmiHeader.Width.ToString();
                result += 'X';
                result += ((VideoInfoHeader)Marshal.PtrToStructure(tempType.formatPtr, typeof(VideoInfoHeader))).BmiHeader.Height.ToString();
            }
            else
            {
                if (tempType.formatType == CLSID_.VideoInfo2)
                {
                    result += ((VideoInfoHeader2)Marshal.PtrToStructure(tempType.formatPtr, typeof(VideoInfoHeader2))).BmiHeader.Width.ToString();
                    result += 'X';
                    result += ((VideoInfoHeader2)Marshal.PtrToStructure(tempType.formatPtr, typeof(VideoInfoHeader2))).BmiHeader.Height.ToString();
                }
            }

            tempType = null;
            return result;
        }

        public AMMediaType this[int index]
        {
            get
            {
                return FList[index];
            }
        }

    }

    public class CAMCameraControlAdapter : IDisposable
    {
        private IAMCameraControl control_;
        private IntPtr device_ = (IntPtr)(-1);

        public CAMCameraControlAdapter(IAMCameraControl control)
        {
            control_ = control;
            device_ = PInvokes.CreateFile("CIF1:", 0x80000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (!IsValidHandle())
            {
                device_ = PInvokes.CreateFile("CAM1:", 0x80000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            }
        }

        public void Dispose()
        {
            if (IsValidHandle())
            {
                PInvokes.CloseHandle(device_);
                device_ = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Turns on flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOn()
        {
            if (IsValidHandle() && SetFlash(1))
            {
                return true;
            }

            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags;
                int hr = control_.GetRange((int)CameraControlProperty.Flash,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Flash, max, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Turns off flash if it supported by device directshow driver
        /// </summary>
        /// <returns>true if successed otherwise false</returns>
        public bool flashOff()
        {
            if (IsValidHandle() && SetFlash(2))
            {
                return true;
            }

            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags;
                int hr = control_.GetRange((int)CameraControlProperty.Flash,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Flash, min, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// turns On autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOn()
        {
            if (IsValidHandle() && SetFocus(1))
            {
                return true;
            }

            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags;
                int hr = control_.GetRange((int)CameraControlProperty.Focus,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return sphm840AutoFocusOn();
                }

                hr = control_.Set((int)CameraControlProperty.Focus, def, (int)CameraControlFlags.Auto);

                if (hr < 0)
                {
                    return sphm840AutoFocusOn();
                }
                return true;
            }
        }

        /// <summary>
        /// turns Off autofocus
        /// </summary>
        /// <returns>true if successed</returns>
        public bool autoFocusOff()
        {
            if (IsValidHandle() && SetFocus(2))
            {
                return true;
            }

            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags;
                int hr = control_.GetRange((int)CameraControlProperty.Focus,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return sphm840AutoFocusOff();
                }

                hr = control_.Set((int)CameraControlProperty.Focus, def, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return sphm840AutoFocusOff();
                }
                return true;
            }
        }

        /// <summary>
        /// Focus +, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusPlus()
        {
            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags, current;
                int hr = control_.GetRange((int)CameraControlProperty.Focus,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Get((int)CameraControlProperty.Focus, out current, out flags);

                if ((hr < 0) || (current == max))
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Focus, current + delta, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Focus -, turns off autofocus if it on
        /// </summary>
        /// <returns>true if successed</returns>
        public bool focusMinus()
        {
            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags, current;
                int hr = control_.GetRange((int)CameraControlProperty.Focus,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Get((int)CameraControlProperty.Focus, out current, out flags);

                if ((hr < 0) || (current == min))
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Focus, current - delta, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Zooms +
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomIn()
        {
            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags, current;
                int hr = control_.GetRange((int)CameraControlProperty.Zoom,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Get((int)CameraControlProperty.Zoom, out current, out flags);

                if ((hr < 0) || (current == max))
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Zoom, current + delta, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Zooms -
        /// </summary>
        /// <returns>true if successed</returns>
        public bool zoomOut()
        {
            if (control_ == null)
            {
                return false;
            }
            else
            {
                int min, max, delta, def, flags, current;
                int hr = control_.GetRange((int)CameraControlProperty.Zoom,
                    out min, out max, out delta, out def, out flags);

                if (hr < 0)
                {
                    return false;
                }

                hr = control_.Get((int)CameraControlProperty.Zoom, out current, out flags);

                if ((hr < 0) || (current == max))
                {
                    return false;
                }

                hr = control_.Set((int)CameraControlProperty.Zoom, current - delta, (int)CameraControlFlags.Manual);

                if (hr < 0)
                {
                    return false;
                }
                return true;
            }
        }

        private bool sphm840AutoFocusOff()
        {
            int hr = control_.Set((int)CameraControlProperty.Flash + 1, 0, (int)CameraControlFlags.Manual);
            if (hr < 0)
            {
                hr = control_.Set((int)CameraControlProperty.Zoom, 704, (int)CameraControlFlags.Manual);
            }

            return hr > -1;
        }

        private bool sphm840AutoFocusOn()
        {
            int hr = control_.Set((int)CameraControlProperty.Flash + 1, 3, (int)CameraControlFlags.Manual);

            if (hr < 0)
            {
                hr = control_.Set((int)CameraControlProperty.Zoom, 705, (int)CameraControlFlags.Auto);
            }

            return hr > -1;
        }

        private bool IsValidHandle()
        {
            return device_ != IntPtr.Zero && device_ != (IntPtr)(-1);
        }

        private bool SetFocus(int val)
        {
            int[] propdat = new int[2];
            IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(propdat));

            propdat[0] = 0x0f;
            propdat[1] = val;
            Marshal.Copy(propdat, 0, p2, propdat.Length);
            uint r = 0;
            int res = PInvokes.DeviceIoControl(device_, 0x90002018, p2, 8, null, 0, ref r, IntPtr.Zero);

            Marshal.FreeHGlobal(p2);

            return res != 0;
        }

        private bool SetFlash(int val)
        {
            IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(val));
            Marshal.WriteInt32(p2, val);
            uint r = 0;
            int res = PInvokes.DeviceIoControl(device_, 0x90002024, p2, 4, null, 0, ref r, IntPtr.Zero);

            Marshal.FreeHGlobal(p2);

            return res != 0;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public class CGuid
    {
        [FieldOffset(0)]
        private Guid guid_;

        public CGuid()
        {
            guid_ = Guid.Empty;
        }

        public CGuid(string guid)
        {
            guid_ = new Guid(guid);
        }

        public CGuid(Guid guid)
        {
            guid_ = guid;
        }
    }
}
