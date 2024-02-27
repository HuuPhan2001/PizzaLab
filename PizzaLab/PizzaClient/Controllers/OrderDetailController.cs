using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;

namespace PizzaClient.Controllers
{
    public class OrderDetailController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> CreateOrderDetailDTOAsync(OrderDetailDTO orderDetail)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "OrderDetail/AddAOrderDetailDTO", orderDetail);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            //Tra ve san pham da add
            ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
            // return URI of the created resource.
            return View();//TO-DO: Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetOrderDetailDTOAsync(int OrderId,int ProductId)
        {
            HttpResponseMessage response = await client.GetAsync("OrderDetail/GetAOrderDetail?OrderId=" + OrderId + "&&ProductId=" + ProductId);
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
                ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetAllOrderDetailDTOAsync(int OrderId)
        {
            HttpResponseMessage response = await client.GetAsync("OrderDetail/GetAllOrderDetail/" + OrderId);
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
                ViewBag.orderDetails = await response.Content.ReadAsAsync<List<OrderDetailDTO>>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> UpdateOrderDetailDTOAsync(OrderDetailDTO orderDetail)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                  $"OrderDetail/UpdateAOrderDetail", orderDetail);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
                ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }

            // Deserialize the updated orderDetail from the response body.
            ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> DeleteOrderDetailDTOAsync(int Orderid, int Productid)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"OrderDetail/DeleteAOrderDetail?OrderId=" + Orderid + "&&ProductId=" + Productid);
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
		//public JsonResult AddToCart(int id, string name, decimal price, int quantity)
		//{
  //          var cart = HttpContext.Session.Get<List<OrderDetailDTO>>("Cart") ?? new List<OrderDetailDTO>();

		//	var existingItem = cart.FirstOrDefault(item => item.Id == id);
		//	if (existingItem != null)
		//	{
		//		existingItem.Quantity += quantity;
		//	}
		//	else
		//	{
		//		cart.Add(new OrderDetailDTO { ProductId = id, Price = price, Quantity = quantity });
		//	}
  //          HttpContext.Session.Set("Cart", cart);
		//	cookie.Value = JsonConvert.SerializeObject(cart);
		//	cookie.Expires = DateTime.Now.AddDays(1);
		//	Response.Cookies.Add(cookie);

		//	return Json(new { success = true });
		//}
	}
}
