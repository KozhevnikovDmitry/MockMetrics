using Common.BL.DataMapping;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    public interface IExpertStateDataMapper : IDomainDataMapper<IExpertState>
    {
        ExpertStateType ExpertStateType { get; }
    }
}