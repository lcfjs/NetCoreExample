using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Example.WebCore.BaseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Areas.Applet.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Area("Applet")]
    [Route("api/applet/[controller]")]
    [ApiExplorerSettings(GroupName = "Applet")]
    [ApiController]
    public class UserController : BaseController
    {
        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="id">用户Id</param>
        [HttpGet]
        [Route("GetById")]
        public async Task<ResponseModel<string>> GetById([Required(ErrorMessage = "Id不能为空")]string id)
        {
            return Success<string>($"test{id}");
        }
    }
}