namespace DaGetV2.Shared.ApiTool
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DtoExtensions
    {
        public static ListResult<T> ToListResult<T>(this IEnumerable<T> values) where T : IDto
        {
            return new ListResult<T>()
            {
                Datas = values.Select(v => (T)v)
            };
        }

        public static SearchResult<T> ToSearchResult<T>(this IEnumerable<IDto> values, string currentUri, int count) where T : IDto
        {
            var result = new SearchResult<T>()
            {
                Count = count,
                Datas = values.Select(v => (T)v),
                Limit = 0,
                Skip = 0
            };
            result.Links.This = currentUri;
            return result;
        }

        public static SearchResult<T> ToSearchResult<T>(this IEnumerable<IDto> values, string currentUri, int count, ISearchCriteriasDto criterias) where T : IDto
        {
            var result = new SearchResult<T>()
            {
                Count = count,
                Datas = values.Select(v => (T)v),
                Limit = criterias.Limit,
                Skip = criterias.Skip
            };
            result.Links.This = currentUri;

            var pattern = $"skip={criterias.Skip}&limit={criterias.Limit}";
            if (count > criterias.Skip + criterias.Limit)
            {
                var t = $"skip={criterias.Skip + criterias.Limit}&limit={criterias.Limit}";
                result.Links.Next = currentUri.Replace(pattern, t);
            }

            if (criterias.Skip > 0)
            {
                var val = (int)criterias.Skip - (int)criterias.Limit;
                if (val < 0)
                {
                    val = 0;
                }

                var t = $"skip={val}&limit={criterias.Limit}";
                result.Links.Prev = currentUri.Replace(pattern, t);
            }

            return result;
        }
    }
}
