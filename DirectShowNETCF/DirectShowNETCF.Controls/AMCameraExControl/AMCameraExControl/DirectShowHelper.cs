using System;
using System.Runtime.InteropServices;

using DirectShowNETCF.Imports;
using DirectShowNETCF.Utils;
using DirectShowNETCF.Structs;
using DirectShowNETCF.Guids;
using DirectShowNETCF.Enums;

namespace DirectShowNETCF.Helper
{
    public static class DirectShowHelper
    {
        public static void clearGraph(IGraphBuilder graph, IBaseFilter filter)
        {
            CFilterList filters_ = new CFilterList();
            filters_.Assign(graph);
            for (int i = 0; i < filters_.Count; i++)
            {
                CPinList pins_ = new CPinList();
                pins_.Assign(filters_[i]);
                for (int j = 0; j < pins_.Count; j++)
                {
                    //pins_[j].Disconnect();
                }
                pins_.Free();
                pins_ = null;
                if (filters_[i] != filter)
                {
                    graph.RemoveFilter(filters_[i]);
                }
            }
            filters_.Free();
            filters_ = null;
        }

        public static void FreeMediaType(AMMediaType mediaType)
        {
            if (mediaType != null)
            {
                if (mediaType.formatSize != 0)
                {
                    Marshal.FreeCoTaskMem(mediaType.formatPtr);
                    mediaType.formatSize = 0;
                    mediaType.formatPtr = IntPtr.Zero;
                }

                if (mediaType.unkPtr != IntPtr.Zero)
                {
                    Marshal.Release(mediaType.unkPtr);
                    mediaType.unkPtr = IntPtr.Zero;
                }
            }
        }

        public static void FreePinInfo(PinInfo info)
        {
            if (info.filter != IntPtr.Zero)
            {
                Marshal.Release(info.filter);
                info.filter = IntPtr.Zero;
            }
        }

        public static int ConnectPins(IGraphBuilder graph, ICaptureGraphBuilder2 capGraph, IBaseFilter output, IBaseFilter input, Guid category)
        {
            CPinList inputs = new CPinList();
            inputs.Assign(input);
            IPin pinOut = null;
            IPin pinIn = null;

            CGuid guid = new CGuid(category);
            CGuid guid2 = new CGuid(CLSID_.MEDIATYPE_Video);

            int hr = capGraph.FindPin(output, PinDirection.Output, guid, guid2, true, 0, out pinOut);

            if (hr < 0)
            {
                inputs.Free();
                inputs = null;
                return -1;
            }

            for (int i = 0; i < inputs.Count; i++)
            {
                PinInfo info;
                inputs[i].QueryPinInfo(out info);
                if (info.dir == PinDirection.Input)
                {
                    pinIn = inputs[i];
                    FreePinInfo(info);
                    break;
                }
                FreePinInfo(info);
            }

            if (pinIn == null)
            {
                inputs.Free();
                Marshal.ReleaseComObject(pinOut);
                inputs = null;
                pinOut = null;
                return -1;
            }

            int res_ = graph.Connect(pinOut, pinIn);

            inputs.Free();
            Marshal.ReleaseComObject(pinOut);
            inputs = null;
            pinOut = null;
            pinIn = null;

            return res_;
        }

        public static int SetMediaType(ICaptureGraphBuilder2 capGraph, IBaseFilter filter, AMMediaType mediaType, Guid categoty)
        {
            IPin reqPin = null;

            CGuid guid = new CGuid(categoty);
            CGuid guid2 = new CGuid(CLSID_.MEDIATYPE_Video);

            capGraph.FindPin(filter, PinDirection.Output, guid, guid2, true, 0, out reqPin);

            if (reqPin == null)
            {
                return -1;
            }

            IAMStreamConfig config = (IAMStreamConfig)reqPin;
            if (config == null)
            {
                Marshal.ReleaseComObject(reqPin);
                reqPin = null;
                return -1;
            }

            int res = config.SetFormat(mediaType);
            Marshal.ReleaseComObject(reqPin);
            reqPin = null;
            return res;
        }
    }
}
