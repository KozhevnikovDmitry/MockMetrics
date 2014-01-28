using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common.BL.Exceptions;
using Common.Types.Exceptions;
using Common.Types.Extensions;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using GU.DataModel.Inquiry;
using GU.Enisey.BL.Converters.Common;
using GU.Enisey.Contract;
using GU.Enisey.DataModel;
using Ionic.Zip;
using InquiryType = GU.Enisey.DataModel.InquiryType;

using GU.Enisey.Contract.ApplicationInquiryNotifierPortType12;

namespace GU.Enisey.BL
{
    public static class AppInquiryNotifierLogic
    {
        #region notifyApplicationSubmitted

        public static notifyApplicationSubmittedResponse notifyApplicationSubmitted(notifyApplicationSubmittedRequest request)
        {
            using (var db = EniseyFacade.GetDbManager())
            {
                var taskReceive = TaskReceive.CreateInstance();
                taskReceive.Stamp = DateTime.Now;
                var response = new notifyApplicationSubmittedResponse();

                try
                {
                    taskReceive.ReceivedMessage = SerializeUtils.GetXmlString(request);
                    var appInfo = request.applicationInfo; // Информация  о найденной заявке
                    taskReceive.EniseyAppId = appInfo.applicationID; // Идентификатор заявки

                    #region тут статусы, пока забьем

                    // appInfo.applicationStateInfo; // ??? статус
                    // appInfo.applicationStateInfo.applicationStateName; // Наименование статуса заявки
                    // appInfo.applicationStateInfo.applicationStateTransitionTimestamp; // Дата и время перевода заявки в данный статус
                    // appInfo.applicationStateInfo.applicationStateTransitionComment; // Комментарий, указанный при переводе статуса

                    #endregion

                    #region это все межвед, пока неактуально

                    // Тип запроса и идентификатор запроса в Полтаве; это все межвед, пока неактуально
                    // inquiryTypeInfoType[] inquiryType = request.inquiryType;
                    // inquiryType[0].inquiryTypeName; // Наименование типа запроса
                    // inquiryType[0].inquiryTypeCode; // Код типа запроса
                    // inquiryType[0].regulatedPeriod; // Регламентированный срок получения ответа в днях с момента отправки запроса
                    // inquiryType[0].regulatedPeriodSpecified; // ???

                    #endregion

                    // Информация  о содержимом заявки
                    var content = request.applicationContent;

                    #region нахера нужны эти три поля??

                    // content.applicationContentID; // Идентификатор содержимого заявки
                    // content.contentDateTime; // Дата создания содежимого заявки
                    // content.@namespace; // Namespace контента

                    #endregion

                    // большего 1 не ждем
                    if (content.contentData.Items.Count() > 1)
                        throw new BLLException("content.contentData.Items.Count() > 1");

                    // получили из первого элемента Task
                    XmlData xmlData = null;
                    var contentData = content.contentData.Items[0];
                    var contentType = content.contentData.ItemsElementName[0];
                    // ждем либо xml либо binary, auxiliary игнорим
                    switch (contentType)
                    {
                        case ItemsChoiceType.XMLContent:
                            xmlData = new XmlData {Xml = (contentData as System.Xml.XmlElement).ToXElement()};
                            break;

                        case ItemsChoiceType.binaryContent:
                            var binContent = contentData as contentDataTypeBinaryContent;
                            xmlData = XmlDataUtils.LoadFromZip(binContent.Value, binContent.XMLFileName);
                            break;

                        default:
                            throw new BLLException("content.contentData.ItemsElementName[0] == ItemsChoiceType.auxiliaryXMLContent");
                    }

                    Task task = null;
                    try
                    {
                        var converterManager = EniseyFacade.GetConverterManager();
                        task = converterManager.ImportTaskFromXml(xmlData);
                    }
#if DEBUG
                    catch (Exception ex)
                    {
                        // TODO: костыль, чтобы принять заявку в детские садики; посмотреть на межвед
                        if (ex.ToString()
                              .Contains("{urn://x-artefacts-it-ru/dob/state-services/krsk/DOU/10.2.1}RecordDOU"))
                        {
                            var tp = GuFacade.GetTaskPolicy();
                            var service = GuFacade.GetDictionaryManager().GetDictionaryItem<Service>(45); // тестовая услуга
                            // 45 - труды, содействие в поиске работы
                            task = tp.CreateDefaultTask(service);
                            
                            // смена статуса в обход policy чтобы не выполнялась валидация
                            //tp.SetStatus(TaskStatusType.CheckupWaiting, string.Empty, task);
                            task.CurrentState = TaskStatusType.CheckupWaiting;
                            TaskStatus ts = TaskStatus.CreateInstance();
                            ts.Stamp = DateTime.Now;
                            ts.Task = task;
                            ts.TaskId = task.Id;
                            ts.User = GuFacade.GetDbUser();
                            ts.UserId = ts.User.Id;
                            ts.State = task.CurrentState;
                            task.StatusList.Add(ts);
                        }
                        else
                        {
                            throw;
                        }
                    }
#else
                    catch (Exception)
                    {
                        throw;
                    }
#endif

                    // тут в таск нужно положить остальные поля из прилетевших данных
                    task.CreateDate = appInfo.applicationSubmissionTimestamp; // Дата и время создания заявки

                    var agency = GetAgencyByEniseyCode(task.Service.ServiceGroup.Agency, request.department.departmentID);
                    if (agency != null)
                    {
                        task.Agency = agency;
                        task.AgencyId = agency.Id;
                    }

                    // appInfo.applicant; // Заявитель (organizationInfoType/personInfoType)

                    // сохраняем в базу прилетевшую заявку
                    var mapper = GuFacade.GetDataMapper<Task>();
                    task = mapper.Save(task);
                    taskReceive.TaskId = task.Id;

                    // Флаг - данные приняты
                    response.Item = new tagType();
                }
                catch (BLLException ex) // не смогли сконвертить/сохранить/отправитель-мудак/etc
                {
                    taskReceive.ErrorMessage = ex.ToString();
                }
                catch (Exception ex)
                {
                    taskReceive.ErrorMessage = ex.ToString();
                }

                // Ошибка, возвращается в случае неуспешного выполнения
                if (response.Item == null)
                    response.Item = new errorType();

                try
                {
                    taskReceive.SendedMessage = SerializeUtils.GetXmlString(response);
                    // сохранили инфу по обращению к сервису
                    db.SaveDomainObject(taskReceive);
                }
                catch (Exception)
                {
                }

                return response;
            }
        }

