using System;
using System.Collections.Generic;
using System.Linq;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.MzOrder;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel
{
    public class AddHolderAddressForOrderVm : NotificationObject
    {
        private readonly Func<IDomainDbManager> _getDb;

        public AddHolderAddressForOrderVm(Func<IDomainDbManager> getDb)
        {
            _getDb = getDb;
        }

        #region Binding Properties

        public string Address { get; set; }

        public List<string> AddressList { get; set; } 

        #endregion

        public void Initialize(IOrder order)
        {
            using (var db = _getDb())
            {
                int holderId =
                    db.GetDomainTable<DossierFileScenarioStep>()
                        .Where(t => t.Id == order.Id)
                        .Select(t => t.DossierFile.LicenseDossier.LicenseHolder.Id)
                        .Single();

                AddressList =
                    db.GetDomainTable<HolderRequisites>()
                        .Where(t => t.LicenseHolderId == holderId)
                        .Select(t => t.Address)
                        .ToList()
                        .Select(t => t.ToLongString())
                        .Distinct()
                        .ToList();

            }

            Address = AddressList.First();
        }
    }
}
