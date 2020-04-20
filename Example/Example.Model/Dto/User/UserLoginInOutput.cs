using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Model.Dto.User
{
    public class UserLoginInOutput
    {
        public string Token { get; set; }
        public string UserId { get; set; }

    }
}
