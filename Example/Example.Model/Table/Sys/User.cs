using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Model.Table.Sys
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }
}
