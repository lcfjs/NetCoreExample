using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Model
{
    public class BaseEntity
    {
        public string Id { get; set; }

        public bool IsDelete { get; set; }

        public long Sort { get; set; }

        public DateTime CreateTime { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string UpdateUserId { get; set; }

        public string UpdateUserName { get; set; }
    }
}
