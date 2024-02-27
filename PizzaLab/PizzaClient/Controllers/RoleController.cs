using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;

namespace PizzaClient.Controllers
{
    public class RoleController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetRoleAsync(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Role/GetARole?Id=" + Id);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.RoleDTO = await response.Content.ReadAsAsync<RoleDTO>();
            }
            return View();
        }

        public async Task<IActionResult> GetAllRoleAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Role/GetAllRole");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.categories = await response.Content.ReadAsAsync<List<RoleDTO>>();
            }
            return View();
        }
    }
}
