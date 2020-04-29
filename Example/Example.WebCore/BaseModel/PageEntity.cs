using System;
using System.Collections.Generic;
using System.Text;

namespace Example.WebCore.BaseModel
{
    public class PageEntity
    {
        public PageEntity()
        {
            this.PageIndex = 0;
            this.PageSize = 20;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
