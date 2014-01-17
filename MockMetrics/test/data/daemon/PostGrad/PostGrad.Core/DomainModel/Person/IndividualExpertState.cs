using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel.Person
{
    /// <summary>
    /// Класс представляющий состояние эксперта - Физическое лицо
    /// </summary>
    [TableName("gumz.individual_expert")]
    public abstract class IndividualExpertState : IdentityDomainObject<IndividualExpertState>, IExpertState
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("expert_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Имя эксперта
        /// </summary>
        [MapField("name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Фамилия эксперта
        /// </summary>
        [MapField("surname")]
        public abstract string Surname { get; set; }

        /// <summary>
        /// Отчеcтво эксперта
        /// </summary>
        [MapField("patronymic")]
        public abstract string Patronymic { get; set; }

        /// <summary>
        /// Должность эксперта.
        /// </summary>
        [MapField("position")]
        public abstract string Position { get; set; }

        /// <summary>
        /// Наименование организации, в которой состоит эксперт
        /// </summary>
        [MapField("organization_name")]
        public abstract string OrganizationName { get; set; }

        /// <summary>
        /// Возвращает имя эксперта
        /// </summary>
        /// <returns>Имя эксперта</returns>
        public string GetName()
        {
            return string.Format("{0} {1}. {2}.", this.Surname, this.Name[0], this.Patronymic[0]);
        }

        /// <summary>
        /// Возвращает рабочие данные эксперта
        /// </summary>
        /// <returns>Рабочие данные эксперта</returns>
        public string GetWorkdata()
        {
            return string.Format("Организация: {0} \nДолжность: {1}", this.OrganizationName, this.Position);
        }

    }
}