        #endregion

        #region notifyApplicationState

        public static notifyApplicationStateResponse notifyApplicationState(notifyApplicationStateRequest request)
        {
            var response = new notifyApplicationStateResponse();
            response.Item = new errorType() { errorMessage = "AppInquiryNotifierService.notifyApplicationState not implemented" };
            return response;
        }

        #endregion

        #region notifyInquiryState

        public static notifyInquiryStateResponse notifyInquiryState(notifyInquiryStateRequest request)
        {
            using (var db = EniseyFacade.GetDbManager())
            {
                var inquiryReceive = InquiryReceive.CreateInstance();
                inquiryReceive.Stamp = DateTime.Now;
                var response = new notifyInquiryStateResponse();

                try
                {
                    inquiryReceive.InquiryType = InquiryType.Outgoing;
                    inquiryReceive.ReceivedMessage = SerializeUtils.GetXmlString(request);
                    inquiryReceive.EniseyAppId = request.applicationID;
                    inquiryReceive.EniseyInquiryId = request.inquiryInfo.inquiryID;
                    inquiryReceive.EniseyInquiryState = (int)request.inquiryState.inquiryStateCode;
                    inquiryReceive.EniseyInquiryTypeName = request.inquiryInfo.inquiryTypeName;

                    // вытащили айдишник таска, если таковой имеется
                    inquiryReceive.TaskId = (from tr in db.TaskReceive
                                             where tr.EniseyAppId == inquiryReceive.EniseyAppId
                                             select tr.TaskId).FirstOrDefault();
                    Task task = null;
                    if (inquiryReceive.TaskId != null)
                        task = GuFacade.GetDataMapper<Task>().Retrieve(inquiryReceive.TaskId);

                    XmlData xmlData = null;
                    if (request.resultContent != null)
                    {
                        var contentData = request.resultContent.Items[0];
                        var contentType = request.resultContent.ItemsElementName[0];
                        // ждем либо xml либо binary, auxiliary игнорим
                        switch (contentType)
                        {
                            case ItemsChoiceType.XMLContent:
                                xmlData = new XmlData {Xml = (contentData as System.Xml.XmlElement).ToXElement()};
                                break;

                            case ItemsChoiceType.binaryContent:
                                var binContent = contentData as contentDataTypeBinaryContent;
                                xmlData = XmlDataUtils.LoadFromZip(binContent.Value, binContent.XMLFileName);
                                break;

                            default:
                                throw new BLLException("content.contentData.ItemsElementName[0] == ItemsChoiceType.auxiliaryXMLContent");
                        }

                        inquiryReceive.RequestContent = xmlData.Xml.ToString();
                    }

                    Inquiry inquiry = null;

                    switch (request.inquiryState.inquiryStateCode)
                    {
                        // начальный статус - создаем межвед запрос
                        case inquiryStateCodeType.SENDING: 
                            var converterManager = EniseyFacade.GetConverterManager();
                            inquiry = converterManager.ImportInquiryFromXml(xmlData, task);
                            //NOTE: в данных передаваемых от енисей-гу нету даты создания, нехорошо
                            //inquiry.CreateDate = 
                            break;

                        // получен ответ - меняем статус и устанавливаем результат для существующего запроса
                        case inquiryStateCodeType.RESPONSE_RECEIVED: 
                            throw new NotImplementedException();
                            break;
                        
                        // остальные статусы - просто смена статуса без доп действий
                        default:
                            if (xmlData != null)
                                throw new BLLException(
                                    "Заведено содержимое межвед запроса для статуса отличного от SENDING и RESPONSE_RECEIVED");

                            throw new NotImplementedException();
                    }

                    // сохраняем в базу прилетевшую заявку
                    var mapper = GuFacade.GetDataMapper<Inquiry>();
                    inquiry = mapper.Save(inquiry);
                    //TODO:
                    //inquiryReceive.InquiryId = inquiry.Id;

                    // Флаг - данные приняты
                    response.Item = new tagType();
                }
                catch (BLLException ex) // не смогли сконвертить/сохранить/отправитель-мудак/etc
                {
                    inquiryReceive.ErrorMessage = ex.ToString();
                }
                catch (Exception ex)
                {
                    inquiryReceive.ErrorMessage = ex.ToString();
                }

                // Ошибка, возвращается в случае неуспешного выполнения
                if (response.Item == null)
                    response.Item = new errorType();

                try
                {
                    inquiryReceive.SendedMessage = SerializeUtils.GetXmlString(response);
                    // сохранили инфу по обращению к сервису
                    db.SaveDomainObject(inquiryReceive);
                }
                catch (Exception)
                {
                }

                return response;
            }
        }

        #endregion

        #region Utils

        private static Agency GetAgencyByEniseyCode(Agency rootAgency, string departmentId)
        {
            var agencies = rootAgency.ChildAgencyList.Where(t => t.EniseyCode == departmentId).ToList();
            if (agencies.Count > 1)
                throw new BLLException(string.Format("Найдено несколько ведомств с кодом {0}", departmentId));
            return agencies.SingleOrDefault();
        }

        #endregion
    }
}
