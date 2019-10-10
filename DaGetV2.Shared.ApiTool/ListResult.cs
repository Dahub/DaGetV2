namespace DaGetV2.Shared.ApiTool
{
    using System.Collections.Generic;

    public class ListResult<T> where T : IDto
    {
        public IEnumerable<T> Datas { get; set; }
    }
}
