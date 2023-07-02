using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SureServeTask.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SureServeTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Returns all of the Books by default
        //Filtering/Searching results is done on front-end
        public IActionResult Index()
        {
            string lotrAPIcall;
            lotrAPIcall = Convert.ToString("https://the-one-api.dev/v2/book/");

            string reader = GetBookData(lotrAPIcall);

            List<BookModel.Book> bookDataList = convertJsonToBookData(reader);

            return View("Index", bookDataList);

        }

        //Searches for all of the chapters in agiven book
        //Filtering/Searching results is done on front-end
        [HttpGet]
        public IActionResult SearchBookChapters(string bookID, string bookName)
        {
            string lotrAPIcall;
            lotrAPIcall = Convert.ToString("https://the-one-api.dev/v2/book/"+ bookID +"/chapter");

            string reader = GetBookData(lotrAPIcall);

            List<BookModel.Book> bookDataList = convertJsonToBookData(reader);

            return View("Index", bookDataList);
        }

        public string GetBookData(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public List<BookModel.Book> convertJsonToBookData(string reader)
        {
            BookModel bookModel = new BookModel();

            bookModel = JsonSerializer.Deserialize<BookModel>(reader);

            List<BookModel.Book> bookList = new List<BookModel.Book>();

            foreach(object obj in bookModel.docs)
            {
                BookModel.Book book = new BookModel.Book();

                var bookJSON = obj.ToString();
                book = JsonSerializer.Deserialize<BookModel.Book>(bookJSON);

                bookList.Add(book);

            }

            return bookList;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
