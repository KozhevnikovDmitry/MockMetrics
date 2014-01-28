using System;
using System.Collections.Generic;

using Common.BL.Search.SearchSpecification;
using Common.Types;

using GU.DataModel;

namespace GU.BL.Search
{
    /// <summary>
    /// Класс контейнер пресетов модуля работы с заявлениями
    /// </summary>
    [Serializable]
    public class GuSearchPresetSpecContainer : SearchPresetSpecContainer
    {
        /// <summary>
        /// Класс контейнер пресетов модуля работы с заявлениями
        /// </summary>
        public GuSearchPresetSpecContainer()
        {
            PresetSpecList = new List<SearchPresetSpec>();
            this.PresetSpecList.Add(GetTaskPresetSpec());
            this.PresetSpecList.Add(GetDbUserPresetSpec());
            this.LastUpdate = new DateTime(2012, 11, 15, 15, 21, 0);
        }

        private SearchPresetSpec GetTaskPresetSpec()
        {
            var task = Task.CreateInstance();

            var preset = new PresetSpec<Task>();

            var notReady = new PresetSpecExpression(Util.GetPropertyName(() => task.CurrentState), SearchConditionType.NotEquals, typeof(Task));
            notReady.DictionarySelectedValue = (int)TaskStatusType.Done;
            var notRejected = new PresetSpecExpression(Util.GetPropertyName(() => task.CurrentState), SearchConditionType.NotEquals, typeof(Task));
            notRejected.DictionarySelectedValue = (int)TaskStatusType.Rejected;

            preset.PresetSpecDets.Add(notReady);
            preset.PresetSpecDets.Add(notRejected);

            preset.AddOrderField(typeof(Task), Util.GetPropertyName(() => task.CreateDate), OrderDirection.Descending);

            return preset;
        }

        private SearchPresetSpec GetDbUserPresetSpec()
        {
            var dbUser = DbUser.CreateInstance();

            var preset = new PresetSpec<DbUser>();

            var nameIncludes = new PresetSpecExpression(Util.GetPropertyName(() => dbUser.Name), SearchConditionType.Includes, typeof(DbUser));

            preset.PresetSpecDets.Add(nameIncludes);

            preset.AddOrderField(typeof(DbUser), Util.GetPropertyName(() => dbUser.Name), OrderDirection.Ascending);

            return preset;
        }
    }
}
