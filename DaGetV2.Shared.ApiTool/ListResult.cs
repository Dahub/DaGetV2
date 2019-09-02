using System.Collections.Generic;

namespace DaGetV2.Shared.ApiTool
{
    public class ListResult<T> where T : IDto
    {
        public IEnumerable<T> Datas { get; set; }
    }
}
