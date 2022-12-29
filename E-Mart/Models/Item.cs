using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mart.Models
{
    public class Item
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }
    }
}