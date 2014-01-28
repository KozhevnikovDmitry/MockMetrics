using System.ComponentModel;

using Common.DA.Interface;

namespace GU.MZ.DataModel.Person
{
    /// <summary>
    /// Интерфейс классов представляющих персональное состояние Эксперта - юр или физ лицо
    /// </summary>
    public interface IExpertState : IPersistentObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Возвращает имя эксперта
        /// </summary>
        /// <returns>Имя эксперта</returns>
        string GetName();

        /// <summary>
        /// Возвращает рабочие данные эксперта
        /// </summary>
        /// <returns>Рабочие данные эксперта</returns>
        string GetWorkdata();
    }
}
