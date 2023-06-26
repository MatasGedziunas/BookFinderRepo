using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookFinderProject.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace BookFinderProject.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public async Task<List<Book>> GetBooks(string title = "", string author = "", string series = "", string bookType = "", string categories = "", int lexileMin = 0, int lexileMax = 0, int resultsPerPage = 0, int page = 0)
        {
            // Rest of the code

            var apiUrl = "https://book-finder1.p.rapidapi.com/api/search?";
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(title))
            {
                queryParams.Add($"title={Uri.EscapeDataString(title)}");
            }

            if (!string.IsNullOrEmpty(author))
            {
                queryParams.Add($"author={Uri.EscapeDataString(author)}");
            }

            if (!string.IsNullOrEmpty(series))
            {
                queryParams.Add($"series={Uri.EscapeDataString(series)}");
            }

            if (!string.IsNullOrEmpty(bookType))
            {
                queryParams.Add($"book_type={Uri.EscapeDataString(bookType)}");
            }

            if (!string.IsNullOrEmpty(categories))
            {
                queryParams.Add($"categories={Uri.EscapeDataString(categories)}");
            }

            if (lexileMin > 0)
            {
                queryParams.Add($"lexile_min={lexileMin}");
            }

            if (lexileMax > 0)
            {
                queryParams.Add($"lexile_max={lexileMax}");
            }

            if (resultsPerPage > 0)
            {
                queryParams.Add($"results_per_page={resultsPerPage}");
            }

            if (page > 0)
            {
                queryParams.Add($"page={page}");
            }

            apiUrl += string.Join("&", queryParams);

            var client = new RestClient(apiUrl);
            var request = new RestRequest();
            request.AddHeader("X-RapidAPI-Key", "f21abfca76mshbf5fb8c9459cda1p1cb357jsn360e0521dbae");
            request.AddHeader("X-RapidAPI-Host", "book-finder1.p.rapidapi.com");

            RestResponse response = await client.ExecuteAsync(request);
            Debug.WriteLine(response.StatusDescription);
            Debug.WriteLine(response.StatusCode);
            Debug.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                List<Book> books = apiResponse.Results;                
                Debug.WriteLine(apiResponse.TotalPages);
                return books;
            }

            return new List<Book>();
        }
        public async Task<ActionResult> Show()
        {
            string title = Request.Form["title"];
            string author = Request.Form["author"];
            string series = Request.Form["series"];
            int lexileMin;
            int lexileMax;

            if (int.TryParse(Request.Form["min_lexile"], out int minLexileValue))
            {
                lexileMin = minLexileValue;
            }
            else
            {
                lexileMin = 0;
            }

            if (int.TryParse(Request.Form["max_lexile"], out int maxLexileValue))
            {
                lexileMax = maxLexileValue;
            }
            else
            {
                lexileMax = 0;
            }
            string bookType = Request.Form["type"];
            string categories = Request.Form.Get("category"); // Retrieve as comma-separated string

            // Call the GetBooks method and pass the form data
            List<Book> books = await GetBooks(title, author, series, bookType, categories, lexileMin, lexileMax);

            // Process the books and return the appropriate view
            return View(books);
        }

    }
}