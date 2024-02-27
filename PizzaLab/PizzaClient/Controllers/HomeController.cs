using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PizzaClient.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Index()
        {
			var myCookieValue = Request.Cookies["jwt"]?.ToString();
			HttpResponseMessage response = await client.GetAsync("Review/GetAllReview");
			HttpResponseMessage responsePro = await client.GetAsync("Product/GetAllProduct");
			try
			{
				response.EnsureSuccessStatusCode();
				responsePro.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
			}
			if (response.IsSuccessStatusCode && responsePro.IsSuccessStatusCode)
			{
				ViewBag.reviews = await response.Content.ReadAsAsync<List<ReviewDTO>>();
				List < ProductDTO > ps = await responsePro.Content.ReadAsAsync<List<ProductDTO>>();
				
				ViewBag.products = ps.Where(p => p.CategoryId == 1).ToList();
			}
            return View();
		}

        public async Task<IActionResult> Privacy()
        {
	        return View();
        }
    }
}