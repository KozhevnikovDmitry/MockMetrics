using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Types.Exceptions;
using Common.Types.Extensions;
using GU.BL;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;
using GU.Enisey.Contract;
using GU.Enisey.DataModel;

using GU.Enisey.Contract.InquiryInternalPortType10;

namespace GU.Enisey.BL
{
    public static class InquiryInternalLogic
    {
        #region getInquiryState

        public static getInquiryStateResponse getInquiryState(getInquiryStateRequest request)
        {
            throw null;
        }

        #endregion

        #region setInquiry

        public static setInquiryResponse setInquiry(setInquiryRequest request)
        {
            throw null;
            /*
            using (var db = EniseyFacade.GetDbManager())
            {
                var interDepartmentReceive = InquiryReceive.CreateInstance();
                interDepartmentReceive.Stamp = DateTime.Now;
                var response = new setInquiryResponse();

                try
                {
                    interDepartmentReceive.ReceivedMessage = SerializeUtils.GetXmlString(request);
                    interDepartmentReceive.EniseyAppId = request.applicationID;
                    // request.departmentID; // Идентификатор подразделения - type="common:IDType"
                    // request.inquirySourceSystemID; // Идентификатор запроса системы инициатора запроса - type="common:IDType"
                    // request.inquiryTypeCode; // Идентификатор запроса - type="common:IDType"

                    // получили контент
                    XmlData xmlData = null;
                    var contentData = request.contentData.Items[0];
                    var contentType = request.contentData.ItemsElementName[0];
                    // ждем либо xml либо binary, auxiliary игнорим
                    switch (contentType)
                    {
                        case ItemsChoiceType.XMLContent:
                            xmlData = new XmlData { Xml = (contentData as System.Xml.XmlElement).ToXElement() };
                            break;

                        case ItemsChoiceType.binaryContent:
                            var binContent = contentData as contentDataTypeBinaryContent;
                            xmlData = XmlDataUtils.LoadFromZip(binContent.Value, binContent.XMLFileName);
                            break;

                        default:
                            throw new BLLException("content.contentData.ItemsElementName[0] == ItemsChoiceType.auxiliaryXMLContent");
                    }

                    interDepartmentReceive.RequestContent = xmlData.Xml.ToString();

                    // Флаг - данные приняты
                    response.Items[0] = "inquiryID"; // set inquiryID
                }
                catch (BLLException ex) // не смогли сконвертить/сохранить/отправитель-мудак/etc
                {
                    interDepartmentReceive.ErrorMessage = ex.ToString();
                }
                catch (Exception ex)
                {
                    interDepartmentReceive.ErrorMessage = ex.ToString();
                }

                // Ошибка, возвращается в случае неуспешного выполнения
                if (response.Items[0] == null)
                    response.Items[0] = new errorType();

                try
                {
                    interDepartmentReceive.SendedMessage = SerializeUtils.GetXmlString(response);
                    // сохранили инфу по обращению к сервису
                    db.SaveDomainObject(interDepartmentReceive);
                }
                catch (Exception)
                {
                }

                return response;
            }
            */
        }

        #endregion
    }
}
