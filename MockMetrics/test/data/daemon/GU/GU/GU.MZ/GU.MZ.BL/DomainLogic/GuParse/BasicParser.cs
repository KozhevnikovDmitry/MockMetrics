using System.Collections.Generic;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.MzTask;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Парсер данных из заявок по услугам рассчитанным и на юридических, и на физических лиц
    /// </summary>
    public abstract class BasicParser : Parser
    {
        protected BasicParser(IContentImporter contentImporter, IContentMapper contentMapper)
            : base(contentImporter, contentMapper)
        {
        }

        protected override IEnumerable<ContentNode> GetRenewalTypeNodes(Task task)
        {
            var holderNode = GetHolderNode(task);
            var baseNode = ContentImporter.GetContentNode(holderNode.ParentContentNode, "Base/BaseChoice");
            if (baseNode == null)
            {
                throw new NoRenewalScenarioException(task);
            }

            return baseNode.ChildContentNodes;
        }

        /// <summary>
        /// Возвращает ноду контента, содержащую реквизиты лицезиата
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns>Нода с реквизитами лицензиата</returns>
        public override ContentNode GetHolderNode(Task task)
        {
            ContentNode holderNode = GetTaskNode(task);
            if (ContentImporter.HasChildContentNodeEndsWith(holderNode, "OrgInfo"))
            {
                return ContentImporter.GetChildContentNodeEndsWith(holderNode, "OrgInfo");
            }
            else
            {
                return ContentImporter.GetChildContentNodeEndsWith(holderNode, "IPInfo");
            }
        }

        /// <summary>
        /// Возвращает ноду контента с данными заявления
        /// Нода содержит данные лицензиата, лицензии, выполняемых работ и.т.д., причины подачи заявления
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns>Нода с данными заявления</returns>
        private ContentNode GetTaskNode(Task task)
        {
            var mzTask = MzTaskContext.Current.MzTask(task);
            var legalChoice = ContentImporter.GetContentNode(task.RootContentNode, "LegalChoice");
            
            ContentNode holderNode = null;

            if (ContentImporter.HasContentNode(legalChoice, mzTask.LicenseActionType.GetEniseyName() + "LicenseIP"))
            {
                holderNode = ContentImporter.GetContentNode(legalChoice, mzTask.LicenseActionType.GetEniseyName() + "LicenseIP");
            }
            else
            {
                holderNode = ContentImporter.GetContentNode(legalChoice, mzTask.LicenseActionType.GetEniseyName() + "LicenseUL");
            }

            return holderNode;
        }

        /// <summary>
        /// Возвращает ноду с данные лицензии
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns>данные лицензии</returns>
        public override ContentNode GetLicenseNode(Task task)
        {
            ContentNode holderNode = GetTaskNode(task);
            return ContentImporter.GetContentNode(holderNode, "LicenseInfo");
        }

        /// <summary>
        /// Возвращает набор нод с данными объектов с номенклатурой(объектов лицензии)
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns>Ноды с данными объектов с номенклатурой(объектов лицензии)</returns>
        public override IEnumerable<ContentNode> GetLincenseObjectNodes(Task task)
        {
            ContentNode holderNode = GetTaskNode(task);
            return ContentImporter.GetContentNodes(holderNode, "WorksInfo");
        }
    }
}