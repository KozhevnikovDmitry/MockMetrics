using System.ComponentModel;

namespace GU.DataModel
{
    [DefaultValue(DbUserStateType.Active)]
    public enum DbUserStateType
    {
        Active = 1,
        Disabled = 2,
        Deleted = 3
    }
}
