using System.Linq;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.MZ.CERT.BL
{
    /// <summary>
    /// Класс, представляющий менеджер базы данных для схем gu и gu_mz_cert.
    /// </summary>
    public class CertDbManager : DomainDbManager
    {
        /// <summary>
        /// Класс, представляющий менеджер базы данных для схем gu и gumz.
        /// </summary>
        public CertDbManager()
            : this(DefaultConfiguration)
        {
        }

        /// <summary>
        ///  Класс, представляющий менеджер базы данных для схем gu и gumz.
        /// </summary>
        /// <param name="configurationString">Наименование конфигурации подключения к базе данных</param>
        public CertDbManager(string configurationString)
            : base(configurationString)
        {
            foreach (var p in GU.BL.GuDbManager.FilterPredicates)
                _filterPredicates.Add(p.Key, p.Value);
        }
        
        /// <summary>
        /// Возвращает запрос по таблице CommonData схемы gumz.
        /// </summary>
        protected override void RetrieveCommonData(IPersistentObject obj)
        {
            var cd = (from c in this.GetTable<DataModel.CommonData>()
                      where c.KeyValue == obj.GetKeyValue() && c.Entity == obj.GetType().Name
                      select c).OrderByDescending(x => x.Stamp).Take(1).ToList().FirstOrNull();

            if (cd != null)
            {
                cd.ConfigurationString = this.ConfigurationString;
            }
            obj.CommonData = cd;
        }

    }
}
