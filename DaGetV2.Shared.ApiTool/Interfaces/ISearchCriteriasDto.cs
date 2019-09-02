namespace DaGetV2.Shared.ApiTool
{
    public interface ISearchCriteriasDto : IDto
    {
        uint Skip { get; set; }

        uint Limit { get; set; }
    }
}
