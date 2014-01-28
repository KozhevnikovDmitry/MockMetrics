using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GU.Enisey.BL;
using GU.Enisey.Contract;
using GU.Enisey.Contract.InquiryInternalPortType10;

namespace GU.Enisey.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Single,
                     Namespace = "urn://x-artefacts-it-ru/dob/poltava/inquiry-internal/1.0")]
    public class InquiryInternalService : InquiryInternalPortType10
    {
        public InquiryInternalService()
        {
            EniseyFacade.Initialize();
        }

        public getInquiryStateResponse1 getInquiryState(getInquiryStateRequest1 request)
        {
            throw new NotImplementedException();
        }

        public setInquiryResponse1 setInquiry(setInquiryRequest1 request)
        {
            var response = InquiryInternalLogic.setInquiry(request.setInquiryRequest);
            return new setInquiryResponse1(response);
        }
    }
}
