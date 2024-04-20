using BookBox_UI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BookBox_UI.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthorsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task <IActionResult> Index()
        {
            List<AuthorDto> response = new List<AuthorDto> ();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7007/api/authors");

                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<AuthorDto>>());
            }
            catch (Exception ex)
            {
                // Log exception
            }




            return View(response);
        }
    }
}

