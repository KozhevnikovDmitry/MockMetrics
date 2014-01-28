using Common.DA;
using Common.DA.Interface;

using GU.MZ.DS.DataModel;

namespace GU.MZ.DS.BL
{
    public class DsDomainObjectInitializer : AbstractDomainObjectInitializer
    {
        public DsDomainObjectInitializer(string userName)
            : base(userName)
        {
        }

        #region Overrides of AbstractDomainObjectInitializer

        /// <summary>
        /// Инициализиует доменный объект
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        public override void InitializeObject<T>(T obj)
        {
            
        }

        protected override ICommonData CreateCommonData()
        {
            return new CommonData();
        }

        #endregion
    }
}
