using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Example.WebCore.BaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("api/mobile/[controller]")]
    [ApiExplorerSettings(GroupName = "Mobile")]
    [ApiController]
    public class UserController : BaseController
    {
        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<ResponseModel<string>> GetById([Required(ErrorMessage = "Id不能为空")]string id)
        {
            return Success<string>($"Mobile - {id}");
        }
    }
}