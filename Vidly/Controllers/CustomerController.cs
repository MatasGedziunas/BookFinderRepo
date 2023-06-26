using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>
            {
                new Customer{Name="John"},
                new Customer{Name="Max"}
            };
            return View(customers);
        }
        [Route("Customer/Details/{id:max(1):min(0)}")]
        public ActionResult Details(int id)
        {
            List<Customer> customers = new List<Customer>
            {
                new Customer{Name="John"},
                new Customer{Name="Max"}
            };           
            return View(customers[id]);
        }
    }
}