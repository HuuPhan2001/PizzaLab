using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace PizzaClient.Controllers
{
    public class BaseController : Controller
    {
        protected static HttpClient client;

        public BaseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7250/");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
