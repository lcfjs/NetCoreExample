﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Areas.Applet.Controllers
{
    /// <summary>
    /// 权限
    /// </summary>
    [Area("Applet")]
    [Route("api/applet/[controller]")]
    [ApiExplorerSettings(GroupName = "Applet")]
    [ApiController]
    public class PermissionController : BaseController
    {
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetList")]
        public async Task GetList()
        { 
        
        }
    }
}