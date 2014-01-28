using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using GU.Enisey.Contract;

namespace GU.Enisey.Stub.Server
{
    //public class AppQuiryServiceStub : AppQuiryPortType10
    //{
    //    public changeApplicationStateResponse changeApplicationState(changeApplicationStateRequest request)
    //    {
    //        Debug.Print("changeApplicationState");
    //        return null;
    //    }

    //    public setApplicationResponse setApplication(setApplicationRequest request)
    //    {
    //        Debug.Print("setApplication");
    //        return null;
    //    }

    //    public getInquiryStateResponse getInquiryState(getInquiryStateRequest request)
    //    {
    //        Debug.Print("getInquiryState");
    //        return null;
    //    }

    //    public setInquiryResponse setInquiry(setInquiryRequest request)
    //    {
    //        Debug.Print("setInquiry");
    //        return null;
    //    }

    //    public changeApplicationStateResponse1 changeApplicationState(changeApplicationStateRequest1 request)
    //    {
    //        Debug.Print("changeApplicationState1");
    //        var val = new changeApplicationStateResponse1();
    //        val.changeApplicationStateResponse = new changeApplicationStateResponse();
    //        val.changeApplicationStateResponse.Item = new tagType();
    //        return val;
    //        //return new changeApplicationStateResponse1();
    //    }

    //    public setApplicationResponse1 setApplication(setApplicationRequest1 request)
    //    {
    //        Debug.Print("setApplication1");
    //        var val = new setApplicationResponse1();
    //        val.setApplicationResponse = new setApplicationResponse();
    //        //val.setApplicationResponse.Items = new object[1] { "setApplicationResponse1 item" };
    //        val.setApplicationResponse.Items = new object[1];
    //        //val.setApplicationResponse.Items[0] = ((XmlElement)request.setApplicationRequest.contentData.Items[0]).OuterXml;
    //        val.setApplicationResponse.Items[0] = ((personInfoType)request.setApplicationRequest.aplicantInfo.Item).personFirstName;
    //        return val;
    //        //return new setApplicationResponse1();
    //    }

    //    public getInquiryStateResponse1 getInquiryState(getInquiryStateRequest1 request)
    //    {
    //        Debug.Print("getInquiryState1");
    //        var val = new getInquiryStateResponse1();
    //        val.getInquiryStateResponse = new getInquiryStateResponse();
    //        val.getInquiryStateResponse.Items = new object[1];
    //        val.getInquiryStateResponse.Items[0] = new errorType() { errorMessage = "asd123", executionResultCode = 123 };
    //        return val;
    //        //return new getInquiryStateResponse1();
    //    }

    //    public setInquiryResponse1 setInquiry(setInquiryRequest1 request)
    //    {
    //        Debug.Print("setInquiry1");
    //        var val = new setInquiryResponse1();
    //        val.setInquiryResponse = new setInquiryResponse();
    //        val.setInquiryResponse.Items = new object[1] { "setInquiryResponse1 item" };
    //        var qweqw = val.setInquiryResponse.Items;
    //        return val;
    //        //return new setInquiryResponse1();
    //    }

    //}
}
