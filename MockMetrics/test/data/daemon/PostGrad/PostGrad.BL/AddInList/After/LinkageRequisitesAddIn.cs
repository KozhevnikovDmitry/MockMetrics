using System.Collections.Generic;
using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.BL.AddInList.After
{
    public class LinkageRequisitesAddIn : ILinkagerAddIn
    {
        private readonly ITaskParser _taskTaskParser;

        public LinkageRequisitesAddIn(ITaskParser taskTaskParser)
        {
            _taskTaskParser = taskTaskParser;
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
                        = _taskTaskParser.ParseHolder(fileLink.DossierFile.Task).RequisitesList.Single();
                }

                fileLink.AvailableRequisites[RequisitesOrigin.FromRegistr] =
                    fileLink.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
            }

            fileLink.SelectedRequisites = fileLink.AvailableRequisites.First().Value;
        }
    }
}