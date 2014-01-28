using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор томов лицензионного дела
    /// </summary>
    public class DossierFileValidator : AbstractDomainValidator<DossierFile>
    {
        private readonly IDomainValidator<Inspection> _inspectionValidator;
        private readonly IDomainValidator<DocumentExpertise> _expertiseValidator;
        private readonly IDomainValidator<StandartOrder> _stanOrdValidator;
        private readonly IDomainValidator<InspectionOrder> _insOrdValidator;
        private readonly IDomainValidator<ExpertiseOrder> _expOrdValidator;
        private readonly IDomainValidator<DossierFileServiceResult> _servResultValidator;
        private readonly IDomainValidator<Notice> _noticeValidator;
        private readonly IDomainValidator<ViolationNotice> _violNoticeValidator;
        private readonly IDomainValidator<ViolationResolveInfo> _violResInfoValidator;

        public DossierFileValidator(IDomainValidator<Inspection> inspectionValidator,
                                    IDomainValidator<DocumentExpertise> expertiseValidator,
                                    IDomainValidator<StandartOrder> stanOrdValidator,
                                    IDomainValidator<InspectionOrder> insOrdValidator,
                                    IDomainValidator<ExpertiseOrder> expOrdValidator,
                                    IDomainValidator<DossierFileServiceResult> servResultValidator,
                                    IDomainValidator<Notice> noticeValidator,
                                    IDomainValidator<ViolationNotice> violNoticeValidator,
                                    IDomainValidator<ViolationResolveInfo> violResInfoValidator)
        {
            _inspectionValidator = inspectionValidator;
            _expertiseValidator = expertiseValidator;
            _stanOrdValidator = stanOrdValidator;
            _insOrdValidator = insOrdValidator;
            _expOrdValidator = expOrdValidator;
            _servResultValidator = servResultValidator;
            _noticeValidator = noticeValidator;
            _violNoticeValidator = violNoticeValidator;
            _violResInfoValidator = violResInfoValidator;


            var dossierFile = DossierFile.CreateInstance();
            _validationActions[Util.GetPropertyName(() => dossierFile.RegNumber)] = ValidateRegNumber;
            _validationActions[Util.GetPropertyName(() => dossierFile.CreateDate)] = ValidateCreateDate;
        }

        public override ValidationErrorInfo Validate(DossierFile domainObject)
        {
            var result = base.Validate(domainObject);

            foreach (var dossierFileScenarioStep in domainObject.DossierFileStepList)
            {
                var stepResult = Validate(dossierFileScenarioStep);
                result.AddResult(stepResult);
            }

            if (domainObject.DossierFileServiceResult != null)
            {
                var servResult = _servResultValidator.Validate(domainObject.DossierFileServiceResult);
                result.AddResult(servResult);
            }

            return result;
        }

        private ValidationErrorInfo Validate(DossierFileScenarioStep fileStep)
        {
            var result = new ValidationErrorInfo();

            if (fileStep.Inspection != null)
            {
                var insResult = _inspectionValidator.Validate(fileStep.Inspection);
                result.AddResult(insResult, "Выездная проверка");
            }

            if (fileStep.DocumentExpertise != null)
            {
                var expResult = _expertiseValidator.Validate(fileStep.DocumentExpertise);
                result.AddResult(expResult, "Документарная проверка");
            }

            if (fileStep.Notice != null)
            {
                var noticeResult = _noticeValidator.Validate(fileStep.Notice);
                result.AddResult(noticeResult, "Уведомление");
            }

            if (fileStep.ViolationNotice != null)
            {
                var violNoticeResult = _violNoticeValidator.Validate(fileStep.ViolationNotice);
                result.AddResult(violNoticeResult, "Уведомление о нарушениях");
            }

            if (fileStep.ViolationResolveInfo != null)
            {

                var violResolveResult = _violResInfoValidator.Validate(fileStep.ViolationResolveInfo);
                result.AddResult(violResolveResult, "Информация о нарушениях");
            }

            if (fileStep.InspectionOrder != null)
            {
                var insOrdResult = _insOrdValidator.Validate(fileStep.InspectionOrder);
                result.AddResult(insOrdResult, "Приказ о выездной проверке");
            }

            if (fileStep.ExpertiseOrder != null)
            {
                var expOrdResult = _expOrdValidator.Validate(fileStep.ExpertiseOrder);
                result.AddResult(expOrdResult, "Приказ о документарной проверке");
            }

            if (fileStep.StandartOrderList != null)
            {
                foreach (var standartOrder in fileStep.StandartOrderList)
                {
                    var stanOrdResult = _stanOrdValidator.Validate(standartOrder);
                    result.AddResult(stanOrdResult, standartOrder.OrderType.GetDescription());
                }
            }

            return result;
        }

        private string ValidateRegNumber(DossierFile dossierFile)
        {
            return dossierFile.RegNumber <= 0 ? "Поле регистрационный номер должно быть заполнено" : null;
        }

        private string ValidateCreateDate(DossierFile dossierFile)
        {
            return dossierFile.CreateDate != new DateTime() ? null : "Поле дата создания должно быть заполнено";
        }
    }
}
