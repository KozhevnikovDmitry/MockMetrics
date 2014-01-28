using System;
using System.Collections.Generic;
using System.Linq;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    public class Linkager : ILinkager
    {
        private readonly Func<IDomainDbManager> _getDb;
        private readonly IEnumerable<ILinkagerAddIn> _addIns;

        public Linkager(Func<IDomainDbManager> getDb, IEnumerable<ILinkagerAddIn> addIns)
        {
            _getDb = getDb;
            _addIns = addIns;
        }


        public IDossierFileLinkWrapper Linkage(DossierFile dossierFile)
        {
            using (var db = _getDb())
            {
                var wrapper = new DossierFileLinkWrapper(dossierFile);
                foreach (var linkagerAddIn in _addIns.OrderBy(t => t.SortOrder))
                {
                    linkagerAddIn.Linkage(wrapper, db);
                }

                return wrapper;
            }
        }
    }
}