using System;
using System.Collections.Generic;
using System.Text;

namespace Example.UnitOfWork.Entity
{
    public interface IPageList<T>
    {
        int PageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        List<T> Items { get; }
    }
}
