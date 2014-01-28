using System;
using Autofac;
using Common.BL;
using Common.BL.DictionaryManagement;

namespace GU.MZ.BL
{
    [Obsolete]
    internal class GumzCore : ICore
    {
        #region Singleton
        
        private static readonly Lazy<GumzCore> Lazy = new Lazy<GumzCore>(() => new GumzCore());

        public static GumzCore Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        private GumzCore()
        {
            UserPreferences = new GumzUserPreferences();
        }

        #endregion
        
        public IDictionaryManager DictionaryManager { get; private set; }

        public IUserPreferences UserPreferences { get; private set; }

        public IDomainLogicContainer MzBlFactory { get; private set; }

        public void Initialize(IDomainLogicContainer blFactory)
        {
            MzBlFactory = blFactory;
            DictionaryManager = blFactory.IocContainer.Resolve<IDictionaryManager>();
        }

        internal Exception InitializationException { get; private set; }
    }
}
