using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Systerm
{
    public class AccountQuery : TableMessage
    {
        public string Name { get; set; }

    }
}
