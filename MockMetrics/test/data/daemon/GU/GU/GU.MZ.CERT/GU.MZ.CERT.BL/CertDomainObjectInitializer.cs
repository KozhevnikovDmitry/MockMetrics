using Common.DA;
using Common.DA.Interface;

using GU.MZ.CERT.DataModel;

namespace GU.MZ.CERT.BL
{
    public class CertDomainObjectInitializer : AbstractDomainObjectInitializer
    {
        public CertDomainObjectInitializer(string userName)
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
            throw new System.NotImplementedException();
        }

        protected override ICommonData CreateCommonData()
        {
            return new CommonData();
        }

        #endregion
    }
}
