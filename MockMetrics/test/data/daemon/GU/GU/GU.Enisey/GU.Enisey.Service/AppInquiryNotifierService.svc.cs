using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GU.Enisey.BL;
using GU.Enisey.Contract;

namespace GU.Enisey.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Single,
                     Namespace = "urn://x-artefacts-it-ru/dob/poltava/application-inquiry-notify/1.0")]
    public class AppInquiryNotifierService : ApplicationInquiryNotifierPortType
    {
        public AppInquiryNotifierService()
        {
            EniseyFacade.Initialize();
        }

        public notifyApplicationSubmittedResponse1 notifyApplicationSubmitted(notifyApplicationSubmittedRequest1 request)
        {
            var response = AppInquiryNotifierLogic.notifyApplicationSubmitted(request.notifyApplicationSubmittedRequest);
            return new notifyApplicationSubmittedResponse1(response);
        }

        public notifyApplicationStateResponse1 notifyApplicationState(notifyApplicationStateRequest1 request)
        {
            var response = AppInquiryNotifierLogic.notifyApplicationState(request.notifyApplicationStateRequest);
            return new notifyApplicationStateResponse1(response);
        }

        public notifyInquiryStateResponse1 notifyInquiryState(notifyInquiryStateRequest1 request)
        {
            var response = AppInquiryNotifierLogic.notifyInquiryState(request.notifyInquiryStateRequest);
            return new notifyInquiryStateResponse1(response);
        }
    }
}
