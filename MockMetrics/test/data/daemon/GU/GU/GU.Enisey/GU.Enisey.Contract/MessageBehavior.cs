using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace GU.Enisey.Contract
{
    // решение поперто отсюда:
    // http://blog.aggregatedintelligence.com/2010/06/wcf-transportwithmessagecredential-and.html

    public class MessageBehavior : IEndpointBehavior
    {
        string _username;
        string _password;

        public MessageBehavior(string username, string password)
        {
            _username = username;
            _password = password;
        }

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector(_username, _password));
        }
        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        { }
        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        { }

    }

    public class MessageInspector : IClientMessageInspector
    {
        string _username;
        string _password;

        public MessageInspector(string username, string password)
        {
            _username = username;
            _password = password;
        }

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, Object correlationState)
        {
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            request.Headers.Clear();

            string headerText =
                "<wsse:Security xmlns:common=\"urn://x-artefacts-it-ru/dob/poltava/common-types/1.5\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
                "<wsse:UsernameToken wsu:Id=\"UsernameToken-1\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
                "<wsse:Username>{0}</wsse:Username>" +
                "<common:PasswordSaltedDigest Type=\"http://www.w3.org/2001/04/xmldsig-more#gostr341194\">{1}</common:PasswordSaltedDigest>" +
                "</wsse:UsernameToken>" +
                "</wsse:Security>";


            headerText = string.Format(headerText, _username, _password);

            XmlDocument MyDoc = new XmlDocument();
            MyDoc.LoadXml(headerText);
            XmlElement myElement = MyDoc.DocumentElement;

            MessageHeader myHeader = MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", myElement, false);
            request.Headers.Add(myHeader);

            return Convert.DBNull;
        }
    }
}
