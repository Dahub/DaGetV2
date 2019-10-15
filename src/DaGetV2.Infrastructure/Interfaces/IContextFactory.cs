namespace DaGetV2.Infrastructure.Interfaces
{
    using ApplicationCore.Interfaces;

    public interface IContextFactory
    {
        IContext CreateContext();
    }
}
