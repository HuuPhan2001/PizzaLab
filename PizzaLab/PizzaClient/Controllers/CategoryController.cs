using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaLabApi.Models;

namespace PizzaClient.Controllers
{
    public class CategoryController : BaseController
    {
        public async Task<IActionResult> Index()
        {
			HttpResponseMessage response = await client.GetAsync("Category/GetAllCategory");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("Error");
			}
			if (response.IsSuccessStatusCode)
			{
				TempData["categories"] = await response.Content.ReadAsAsync<List<CategoryDTO>>();
			}
			return RedirectToAction("Menu","Product");
        }

        public  async Task<IActionResult> GetCategoryAsync(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Category/GetACategory?Id=" + Id);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("Error");
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoryDTO = await response.Content.ReadAsAsync<CategoryDTO>();
            }
            return View("Error");
        }

        public async Task<IActionResult> GetAllCategoryAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Category/GetAllCategory");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("Error");
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.categories = await response.Content.ReadAsAsync<List<CategoryDTO>>();
            }
            return View("/Views/Home/Index.cshtml");
        }
    }
}
