using System;
using System.Collections.Generic;
using System.Text;

namespace DirectShowNETCF.Guids
{

    public class CLSID_
    {
        public static readonly Guid FilterGraph = new Guid("E436EBB3-524F-11CE-9F53-0020AF0BA770");
        public static readonly Guid Camera = new Guid("CB998A05-122C-4166-846A-933E4D7E3C86");
        public static readonly Guid Audio = new Guid("A32942B7-920C-486b-B0E6-92A702A99B35");
        public static readonly Guid VideoCapture = new Guid("F80B6E95-B55A-4619-AEC4-A10EAEDE980C");
        public static readonly Guid AudioCapture = new Guid("E30629D2-27E5-11CE-875D-00608CB78066");
        public static readonly Guid CWMV9EncMediaObject = new Guid("D23B90D0-144F-46BD-841D-59E4EB19DC59");
        public static readonly Guid DMOWrapperFilter = new Guid("94297043-BD82-4DFD-B0DE-8177739C6D20");
        public static readonly Guid DMO_Mp3 = new Guid("86A495AC-64CE-42DE-A13A-321ACC0F02DB");
        public static readonly Guid CaptureGraphBuilder = new Guid("BF87B6E0-8C27-11D0-B3F0-00AA003761C5");
        public static readonly Guid VideoRenderer = new Guid("4D4B1600-33AC-11CF-BF30-00AA0055595A");//new Guid("70E102B0-5556-11CE-97C0-00AA0055595A");
        public static readonly Guid AudioRender = new Guid("e30629d1-27e5-11ce-875d-00608cb78066");
        public static readonly Guid AsfMux = new Guid("F560AE42-6CDD-11d1-ADE2-0000F8754B99");
        public static readonly Guid ASFWriter = new Guid("4F4BA16C-CCB0-4D23-B1E8-672C7DFE4A02");
        public static readonly Guid FileWriter = new Guid("8596E5F0-0DA5-11d0-BD21-00A0C911CE86");
        public static readonly Guid IMGSinkFilter = new Guid("1D4D3676-96EF-4CD7-A3D7-07FAC0D0C585");
        public static readonly Guid PIN_CATEGORY_STILL = new Guid("FB6C428A-0353-11D1-905F-0000C0CC16BA");
        public static readonly Guid PIN_CATEGORY_PREVIEW = new Guid("FB6C4282-0353-11D1-905F-0000C0CC16BA");
        public static readonly Guid PIN_CATEGORY_CAPTURE = new Guid("FB6C4281-0353-11D1-905F-0000C0CC16BA");
        public static readonly Guid DMOCATEGORY_VIDEO_ENCODER = new Guid("33D9A760-90C8-11d0-BD43-00A0C911CE86");
        public static readonly Guid DMOCATEGORY_AUDIO_DECODER = new Guid("57f2db8b-e6bb-4513-9d43-dcd2a6593125");
        public static readonly Guid MEDIATYPE_Video = new Guid("73646976-0000-0010-8000-00AA00389B71");
        public static readonly Guid MEDIATYPE_Audio = new Guid("73647561-0000-0010-8000-00AA00389B71");
        public static readonly Guid MEDIASUBTYPE_Asf = new Guid("3DB80F90-9412-11d1-ADED-0000F8754B99");
        public static readonly Guid VideoInfo = new Guid("05589F80-C356-11CE-BF01-00AA0055595A");
        public static readonly Guid VideoInfo2 =  new Guid("F72A76A0-EB0A-11D0-ACE4-0000C0CC16BA");
        public static readonly Guid AVISplitter = new Guid("1B544C20-FD0B-11CE-8C63-00AA0044B51E");
        public static readonly Guid FileSource = new Guid("E436EBB5-524F-11CE-9F53-0020AF0BA770");
    }

    public class IID_
    {
        public static readonly Guid IFilterGraph2 = new Guid("36B73882-C2C8-11CF-8B46-00805F6CEF60");
        public static readonly Guid IBaseFilter = new Guid("56A86895-0AD4-11CE-B03A-0020AF0BA770");
        public static readonly Guid ICaptuGraphBuilder2 = new Guid("93E5A4E0-2D50-11D2-ABFA-00A0C9C6E38D");
    }
}
