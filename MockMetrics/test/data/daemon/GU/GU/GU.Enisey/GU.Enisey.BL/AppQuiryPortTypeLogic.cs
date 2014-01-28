using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using GU.BL;
using GU.DataModel;
using Common.Types.Exceptions;
using System.ServiceModel;
using GU.BL.Policy;
using Common.DA.ProviderConfiguration;
using System.Configuration;
using GU.Enisey.Contract;
using GU.Enisey.DataModel;
using Common.Types.Extensions;
using BLToolkit.Data.Linq;
using GU.Enisey.BL;

using GU.Enisey.Contract.AppQuiryPortType101;

namespace GU.Enisey.BL
{
    public static class AppQuiryPortTypeLogic
    {
        private const int _triesCount = 3; // magic number

        #region SetApplication

        public static void SetApplicationList()
        {
            System.Diagnostics.Debug.Print("===>>> STARTING SetApplicationList <<<===");

            using (var db = EniseyFacade.GetDbManager())
            {
                var detList = (from sd in db.GetTable<TaskSendDetail>()
                               group sd by sd.TaskSendId into g
                               where g.Count() >= _triesCount
                               select g.Key);

                var sendList = (from s in db.GetTable<TaskSend>()
                                where !detList.Contains(s.Id)
                                select s);

                foreach (var i in sendList)
                {
                    SetApplication(i.TaskId, i.Id);
                }
            }
        }

        private static void SetApplication(int taskId, int taskSendId)
        {
            setApplicationResponse response = null;
            string eniseyAppId = null;
            TaskSendDetail taskSendDetail = TaskSendDetail.CreateInstance();
            taskSendDetail.TaskSendId = taskSendId;

            try
            {
                // Загрузили таск из базы
                var task = GuFacade.GetDataMapper<Task>().Retrieve(taskId);
                var appRequest = GetSetApplicationRequest(task);

                // сохранили отправляемое сообщение
                taskSendDetail.SendedMessage = SerializeUtils.GetXmlString(appRequest);

                MessageBehavior messageBehavior = new MessageBehavior("krasinform", "eb1674ca0d379a3aca489942e16449b0");
                var client = new AppQuiryPortType101Client();
                client.Endpoint.Behaviors.Add(messageBehavior);
                client.Using(serv =>
                {
                    response = serv.setApplication(appRequest);
                });
            }
            catch (BLLException ex)
            {
                // не смогли загрузить из базы/создать xml
                // делать ли еще попытку (ну, например, база отвалилась) или сразу записывать Task в бракованные?..
                taskSendDetail.State = SendState.ConvertFail;
                taskSendDetail.ErrorMessage = ex.ToString();
            }
            catch (CommunicationException ex)
            {
                // проблемы с доступом к сервису
                // вероятно, тут еще надо пытаццо (связь - дерьмо)
                taskSendDetail.State = SendState.SendFail;
                taskSendDetail.ErrorMessage = ex.ToString();
            }
            catch (InvalidOperationException)
            {
                // сюда мы попадем если будет неверно установлен логин/пароль
                // UPD: это было бы актуально, будь на той стороне человеческий сервис с нормалтной привязкой;
                //      а т.к. сообщение меняется ручками - здесь мы окажемся неизвестно в каком случае
                throw;
            }

            if (response != null)
            {
                // сохранили полученное сообщение
                taskSendDetail.ReceivedMessage = SerializeUtils.GetXmlString(response);

                // разбор ответа
                if (response.Items[0] is errorType)
                {
                    var err = response.Items[0] as errorType;
                    // куда-то положить их ошибку
                    taskSendDetail.State = SendState.ProcessingFail;
                    taskSendDetail.ErrorMessage = string.Format("error code: {0}; error message: {1}", err.executionResultCode, err.errorMessage);
                }
                else
                {
                    // TODO: что-то сделать с inquiryTypeInfoType
                    //       приходит только гуид строкой в случае успеха и errorType с ошибкой.
                    //       что это вообще за тип?
                    taskSendDetail.State = SendState.Success;
                    eniseyAppId = response.Items[0].ToString();
                }
            }

            using (var db = EniseyFacade.GetDbManager())
            {
                taskSendDetail.Stamp = DateTime.Now;
                db.SaveDomainObject(taskSendDetail);
                
                // апдейт текущего статуса в TaskSend и (опционально) eniseyAppId
                var uo = db.TaskSend.Where(x => x.Id == taskSendId)
                    .Set(x => x.CurrentState, taskSendDetail.State);
                if (eniseyAppId != null)
                    uo = uo.Set(x => x.EniseyAppId, eniseyAppId);
                uo.Update();
            }

            System.Diagnostics.Debug.Print(string.Format("SetApplication client completed; taskSendId == {0}", taskSendId));
        }

