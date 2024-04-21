using BookBox_UI.Models;
using BookBox_UI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

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

        [HttpGet]

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddAuthorViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7007/api/authors"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<AuthorDto>();
            if (response != null)
            {
                return RedirectToAction("Index", "Authors");
            }
            return View();

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<AuthorDto>($"https://localhost:7007/api/authors/{id.ToString()}");
            if (response != null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(AuthorDto request)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {

                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7007/api/authors/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")

            };
            var httpResponseMessage =  await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<AuthorDto>();

            if(response != null)
            {
                return RedirectToAction("Edit", "Authors");
            }

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Delete( AuthorDto request)
        {
            try
            {


                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7007/api/authors/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Authors");
            }
            catch(Exception ex)
            {
                //log exception
            }
            return View("Edit");


        }

    }
}

