namespace PostGrad.Core.BL
{
    public interface ILifetimeScope
    {
        T Resolve<T>();
    }
}
