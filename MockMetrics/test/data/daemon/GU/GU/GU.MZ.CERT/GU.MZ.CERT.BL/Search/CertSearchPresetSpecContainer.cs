using System;
using System.Collections.Generic;

using Common.BL.Search.SearchSpecification;

namespace GU.MZ.CERT.BL.Search
{
    /// <summary>
    /// Класс контейнер пресетов предметной области Аттестация
    /// </summary>
    [Serializable]
    public class CertSearchPresetSpecContainer : SearchPresetSpecContainer
    {
        /// <summary>
        /// Класс контейнер пресетов предметной области Аттестация
        /// </summary>
        public CertSearchPresetSpecContainer()
        {
            this.PresetSpecList = new List<SearchPresetSpec>();
            this.LastUpdate = new DateTime(2012, 11, 08, 13, 11, 0);
        }
    }
}