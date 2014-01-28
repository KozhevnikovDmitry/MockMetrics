using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.Building.DataModel;
using GU.DataModel;

namespace GU.Building.BL
{
    public class BuildingDictionaryManager : AbstractDictionaryManager
    {
        public BuildingDictionaryManager()
        {
            try
            {
                using (var db = new BuildingDbManager())
                {
                    _dictionaries = new Dictionary<Type, List<IDomainObject>>();
                    _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("DictionaryManager constructor failed", ex);
            }
        }
    }
}