        private static setApplicationRequest GetSetApplicationRequest(Task task)
        {
            // Получили по таску XElement-контент
            var converterManager = EniseyFacade.GetConverterManager();
            var xmlContent = converterManager.ExportTaskToXml(task);

            var ar = new setApplicationRequest();
            // заполнили Content
            ar.contentData = new contentDataType();
            ar.contentData.Items = new object[] { xmlContent.ToXmlElement() }; // сконвертили в XmlElement
            ar.contentData.ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.XMLContent };
            // заполнили departmentID
            //ar.departmentID = ... Но т.к. пока неизвестно что это - не делаем
            // заполнили applicantInfoType
            // по идее, тут заполняется инфа о том, для кого заявление, а не кто заполнил
            // (и это еще может быть либо юр. либо фз.лицо)
            // но т.к. пока, опять же, нихера неизвестно - заполняется из Task'а инфа о том, кто заполнял
            ar.aplicantInfo = new applicantInfoType();
            ar.aplicantInfo.Item = new personInfoType()
            {
                personEmail = task.CustomerEmail,
                phone = task.CustomerPhone,
                personFirstName = task.CustomerFio,
                personLastName = task.CustomerFio
            };

            return ar;
        }

        #endregion

        #region ChangeApplicationState

        public static void SetStateList()
        {
            System.Diagnostics.Debug.Print("===>>> STARTING SetStateList <<<===");

            using (var db = EniseyFacade.GetDbManager())
            {
                var detList = (from sd in db.GetTable<TaskStateSendDetail>()
                               group sd by sd.TaskStateSendId into g
                               where g.Count() >= _triesCount
                               select g.Key);

                var sendList = (from s in db.GetTable<TaskStateSend>()
                                where !detList.Contains(s.Id)
                                   && s.CurrentState != SendState.Success
                                select s);

                foreach (var i in sendList)
                {
                    SetState(i.TaskStateId, i.Id);
                }
            }
        }

