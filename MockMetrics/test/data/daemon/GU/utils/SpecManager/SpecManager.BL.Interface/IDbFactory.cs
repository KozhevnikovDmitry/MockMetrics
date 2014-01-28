namespace SpecManager.BL.Interface
{
    public interface IDbFactory
    {
        string ConnectionString { get; set; }

        IDomainDbManager GetDbManager();
    }
}
