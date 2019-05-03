using SingleCoco.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Furniture
{
    public class WarehouseDto : TableMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
