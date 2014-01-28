using System.ComponentModel;

namespace GU.MZ.DataModel.Person
{
    /// <summary>
    /// Перечисление типов состояния эксперта
    /// </summary>
    public enum ExpertStateType
    {
        [Description("Физическое лицо")]
        Individual = 1,

        [Description("Юридическое лицо")]
        Juridical = 2
    }
}