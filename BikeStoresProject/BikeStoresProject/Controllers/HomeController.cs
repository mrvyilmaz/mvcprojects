using BikeStoresProject.Models;
using BikeStoresProject.ViewModels;
using BikeStoresProject.ViewModels.HomeModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikeStoresProject.Controllers
{
    public class HomeController : Controller
    {
        #region Customer İşlemleri

        public ActionResult CustomerList()
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            List<customers> model = new List<customers>();

            try
            {
                // LINQ Method syntax:
                model = ent.customers.OrderBy(q => q.first_name).ThenBy(q => q.last_name).ToList();

                //select * from sales.customers where city = 'New York'
                //model = ent.customers.Where(q => q.city.Equals("New York")).ToList();

                //select * from sales.customers where first_name like 'Ab%' 
                //model = ent.customers.Where(q => q.first_name.Substring(0, 2).Equals("Ab")).ToList();

                //LINQ Query syntax:
                var customers = (from s in ent.customers
                                 where s.first_name == "Abby"
                                 select s).ToList();

                //Native SQL:
                var customerList = ent.Database.SqlQuery<customers>("select * from sales.customers order by first_name").ToList();

                var customer = ent.customers.SqlQuery("select * from sales.customers " +
                    "where first_name = @name", new SqlParameter("@name", "Abby")).FirstOrDefault();

                customer = ent.customers.SqlQuery("select * from sales.customers where first_name='Abby'").FirstOrDefault();

            }
            catch (Exception) { throw; }

            return View(model);

        }

        public ActionResult CustomerAdd(int? id = 0)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            customers model = new customers();

            try
            {
                if (id > 0)
                {
                    //Update
                    model = ent.customers.Find(id);

                }
            }
            catch (Exception) { throw; }

            return View(model);
        }

        [HttpPost]
        public ActionResult CustomerAdd(customers model)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            try
            {
                if (model.customer_id > 0)
                {   //update

                    customers customer = ent.customers.Find(model.customer_id);

                    customer.first_name = model.first_name;
                    customer.last_name = model.last_name;
                    customer.phone = model.phone;
                    customer.email = model.email;
                    customer.street = model.street;
                    customer.city = model.city;
                    customer.state = model.state;
                    customer.zip_code = model.zip_code;

                    ent.SaveChanges();

                }
                else
                {   //insert

                    ent.customers.Add(model);
                    ent.SaveChanges();
                }
            }
            catch (Exception) { throw; }

            return RedirectToAction("CustomerList");
        }

        public ActionResult CustomerDelete(int? id = 0)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            customers model = new customers();
            try
            {
                model = ent.customers.Find(id);
                ent.customers.Remove(model);
                ent.SaveChanges();

            }
            catch (Exception) { throw; }

            return RedirectToAction("CustomerList");
        }

        #endregion

        #region Product İşlemleri

        public ActionResult ProductList()
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            List<products> model = new List<products>();

            try
            {
                //Tablolara arası entity ile geçiş
                model = ent.products.OrderBy(q => q.product_name).ToList();
            }
            catch (Exception) { throw; }

            return View(model);
        }

        public ActionResult ProductAdd(int? id = 0)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            ProductClass model = new ProductClass();

            try
            {
                model.brandList = ent.brands.ToList();
                model.categoryList = ent.categories.ToList();

                if (id > 0)
                {   //update
                    model.product = ent.products.Find(id); ;
                }

            }
            catch (Exception) { throw; }

            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAdd(products model)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            try
            {
                if (model.product_id > 0)
                {   //update

                    //products product = ent.products.Find(model.product_id);

                    //product.product_name = model.product_name;
                    //product.model_year = model.model_year;
                    //product.list_price = model.list_price;
                    //product.brand_id = model.brand_id;
                    //product.category_id = model.category_id;

                    //ent.SaveChanges();

                    ent.Database.ExecuteSqlCommand("update production.products set product_name = @productname where product_id = @id", new SqlParameter("@productname", model.product_name), new SqlParameter("@id", model.product_id));

                }
                else
                {   //insert

                    //ent.products.Add(model);
                    //ent.SaveChanges();

                    ent.Database.ExecuteSqlCommand(@"insert into production.products values(@productname, @brandid, @categoryid, @modelyear, @listprice)", new SqlParameter("@productname", model.product_name), new SqlParameter("@brandid", model.brand_id), new SqlParameter("@categoryid", model.category_id), new SqlParameter("@modelyear", model.model_year), new SqlParameter("@listprice", model.list_price));

                }
            }
            catch (Exception) { throw; }

            return RedirectToAction("ProductList");
        }

        public ActionResult ProductDelete(int? id = 0)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            products model = new products();
            try
            {
                ent.Database.ExecuteSqlCommand("delete from production.products where product_id = @id", new SqlParameter("@id", id));

                //model = ent.products.Find(id);
                //ent.products.Remove(model);
                //ent.SaveChanges();

            }
            catch (Exception) { throw; }

            return RedirectToAction("ProductList");
        }

        #endregion

        #region Order İşlemleri

        public ActionResult OrderList()
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            List<OrderClass> model = new List<OrderClass>();
            try
            {
                model = ent.Database.SqlQuery<OrderClass>(@"select c.first_name, c.last_name, p.product_name, oi.list_price, o.order_date, o.shipped_date from sales.orders o
                                                inner join sales.customers c on c.customer_id = o.customer_id
                                                inner join sales.order_items oi on oi.order_id = o.order_id
                                                inner join production.products p on p.product_id = oi.product_id
                                                order by c.first_name").ToList();
            }
            catch (Exception) { throw; }

            return View(model);
        }

        public ActionResult OrderAdd(int? id = 0)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            OrderClass model = new OrderClass();

            try
            {
                model.productList = ent.products.OrderBy(q => q.product_name).ToList();
                model.customerList = ent.customers.OrderBy(q => q.first_name).ThenBy(q => q.last_name).ToList();
                model.staffList = ent.staffs.OrderBy(q => q.first_name).ThenBy(q => q.last_name).ToList();

                if (id > 0)
                {   //update
                }

            }
            catch (Exception) { throw; }

            return View(model);
        }

        [HttpPost]
        public ActionResult OrderAdd(OrderClass.OrderItems model)
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            try
            {
                if (model.id > 0)
                {   //update

                }
                else
                {   //insert
                    orders order = new orders();
                    order.customer_id = model.customer_id;
                    order.order_status = 4;
                    order.order_date = DateTime.Now;
                    order.required_date = DateTime.Now;
                    order.staff_id = model.staff_id;
                    var staf = ent.staffs.Where(q => q.staff_id == model.staff_id).FirstOrDefault();
                    order.store_id = staf.store_id;
                    ent.orders.Add(order);
                    ent.SaveChanges();

                    order_items order_item = new order_items();
                    order_item.order_id = order.order_id;
                    //var orderItems = ent.order_items.Where(q => q.order_id == order.order_id).OrderByDescending(q => q.item_id).FirstOrDefault();
                    order_item.item_id = 1;
                    order_item.product_id = model.product_id;
                    order_item.quantity = model.quantity;
                    order_item.list_price = model.list_price;
                    order_item.discount = model.discount;
                    ent.order_items.Add(order_item);
                    ent.SaveChanges();

                }

            }
            catch (Exception) { throw; }

            return View();
        }

        #endregion

    }
}