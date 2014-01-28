using System;
using System.Collections.Generic;

using Common.BL.Search.SearchSpecification;

namespace GU.MZ.DS.BL.Search
{
    /// <summary>
    /// ����� ��������� �������� ���������� ������� ������������� �����������
    /// </summary>
    [Serializable]
    public class DsSearchPresetSpecContainer : SearchPresetSpecContainer
    {
        /// <summary>
        /// ����� ��������� �������� ���������� ������� ������������� �����������
        /// </summary>
        public DsSearchPresetSpecContainer()
        {
            this.PresetSpecList = new List<SearchPresetSpec>();
            this.LastUpdate = new DateTime(2012, 11, 08, 13, 11, 0);
        }
    }
}