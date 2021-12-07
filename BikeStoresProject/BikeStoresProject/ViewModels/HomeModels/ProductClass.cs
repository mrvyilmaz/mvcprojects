using BikeStoresProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeStoresProject.ViewModels.HomeModels
{
    public class ProductClass
    {
        public ProductClass()
        {
            product = new products();
            brandList = new List<brands>();
            categoryList = new List<categories>();
        }

        public products product { get; set; }
        public List<brands> brandList { get; set; }
        public List<categories> categoryList { get; set; }
    }
}