        private static void SetState(int taskStateId, int taskStateSendId)
        {
            changeApplicationStateResponse response = null;
            string eniseyAppId = null;
            var sendDetail = TaskStateSendDetail.CreateInstance();
            sendDetail.TaskStateSendId = taskStateSendId;

            try
            {
                TaskStatus taskStatus = null;

                using(var db = EniseyFacade.GetDbManager())
                {
                    // Загрузили объект из базы
                    taskStatus = db.RetrieveDomainObject<TaskStatus>(taskStateId);

                    // взяли из TaskReceive все записи по айдишнику таска
                    var taskReceiveList = db.TaskReceive.Where(x => x.TaskId == taskStatus.TaskId).ToList();

                    // заявка к нам пришла не из Енисея, статус отсылать некуда
                    if (taskReceiveList.Count == 0)
                        return;

                    // каким-то неведомым образом в TaskReceive оказалось две записи с одним TaskId
                    // TODO: надо как-то обработать и другие паранормальные ситуации
                    if (taskReceiveList.Count > 1)
                        throw new BLLException("TaskReceive.Count > 1");

                    eniseyAppId = taskReceiveList.First().EniseyAppId;

                    // у нас еще нет гуида из их системы
                    if (eniseyAppId == null)
                        throw new BLLException("eniseyAppId is null");
                }

                var request = new changeApplicationStateRequest();
                request.applicationID = eniseyAppId;
                request.applicationStateName = GetStateString(taskStatus.State);
                request.applicationStateComment = taskStatus.Note;
                // когда-нибудь
                //request.appendContent

                // сохранили отправляемое сообщение
                sendDetail.SendedMessage = SerializeUtils.GetXmlString(request);

                var messageBehavior = new MessageBehavior("krasinform", "eb1674ca0d379a3aca489942e16449b0");
                var client = new AppQuiryPortType101Client();
                client.Endpoint.Behaviors.Add(messageBehavior);
                client.Using(serv =>
                {
                    response = serv.changeApplicationState(request);
                });
            }
            catch (BLLException ex)
            {
                // не смогли загрузить из базы/что-то еще
                sendDetail.State = SendState.ConvertFail;
                sendDetail.ErrorMessage = ex.ToString();
            }
            catch (CommunicationException ex)
            {
                // проблемы с доступом к сервису
                // вероятно, тут еще надо пытаццо (связь - дерьмо)
                sendDetail.State = SendState.SendFail;
                sendDetail.ErrorMessage = ex.ToString();
            }
            catch(Exception ex)
            {
                // вообще неизведанная хуета
                sendDetail.State = SendState.Success;
                sendDetail.ErrorMessage = ex.ToString();
            }

            if (response != null)
            {
                // сохранили полученное сообщение
                sendDetail.ReceivedMessage = SerializeUtils.GetXmlString(response);

                // разбор ответа
                if (response.Item is errorType)
                {
                    var err = response.Item as errorType;
                    // куда-то положить их ошибку
                    sendDetail.State = SendState.ProcessingFail;
                    sendDetail.ErrorMessage = string.Format("error code: {0}; error message: {1}", err.executionResultCode, err.errorMessage);
                }
                else
                {
                    sendDetail.State = SendState.Success;
                }
            }

            using (var db = EniseyFacade.GetDbManager())
            {
                sendDetail.Stamp = DateTime.Now;
                db.SaveDomainObject(sendDetail);

                // апдейт текущего статуса в TaskStateSend
                db.TaskStateSend.Where(x => x.Id == taskStateSendId)
                    .Set(x => x.CurrentState, sendDetail.State)
                    .Set(x => x.EniseyAppId, eniseyAppId)
                    .Update();
            }
        }

        private static string GetStateString(TaskStatusType state)
        {
            switch (state)
            {
                case TaskStatusType.CheckupWaiting:
                    return "Ожидает проверки";

                case TaskStatusType.Accepted:
                    return "Принята к рассмотрению";

                case TaskStatusType.Working:
                    return "В работе";

                case TaskStatusType.Ready:
                    return "Готово для получения";

                case TaskStatusType.Done:
                    return "Услуга предоставлена";

                case TaskStatusType.Rejected:
                    return "В услуге отказано";

                default:
                    throw new BLLException("Wrong state!");
            }
        }

        #endregion

        #region Test

        private static void FillTaskSend()
        {
            using (var db = EniseyFacade.GetDbManager())
            {
                var taskIdList = (from t in db.GetTable<Task>()
                                  select t.Id).ToList();

                foreach (var id in taskIdList)
                {
                    db.Into(db.GetTable<TaskSend>())
                        .Value(x => x.TaskId, id)
                        .Value(x => x.CurrentState, SendState.None)
                        .Insert();
                }
            }
        }

        public static void TestSetApplication()
        {
            SetApplication(9002, 10299);
        }

        public static void TestSetState()
        {
            SetState(27305, 20);
        }

        #endregion
    }
}