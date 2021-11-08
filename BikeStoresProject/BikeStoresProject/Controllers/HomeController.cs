using BikeStoresProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikeStoresProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BikeStoresEntities ent = new BikeStoresEntities();
            List<customers> model = new List<customers>();

            try
            {
                model = ent.customers.OrderBy(q => q.first_name).ThenBy(q => q.last_name).ToList();

                //select * from sales.customers where city = 'New York'
                //model = ent.customers.Where(q => q.city.Equals("New York")).ToList();

                //select * from sales.customers where first_name like 'Ab%' 
                //model = ent.customers.Where(q => q.first_name.Substring(0, 2).Equals("Ab")).ToList();

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

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}