using System.Linq;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.BL;
using CommonData = GU.MZ.DataModel.CommonData;

namespace GU.MZ.BL
{
    /// <summary>
    /// Класс, представляющий менеджер базы данных для схем gu и gumz.
    /// </summary>
    public class GumzDbManager : DomainDbManager
    {
        /// <summary>
        /// Класс, представляющий менеджер базы данных для схем gu и gumz.
        /// </summary>
        public GumzDbManager()
            : this(DefaultConfiguration)
        {
        }

        /// <summary>
        ///  Класс, представляющий менеджер базы данных для схем gu и gumz.
        /// </summary>
        /// <param name="configurationString">Наименование конфигурации подключения к базе данных</param>
        public GumzDbManager(string configurationString)
            : base(configurationString)
        {
            foreach (var p in GuDbManager.FilterPredicates)
                _filterPredicates.Add(p.Key, p.Value);
        }
        
        /// <summary>
        /// Возвращает запрос по таблице CommonData схемы gumz.
        /// </summary>
        protected override void RetrieveCommonData(IPersistentObject obj)
        {
            var cd = (from c in GetTable<CommonData>()
                      where c.KeyValue == obj.GetKeyValue() && c.Entity == obj.GetType().Name
                      select c).OrderByDescending(x => x.Stamp).Take(1).ToList().FirstOrNull();

            if (cd != null)
            {
                cd.ConfigurationString = ConfigurationString;
            }
            obj.CommonData = cd;
        }

    }
}
