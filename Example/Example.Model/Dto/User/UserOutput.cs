using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Model.Dto.User
{
    public class UserOutput
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public long Sort { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }
    }
}
