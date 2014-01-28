using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;

namespace GU.Enisey.Contract
{
    public class PortNameWsdlBehavior : IWsdlExportExtension, IEndpointBehavior
    {
        public string Name { get; set; }

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                context.WsdlPort.Name = Name;
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class PortNameWsdlBehaviorExtension : BehaviorExtensionElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                object value = this["name"];
                return value != null ? value.ToString() : string.Empty;
            }
            set { this["name"] = value; }
        }

        public override Type BehaviorType
        {
            get { return typeof(PortNameWsdlBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new PortNameWsdlBehavior { Name = Name };
        }
    }
}
