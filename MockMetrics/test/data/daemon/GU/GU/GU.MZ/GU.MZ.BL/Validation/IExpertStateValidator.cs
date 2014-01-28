using Common.BL.Validation;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.Validation
{
    public interface IExpertStateValidator : IDomainValidator<IExpertState>
    {
        ExpertStateType ExpertStateType { get; }
    }
}