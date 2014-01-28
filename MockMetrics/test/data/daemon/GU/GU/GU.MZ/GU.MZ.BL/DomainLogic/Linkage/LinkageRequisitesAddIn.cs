using System.Collections.Generic;
using System.Linq;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class LinkageRequisitesAddIn : ILinkagerAddIn
    {
        private readonly IParser _taskParser;

        public LinkageRequisitesAddIn(IParser taskParser)
        {
            _taskParser = taskParser;
        }

        public int SortOrder {
            get
            {
                return 1;
            }
        }

        public void Linkage(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            fileLink.AvailableRequisites = new Dictionary<RequisitesOrigin, HolderRequisites>();
            if (fileLink.LicenseHolder.Id == 0)
            {
                fileLink.AvailableRequisites[RequisitesOrigin.FromTask] =
                    fileLink.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Full)
                {
                    fileLink.AvailableRequisites[RequisitesOrigin.FromTask]
                        = _taskParser.ParseHolder(fileLink.DossierFile.Task).RequisitesList.Single();
                }

                fileLink.AvailableRequisites[RequisitesOrigin.FromRegistr] =
                    fileLink.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
            }

            fileLink.SelectedRequisites = fileLink.AvailableRequisites.First().Value;
        }
    }
}