using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example.UnitOfWork.Entity
{
    public class PageList<T> : IPageList<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; }

        /// <summary>
        /// 需要对传入的集合进行分页操作
        /// </summary>
        /// <param name="source">分页前的源集合</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }

            if (source is IQueryable<T> querable)
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                TotalCount = querable.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                Items = querable.Skip((PageIndex) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                PageIndex = pageIndex;
                PageSize = pageSize;
                TotalCount = source.Count();
                TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

                Items = source.Skip((PageIndex) * PageSize).Take(PageSize).ToList();
            }
        }

        /// <summary>
        /// 直接将List转为分页
        /// </summary>
        /// <param name="list">分页后的最终集合</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        public PageList(List<T> list, int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = list;
        }

        public PageList()
        {

        }
    }
}
