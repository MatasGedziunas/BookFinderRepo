using BookFinderProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookFinderProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {
                using(var context = new BookContext())
                {
                    // Retrieve the Username and Password from the request
                    string username = collection["Username"];
                    string password = collection["Password"];
                    // Check if a user with the provided username and password exists
                    bool userExists = context.user.Any(u => u.Username == username && u.Password == password);

                    if (userExists)
                    {
                        // User exists, perform the desired action
                        // For example, redirect to a different view or return a success message
                        return RedirectToAction("Details");
                    }
                    else
                    {
                        // User does not exist, handle the error or return an error message
                        ModelState.AddModelError("", "Invalid username or password");
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
            
        }


        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            return View(id);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // Establish a connection to the database using the DbContext
                using (var context = new BookContext())
                {
                    // Retrieve the Username and Password from the request
                    string username = collection["Username"];
                    string password = collection["Password"];

                    // Create a new user entity
                    var newUser = new User
                    {
                        Username = username,
                        Password = password
                    };

                    // Save the user to the database using Entity Framework
                    context.user.Add(newUser);

                    context.SaveChanges();
                }

                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
