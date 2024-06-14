using CST.Common.Exceptions;
using CST.Common.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Extensions
{
    public static class PaginationExtension
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, PaginationParameters paginationParameters)
        {
           if (paginationParameters.Limit < 0 || paginationParameters.Offset < 0) throw new BadRequestException(nameof(paginationParameters));

           return source.Skip(paginationParameters.Offset).Take(paginationParameters.Limit);
        }
    }
}
