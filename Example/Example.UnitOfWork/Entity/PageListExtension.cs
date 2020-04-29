using Example.UnitOfWork.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example.UnitOfWork
{
    public static class PageListExtension
    {
        public static IPageList<T> ToPageList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PageList<T>(source, pageIndex, pageSize);
        }
        public static async Task<IPageList<T>> ToPageListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PageList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = items,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }

        public static async Task<IPageList<TResult>> ToPageListAsync<TSource, TResult>(this IQueryable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> fun, int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((pageIndex) * pageSize)
                                    .Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PageList<TResult>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = fun(items).ToList(),
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            return pagedList;
        }
    }
}
