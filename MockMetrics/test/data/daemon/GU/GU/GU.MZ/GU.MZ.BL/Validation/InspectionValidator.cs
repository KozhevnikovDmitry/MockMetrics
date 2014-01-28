using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Inspect;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Класс валидатор сущностей Выездная проверка 
    /// </summary>
    public class InspectionValidator : AbstractDomainValidator<Inspection>
    {
        public InspectionValidator()
        {
            var inspection = Inspection.CreateInstance();

            _validationActions[Util.GetPropertyName(() => inspection.ActStamp)] = ValidateActStamp;
            _validationActions[Util.GetPropertyName(() => inspection.StartStamp)] = ValidateStartStamp;
            _validationActions[Util.GetPropertyName(() => inspection.EndStamp)] = ValidateEndStamp;
            _validationActions[Util.GetPropertyName(() => inspection.InspectionNote)] = ValidateNote;
        }

        private string ValidateNote(Inspection inspection)
        {
            return string.IsNullOrEmpty(inspection.InspectionNote)
                ? "Поле Примечание к выездной проверке должно быть заполнено"
                : null;
        }

        private string ValidateActStamp(Inspection inspection)
        {
            if (inspection.ActStamp == new DateTime())
                return "Поле Дата акта проверки должно быть заполнено";

            if (inspection.ActStamp < inspection.EndStamp)
            {
                return "Дата акта не может быть ранее даты окончания проверки";
            }

            return null;
        }

        private string ValidateStartStamp(Inspection inspection)
        {
            if (inspection.StartStamp == new DateTime())
                return "Поле Дата начала проверки должно быть заполнено";

            if (inspection.StartStamp > inspection.EndStamp)
            {
                return "Дата начала проверки не может быть позднее даты окончания проверки";
            }

            return null;
        }

        private string ValidateEndStamp(Inspection inspection)
        {
            if (inspection.EndStamp == new DateTime())
                return "Поле Дата окончания проверки должно быть заполнено";

            if (inspection.StartStamp > inspection.EndStamp)
            {
                return "Дата начала проверки не может быть позднее даты окончания проверки";
            }

            return null;
        }
    }
}
