using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.Archive.DataModel;
using GU.DataModel;

namespace GU.Archive.BL
{
    public class ArchiveDictionaryManager : AbstractDictionaryManager
    {
        public ArchiveDictionaryManager()
        {
            try
            {
                using (var db = new ArchiveDbManager())
                {
                    _dictionaries = new Dictionary<Type, List<IDomainObject>>();
                    _dictionaries[typeof (Author)] = GetDomainList<Author>(db);
                    _dictionaries[typeof (Organization)] = GetOrganizationList(db);
                    _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
                    _enumDictionaries[typeof (Sex)] = GetSexList();
                    _enumDictionaries[typeof(PostType)] = GetPostTypeList();
                    _enumDictionaries[typeof(DeliveryType)] = GetDeliveryTypeList();
                    _enumDictionaries[typeof(RequestType)] = GetRequestTypeList();
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("DictionaryManager constructor failed", ex);
            }
        }

        #region Data retrieving

        private List<IDomainObject> GetDomainList<T>(IDomainDbManager dbManager) where T : class, IDomainObject
        {
            return dbManager.GetDomainTable<T>().ToList<IDomainObject>();
        }

        private  List<IDomainObject> GetOrganizationList(IDomainDbManager dbManager)
        {
            return dbManager.GetDomainTable<Organization>().OrderBy(x => x.ShortName).ToList<IDomainObject>();
        }

        private Dictionary<int, string> GetSexList()
        {
            Dictionary<int, string> requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "Мужской";
            requestSexList[2] = "Женский";
            return requestSexList;
        }

        private Dictionary<int, string> GetPostTypeList()
        {
            Dictionary<int, string> requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "Входящие";
            requestSexList[2] = "Исходящие";
            requestSexList[3] = "Напоминания";
            requestSexList[4] = "Приказы";
            requestSexList[5] = "Командировки";
            return requestSexList;
        }

        private Dictionary<int, string> GetDeliveryTypeList()
        {
            Dictionary<int, string> requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "письмо";
            requestSexList[2] = "факс";
            requestSexList[3] = "телеграмма";
            requestSexList[4] = "электронная почта";
            requestSexList[6] = "лично";
            requestSexList[7] = "гос.услуги";
            return requestSexList;
        }

        private Dictionary<int, string> GetRequestTypeList()
        {
            Dictionary<int, string> requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "социально-правовые";
            requestSexList[2] = "тематические";
            requestSexList[3] = "генеалогические";
            requestSexList[4] = "иностранные граждане";
            requestSexList[5] = "жалобы";
            requestSexList[6] = "прочие";
            return requestSexList;
        }

        #endregion
    }
}
