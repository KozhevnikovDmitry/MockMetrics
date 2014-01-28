using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using Common.Types.Exceptions;
using GU.Enisey.Contract;
using GU.Enisey.BL.TaskConverters;
using GU.Enisey.DataModel;

namespace GU.Enisey.BL
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Single)]
    public class AppInquiryNotifierService : ApplicationInquiryNotifierPortType
    {
        public AppInquiryNotifierService()
        {
            EniseyFacade.Initialize();
        }

        #region notifyApplicationSubmitted

        public notifyApplicationSubmittedResponse1 notifyApplicationSubmitted(notifyApplicationSubmittedRequest1 request)
        {
            using (var db = EniseyFacade.GetDbManager())
            {
                var taskReceive = TaskReceive.CreateInstance();
                taskReceive.Stamp = DateTime.Now;
                var response = new notifyApplicationSubmittedResponse();

                try
                {
                    taskReceive.ReceivedMessage = SerializeUtils.GetXmlString(request);
                    var req = request.notifyApplicationSubmittedRequest;
                    var appInfo = req.applicationInfo; // Информация  о найденной заявке
                    taskReceive.EniseyAppId = appInfo.applicationID; // Идентификатор заявки.

                    #region тут статусы, пока забьем

                    // appInfo.applicationStateInfo; // ??? статус
                    // appInfo.applicationStateInfo.applicationStateName; // Наименование статуса заявки
                    // appInfo.applicationStateInfo.applicationStateTransitionTimestamp; // Дата и время перевода заявки в данный статус
                    // appInfo.applicationStateInfo.applicationStateTransitionComment; // Комментарий, указанный при переводе статуса

                    #endregion

                    #region это все межвед, пока неактуально

                    // Тип запроса и идентификатор запроса в Полтаве; это все межвед, пока неактуально
                    inquiryTypeInfoType[] inquiryType = req.inquiryType;
                    // inquiryType[0].inquiryTypeName; // Наименование типа запроса
                    // inquiryType[0].inquiryTypeCode; // Код типа запроса
                    // inquiryType[0].regulatedPeriod; // Регламентированный срок получения ответа в днях с момента отправки запроса
                    // inquiryType[0].regulatedPeriodSpecified; // ???

                    #endregion

                    // Информация  о содержимом заявки
                    var content = req.applicationContent;

                    #region нахера нужны эти три поля??

                    // content.applicationContentID; // Идентификатор содержимого заявки
                    // content.contentDateTime; // Дата создания содежимого заявки
                    // content.@namespace; // Namespace контента

                    #endregion

                    // большего 1 не ждем
                    if (content.contentData.Items.Count() > 1)
                        throw new BLLException("content.contentData.Items.Count() > 1");
                    // ждем только контент
                    if (content.contentData.ItemsElementName[0] != ItemsChoiceType.XMLContent)
                        throw new BLLException("content.contentData.ItemsElementName[0] != ItemsChoiceType.XMLContent");
                    // получили из первого элемента Task
                    var xelement = (XElement) content.contentData.Items[0];
                    var _taskConverterManager = new TaskConverterManager();
                    var task = _taskConverterManager.ConvertFromEniseyXml(xelement);

                    // тут в таск нужно положить остальные поля из прилетевших данных
                    task.CreateDate = appInfo.applicationSubmissionTimestamp; // Дата и время создания заявки
                    // appInfo.applicant; // Заявитель (organizationInfoType/personInfoType)

                    // сохраняем в базу прилетевшую заявку
                    db.SaveDomainObject(task);
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

                var resp = new notifyApplicationSubmittedResponse1(response);

                try
                {
                    taskReceive.SendedMessage = SerializeUtils.GetXmlString(resp);
                    // сохранили инфу по обращению к сервису
                    db.SaveDomainObject(taskReceive);
                }
                catch(Exception ex)
                {
                    try
                    {
                        string appName = "Enisey Service";
                        if (!System.Diagnostics.EventLog.SourceExists(appName))
                            System.Diagnostics.EventLog.CreateEventSource(appName, "Application");
                        var eventLog = new EventLog();
                        eventLog.Source = appName;
                        eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Warning);
                    }
                    catch(Exception)
                    {
                    }
                }

                return resp;
            }
        }

        #endregion

        #region notifyApplicationState

        public notifyApplicationStateResponse1 notifyApplicationState(notifyApplicationStateRequest1 request)
        {
            var response = new notifyApplicationStateResponse();
            response.Item = new errorType()
                                {errorMessage = "AppInquiryNotifierService.notifyApplicationState not implemented"};
            return new notifyApplicationStateResponse1(response);
        }

        #endregion

        #region notifyInquiryState

        public notifyInquiryStateResponse1 notifyInquiryState(notifyInquiryStateRequest1 request)
        {
            var response = new notifyInquiryStateResponse();
            response.Item = new errorType()
                                {errorMessage = "AppInquiryNotifierService.notifyInquiryState not implemented"};
            return new notifyInquiryStateResponse1(response);
        }

        #endregion

    }
}
