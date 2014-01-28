using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.DataModel;
using GU.Trud.DataModel;

namespace GU.Trud.BL
{
    public class TrudDictionaryManager : AbstractDictionaryManager
    {
        public TrudDictionaryManager()
        {
            try
            {
                _dictionaries = new Dictionary<Type, List<IDomainObject>>();
                _dictionaries[typeof(ExportType)] = GetExportType();
            }
            catch (Exception ex)
            {
                throw new BLLException("DictionaryManager constructor failed", ex);
            }  
        }

        private List<IDomainObject> GetExportType()
        {
            using(var db = new TrudDbManager())
            {
                return db.ExportType.ToList<IDomainObject>();
            }
        }
    }
}
