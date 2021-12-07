using BikeStoresProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeStoresProject.ViewModels.HomeModels
{
    public class OrderClass
    {
        public List<products> productList { get; set; }
        public List<customers> customerList { get; set; }
        public List<staffs> staffList { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string product_name { get; set; }
        public int order_id { get; set; }
        public decimal list_price { get; set; }
        public DateTime order_date { get; set; }
        public Nullable<DateTime> shipped_date { get; set; }

        public class OrderItems
        {
            public int id { get; set; }
            public int customer_id { get; set; }
            public int product_id { get; set; }            
            public int staff_id { get; set; }
            public decimal list_price { get; set; }
            public decimal discount { get; set; }
            public int quantity { get; set; }

        }
    }
}