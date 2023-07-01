using BookFinderProject.Models;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookFinderProject.Controllers;

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
        // POST: User/Login
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            try
            {
                using (var context = new BookContext())
                {
                    // Retrieve the Username and Password from the request
                    string username = collection["Username"];
                    string password = collection["Password"];
                    // Check if a user with the provided username and password exists
                    User user = context.user.Where(u => u.Username == username && u.Password == password).First();

                    if (user != null)
                    {
                        // Retrieve the user ID
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

                        // Redirect to the Details action
                        return RedirectToAction("Details", new { id = user.Id.ToString() });

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
        [Authorize]
        [HttpPost]
        public ActionResult Bookmark(string bookId)
        {
            using (var context = new BookContext())
            {
                // Retrieve the currently logged-in user's ID
                string userId = User.Identity.Name;

                // Define your SQL query
                string query = "INSERT INTO book (userId, canonical_published_work_id) VALUES (@UserId, @BookId)";

                // Define the parameters for your query
                var parameters = new[]
                {
                    new MySqlParameter("@UserId", userId),
                    new MySqlParameter("@BookId", bookId)
                };

                // Execute the SQL query
                context.Database.ExecuteSqlCommand(query, parameters);
            }
            return Json(new { success = true }); // Modify as per your bookmarking logic
        }
        [Authorize]
        [HttpPost]
        public ActionResult UnBookmark(string bookId)
        {
            using (var context = new BookContext())
            {
                // Retrieve the currently logged-in user's ID
                string userId = User.Identity.Name;

                // Define your SQL query
                string query = "DELETE FROM book WHERE canonical_published_work_id = @BookId AND userId = @UserId";

                // Define the parameters for your query
                var parameters = new[]
                {
                    new MySqlParameter("@UserId", userId),
                    new MySqlParameter("@BookId", bookId)
                };

                // Execute the SQL query
                context.Database.ExecuteSqlCommand(query, parameters);
            }
            return Json(new { success = true }); // Modify as per your bookmarking logic
        }
        [Authorize]
        public ActionResult Show(int id)
        {
            if(User.Identity.Name != id.ToString())
            {
                return new HttpUnauthorizedResult("Unauthorized Access");
            }
            List<Book> books = new List<Book>();
            var bookTitles = GetBooksTitles(id);
            BookController bookController = new BookController();
            foreach (var book in bookTitles)
            {
                books.Add(bookController.GetBooks(book).Result.First());                
            }
            return View(books);
        }

        private List<string> GetBooksTitles(int userId)
        {
            string query = "SELECT canonical_published_work_id FROM book WHERE userId = @UserId";
            var parameters = new[]
            {
                new MySqlParameter("@UserId", userId),
            };
            using (var context = new BookContext())
            {
                var books = context.Database.SqlQuery<string>(query, parameters).ToList();
                return books;
            }
        }


        // POST: User/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            if(User.Identity.IsAuthenticated && User.Identity.Name == id.ToString())
               return View(id);
            else
            {
                return View("Login");
            }
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
