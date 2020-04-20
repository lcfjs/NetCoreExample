using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.WebCore.BaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.API
{
    public class BaseController : ControllerBase
    {
        #region Response

        protected ResponseModel Success(string message = "操作成功")
        {
            return new ResponseModel
            {
                Code = 0,
                Message = message,
            };
        }

        protected ResponseModel<T> Success<T>(T data, string message = "操作成功")
        {
            return new ResponseModel<T>
            {
                Code = 0,
                Message = message,
                Data = data,
            };
        }

        protected ResponseModel Error(string message = "操作失败")
        {
            return new ResponseModel
            {
                Code = 0,
                Message = message,
            };
        }

        protected ResponseModel<T> Error<T>(T data, string message = "操作失败")
        {
            return new ResponseModel<T>
            {
                Code = 0,
                Message = message,
                Data = data,
            };
        }

        #endregion
    }
}
