﻿using System;
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
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.IO;
using Microsoft.AspNet.Identity;

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

            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsetings.json")
                    .Build();

            string rapidApiKey = configuration["X-RapidAPI-Key"];
            string rapidApiHost = configuration["X-RapidAPI-Host"];

            // Use the API keys in your requests
            request.AddHeader("X-RapidAPI-Key", rapidApiKey);
            request.AddHeader("X-RapidAPI-Host", rapidApiHost);
            // Use the API keys in your requests
            client.AddDefaultHeader("X-RapidAPI-Key", rapidApiKey);
            client.AddDefaultHeader("X-RapidAPI-Host", rapidApiHost);
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
    string title = "";
    string author = "";
    string series = "";
    int lexileMin = 0;
    int lexileMax = 0;

    if (TempData["FormData"] != null)
    {
        // Retrieve the preserved form data from TempData
        var formData = TempData["FormData"] as NameValueCollection;

        // Retrieve the individual form values
        title = formData["title"];
        author = formData["author"];
        series = formData["series"];
    }
    else
    {
        title = Request.Form["title"];
        author = Request.Form["author"];
        series = Request.Form["series"];
    }

    if (int.TryParse(Request.Form["min_lexile"], out int minLexileValue))
    {
        lexileMin = minLexileValue;
    }

    if (int.TryParse(Request.Form["max_lexile"], out int maxLexileValue))
    {
        lexileMax = maxLexileValue;
    }

    string bookType = Request.Form["type"];
    string categories = Request.Form.Get("category"); // Retrieve as comma-separated string

    // Call the GetBooks method and pass the form data
    List<Book> books = await GetBooks(title, author, series, bookType, categories, lexileMin, lexileMax);

    // Process the books and return the appropriate view
    return View(books);
}


        public ActionResult About()
        {
            return View();
        }

    }
}