using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Furniture
{
    public class AgentDto : TableMessage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Type { get; set; }
    }
}