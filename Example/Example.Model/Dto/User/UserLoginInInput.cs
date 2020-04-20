using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Model.Dto.User
{
    public class UserLoginInInput
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Display(Name = "登录账号")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Account { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "登录密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string PassWord { get; set; }
    }
}
