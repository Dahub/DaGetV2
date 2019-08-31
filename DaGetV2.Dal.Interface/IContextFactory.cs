namespace DaGetV2.Dal.Interface
{
    public interface IContextFactory
    {
        string ConnexionString { get; set; }

        IContext CreateContext();
    }
}
