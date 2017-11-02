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
            try
            {
                CFilterList filters_ = new CFilterList();
                filters_.Assign(graph);
                for (int i = 0; i < filters_.Count; i++)
                {
                    CPinList pins_ = new CPinList();
                    pins_.Assign(filters_[i]);
                    for (int j = 0; j < pins_.Count; j++)
                    {
                        pins_[j].Disconnect();
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
            catch (System.Runtime.InteropServices.COMException)
            {
            }
            catch (Exception)
            {
            }
            catch
            {
            }
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

        public static bool ConnectPins(IGraphBuilder graph, IBaseFilter output, IBaseFilter input)
        {
            CPinList inputs = new CPinList();
            inputs.Assign(input);
            CPinList outputs = new CPinList();
            outputs.Assign(output);

            int hr = 0;

            for (int i = 0; i < outputs.Count; ++i)
            {
                hr = graph.Connect(outputs[i], inputs[0]);
                if (hr > -1)
                    break;
            }

            inputs.Free();
            outputs.Free();

            return hr > -1;
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
