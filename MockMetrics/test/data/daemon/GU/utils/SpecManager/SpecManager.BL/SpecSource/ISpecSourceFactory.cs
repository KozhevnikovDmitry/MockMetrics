namespace SpecManager.BL.SpecSource
{
    public interface ISpecSourceFactory
    {
        ISpecSource GetSpecSource(string path);

        ISpecSource GetSpecSource(int id, string connectionString);

        ISpecSource GetSpecSource(string uri, string connectionString);
    }
}
