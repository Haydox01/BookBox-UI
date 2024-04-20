using BookBox_UI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookBox_UI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BookDto> response = new List<BookDto>(); // Creating a list of DTO
            try
            {


                //Get all Books from Web API
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7007/api/books");

                httpResponseMessage.EnsureSuccessStatusCode(); //Checks if it is success
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<BookDto>>());
               
            } catch (Exception ex)
            {
                //Log the exception
            }


            return View(response); // The view is getting the list of DTO
        }
    }
}
