using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace PizzaClient.Controllers
{

    public class OrderController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateOrderDTOAsync(OrderDTO order)
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "Order/AddAOrderDTO", order);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("Error");
            }
            //Tra ve san pham da add
            ViewBag.OrderDTO = await response.Content.ReadAsAsync<OrderDTO>();
            // return URI of the created resource.
            return View();//TO-DO: Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetOrderDTOAsync(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Order/GetAOrder?Id=" + Id);
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
                ViewBag.OrderDTO = await response.Content.ReadAsAsync<OrderDTO>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetAllOrderDTOAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Order/GetAllOrder");
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
                ViewBag.orders = await response.Content.ReadAsAsync<List<OrderDTO>>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> UpdateOrderDTOAsync(OrderDTO order)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"Order/UpdateAOrder", order);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }

            // Deserialize the updated order from the response body.
            ViewBag.OrderDTO = await response.Content.ReadAsAsync<OrderDTO>();
            return View();//TO-DO:Dat Duong dan rang tra ve
        }
        [HttpGet("Order/AddToCart/{ProductId:int}")]
        public async Task<IActionResult> AddToCart(int ProductId)
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.GetAsync(
                $"Order/AddToCart/{userId}/{ProductId}");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return BadRequest();
            }
            //Tra ve san pham da add
            // return URI of the created resource.
            return Ok();//TO-DO: Dat Duong dan rang tra ve
        }
        [HttpGet("Order/RemoveFromCart/{ProductId:int}")]
        public async Task<IActionResult> RemoveFromCart(int ProductId)
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.GetAsync(
                $"Order/RemoveFromCart/{userId}/{ProductId}");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return BadRequest();
            }
            //Tra ve san pham da add
            // return URI of the created resource.
            return Ok();//TO-DO: Dat Duong dan rang tra ve
        }
        [Authorize(Roles = "Member")]
        [HttpGet("Order/GetCart")]
        public async Task<IActionResult> GetCart()
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.GetAsync($"Order/GetCart/{userId}");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return Redirect("../Home/Index");
            }
            //Tra ve san pham da add
            if (response.IsSuccessStatusCode)
            {
                if (response.Content == null || response.Content.ToString().Length == 0)
                    ViewBag.order = null;
                else
                    ViewBag.order = await response.Content.ReadAsAsync<OrderDTO>();
            }
            // return URI of the created resource.
            return View("./Cart");//TO-DO: Dat Duong dan rang tra ve
        }
        [Authorize(Roles = "Member")]
        [HttpPost("Order/CheckOut")]
        public async Task<IActionResult> CheckOut(OrderDTO order)
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.GetAsync(
                $"Order/CheckOut/{JsonSerializer.Serialize(order)}");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return Redirect("../Home/Index");
            }
            //Tra ve san pham da add
            // return URI of the created resource.
            return Redirect("../Order/LoadAllOrder");
        }
        [Authorize(Roles = "Member")]
        [HttpGet("Order/LoadAllOrder")]
        public async Task<IActionResult> LoadAllOrder()
        {
            HttpContext hc = HttpContext;
            ClaimsPrincipal claimsPrincipal = hc.User;
            string userId = claimsPrincipal.FindFirstValue("UserId");
            string email = claimsPrincipal.FindFirstValue("Email");
            string userName = claimsPrincipal.FindFirstValue("UserName");
            HttpResponseMessage response = await client.GetAsync(
                $"Order/LoadAllOrder/{userId}");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return Redirect("Home/Index");
            }
            //Tra ve san pham da add
                List < OrderDTO > orders = await response.Content.ReadAsAsync<List<OrderDTO>>();
            ViewBag.orders = orders.OrderByDescending(o => o.Id).ToList();

            // return URI of the created resource.
            return View("./OrderPersonal");//TO-DO: Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> DeleteOrderDTOAsync(int Id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"Order/DeleteAOrder?Id=" + Id);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }
    }
}
