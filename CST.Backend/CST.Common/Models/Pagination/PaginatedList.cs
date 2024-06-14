using System.Collections;

namespace CST.Common.Models.Pagination
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Entities { get; set; }

        public int TotalCount { get; set; }

        public PaginatedList(IEnumerable<T> entities, int totalCount)
        {
            Entities = entities;
            TotalCount= totalCount;
        }
    }
}
