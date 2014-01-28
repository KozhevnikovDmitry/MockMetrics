using System;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Notifying;

namespace GU.MZ.BL.DomainLogic.Notify
{
    /// <summary>
    /// Класс ответсвенный за создание уведомлений заявителю
    /// </summary>
    public class DossierFileNotifier
    {
        public Notice AddNotice(DossierFile dossierFile, NoticeType noticeType, ScenarioStep scenarioStep)
        {
            var notice = Notice.CreateInstance();
            notice.Id = dossierFile.GetStep(scenarioStep).Id;
            notice.NoticeType = noticeType;
            notice.Stamp = DateTime.Now;
            notice.Email = dossierFile.TaskCustomerEmail;
            notice.Address = dossierFile.HolderAddress;
            dossierFile.GetStep(scenarioStep).Notice = notice;
            return notice;
        }
    }
}
