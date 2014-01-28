using System.Collections.Generic;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertiseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сущности Результат документарной проверки в списке результатов
    /// </summary>
    public class DocExpertiseResultItemListVm : SmartListItemVm<DocumentExpertiseResult>
    {
        public override void Initialize(DocumentExpertiseResult entity)
        {
            base.Initialize(entity);
            DocumentValidList = new Dictionary<bool, string>();
            DocumentValidList[true] = "Соответствует";
            DocumentValidList[false] = "Не соответствует";
        }

        #region Binding Properties

        /// <summary>
        /// Id спецификации документа прилагаемого к заявке
        /// Корректность документа отображается в результате док проверки
        /// </summary>
        public int ExpertedDocumentId
        {
            get
            {
                return Entity.ExpertedDocumentId;
            }
            set
            {
                if (Entity.ExpertedDocumentId != value)
                {
                    Entity.ExpertedDocumentId = value;
                    RaisePropertyChanged(() => ExpertedDocumentId);
                }
            }
        }

        /// <summary>
        /// Список документов, которы могут участововать в проверке
        /// </summary>
        public List<ExpertedDocument> ExpertedDocumentList { get; set; }

        /// <summary>
        /// Флаг соответствия, валидности документа
        /// </summary>
        public bool IsDocumentValid
        {
            get
            {
                return Entity.IsDocumentValid;
            }
            set
            {
                if (Entity.IsDocumentValid != value)
                {
                    Entity.IsDocumentValid = value;
                    RaisePropertyChanged(() => IsDocumentValid);
                }
            }
        }

        /// <summary>
        /// Словарь вариантов валидности документа
        /// </summary>
        public Dictionary<bool, string> DocumentValidList { get; private set; }

        /// <summary>
        /// Примечание к результату проверки
        /// </summary>
        public string DocumentInvalidNote
        {
            get
            {
                return Entity.DocumentInvalidNote;
            }
            set
            {
                if (Entity.DocumentInvalidNote != value)
                {
                    Entity.DocumentInvalidNote = value;
                    RaisePropertyChanged(() => DocumentInvalidNote);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => IsDocumentValid);
            RaisePropertyChanged(() => DocumentInvalidNote);
        }
    }
}
