using SpecManager.BL.Model;

namespace SpecManager.BL.SpecSource
{
    public interface ISpecSource
    {
        Spec Get();

        Spec Save(Spec spec);

        string Name { get; }

        PreSaveWarning PreSave(Spec spec);
    }
}