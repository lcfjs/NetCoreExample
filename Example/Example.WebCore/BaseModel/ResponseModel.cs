using System;
using System.Collections.Generic;
using System.Text;

namespace Example.WebCore.BaseModel
{
    /// <summary>
    /// 响应实体
    /// </summary>
    public class ResponseModel<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    /// <summary>
    /// 响应实体
    /// </summary>
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
