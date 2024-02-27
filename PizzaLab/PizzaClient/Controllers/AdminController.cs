using BusinessObject.Models;
using DataAccess.Dao;
using Microsoft.AspNetCore.Mvc;

using PizzaLabApi.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace PizzaClient.Controllers
{
	public class AdminController : BaseController
	{
		public async Task<IActionResult> DashBoard()
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
				List<OrderDTO> order = await response.Content.ReadAsAsync<List<OrderDTO>>();
				ViewBag.orders = order.ToList();

			}
			return View();
		}
		public async Task<IActionResult> AddOrder()
		{
			HttpResponseMessage response = await client.GetAsync("User/GetAllUser");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.users = await response.Content.ReadAsAsync<List<UserDTO>>();
			}
			return View();
		}
		public async Task<IActionResult> CreateOrder(OrderDTO order)
		{
			string data = JsonSerializer.Serialize(order);
			HttpResponseMessage response = await client.GetAsync(
				 $"Order/AddAOrder/{data}");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("AddOrder");
			}
			//Tra ve san pham da add
			ViewBag.OrderDTO = await response.Content.ReadAsAsync<OrderDTO>();
			// return URI of the created resource.
			return RedirectToAction("DashBoard","Admin");//TO-DO: Dat Duong dan rang tra ve
		}


		[HttpGet("Admin/ListDetail/{id}")]
		public async Task<IActionResult> ListDetail(int id)
		{
			HttpResponseMessage responseDetail = await client.GetAsync("OrderDetail/GetAllOrderDetail/" + id);
			ViewBag.orderId = id;
			try
			{
				responseDetail.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await responseDetail.Content.ReadAsStringAsync();
				return View();
			}
			if (responseDetail.IsSuccessStatusCode)
			{
				
				ViewBag.details = await responseDetail.Content.ReadAsAsync<List<OrderDetailDTO>>();
			}
			return View();
		}

		[HttpGet("Admin/AddDetail/{id}")]
		public async Task<IActionResult> AddDetail(int id)
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAllProduct");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.orderId = id;
				ViewBag.products = await response.Content.ReadAsAsync<List<ProductDTO>>();
			}
			return View();
		}
		public async Task<IActionResult> CreateDetail(OrderDetailDTO detailDTO)
		{
			string data = JsonSerializer.Serialize(detailDTO);
			HttpResponseMessage response = await client.GetAsync(
				 $"OrderDetail/AddAOrderDetail/{data}");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); 
				Redirect($"/Admin/AddDetail/{detailDTO.OrderId}");
			}
			//Tra ve san pham da add
			ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
			// return URI of the created resource.
			return Redirect($"/Admin/ListDetail/{detailDTO.OrderId}");
		}
		[HttpGet("Admin/UpdateDetail/{orderId}/{proId}")]
		public async Task<IActionResult> UpdateDetail(int orderId, int proId)
		{
			HttpResponseMessage response = await client.GetAsync("OrderDetail/GetAnOrderDetail?OrderId=" + orderId + "&&ProductId=" + proId);
			HttpResponseMessage responsePro = await client.GetAsync("Product/GetAllProduct");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return Redirect($"/Admin/ListDetail/{orderId}");
			}
			//Tra ve san pham da add
			ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
			ViewBag.orderId = orderId;
			ViewBag.products = await responsePro.Content.ReadAsAsync<List<ProductDTO>>();
			// return URI of the created resource.
			return View();
		}
		public async Task<IActionResult> EditDetail(OrderDetailDTO orderDetailDTO)
		{
			string data = JsonSerializer.Serialize(orderDetailDTO);
			HttpResponseMessage response = await client.GetAsync(
				 $"OrderDetail/UpdateAOrderDetail/{data}");

			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return Redirect($"/Admin/ListDetail/{orderDetailDTO.OrderId}");
			}
			//Tra ve san pham da add
			ViewBag.OrderDetailDTO = await response.Content.ReadAsAsync<OrderDetailDTO>();
			ViewBag.products = await response.Content.ReadAsAsync<List<ProductDTO>>();
			// return URI of the created resource.
			return Redirect($"/Admin/ListDetail/{orderDetailDTO.OrderId}");
		}
		[HttpGet("Admin/DeleteDetail/{orderId}/{productId}")]
		public async Task<IActionResult> DeleteDetail(int orderId, int productId)
		{
			HttpResponseMessage responseDetail = await client.DeleteAsync(
				$"OrderDetail/DeleteAnOrderDetail?orderId=" + orderId + "&&productId=" + productId);
			try
			{
				responseDetail.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await responseDetail.Content.ReadAsStringAsync();
				return Redirect($"/Admin/ListDetail/{orderId}");
			}
			return RedirectToAction("DashBoard", "Admin");//TO-DO:Dat Duong dan rang tra ve
		}
		public async Task<IActionResult> ListProduct(int? id, string? name, decimal? price, double? weight)
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAllWithSearch?Id=" + id + "&&name=" + name + "&&price=" + price + "&&weight=" + weight);
			HttpResponseMessage responseCate = await client.GetAsync("Category/GetAllCategory");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				ViewBag.Error = await responseCate.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode && responseCate.IsSuccessStatusCode)
			{
				ViewBag.products = await response.Content.ReadAsAsync<List<ProductDTO>>();
				ViewBag.categories = await responseCate.Content.ReadAsAsync<List<CategoryDTO>>();
				ViewBag.CateId = id.HasValue && id != 0 ? id : 0;
				ViewBag.Name = !string.IsNullOrEmpty(name) ? name : "";
				ViewBag.Price = price.HasValue && price != 0 ? price : 0;
				ViewBag.Weight = weight.HasValue && weight != 0 ? weight : 0;
			}
			return View();
		}
		public async Task<IActionResult> EditAccount(UserDTO user)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync(
				$"User/UpdateAnUser", user);
			HttpResponseMessage responseRo = await client.GetAsync("Role/GetAllRole");
			try
			{
				response.EnsureSuccessStatusCode();
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View();
			}

			// Deserialize the updated user from the response body.
			ViewBag.UserDTO = await response.Content.ReadAsAsync<UserDTO>();
			ViewBag.Role = await responseRo.Content.ReadAsAsync<RoleDTO>();
			return View();
		}
		public async Task<IActionResult> DeleteAccount(int Id)
		{
			HttpResponseMessage response = await client.DeleteAsync(
				$"User/DeleteAnUser?Id=" + Id);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View("Account");
			}
			return View("Account");//TO-DO:Dat Duong dan rang tra ve
		}
		public async Task<IActionResult> Account()
		{
			HttpResponseMessage response = await client.GetAsync("User/GetAllUser");

			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.users = await response.Content.ReadAsAsync<List<UserDTO>>();
			}
			return View();
		}
		[HttpGet("Admin/EditOrder/{id}")]
		public async Task<IActionResult> EditOrder(int id)
		{
			HttpResponseMessage response = await client.GetAsync("Order/GetAOrder?Id=" + id);
			HttpResponseMessage responseUser = await client.GetAsync("User/GetAllUser");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				ViewBag.Error = await responseUser.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode && responseUser.IsSuccessStatusCode)
			{
				ViewBag.orders = await response.Content.ReadAsAsync<OrderDTO>();
				ViewBag.users = await responseUser.Content.ReadAsAsync<List<UserDTO>>();
			}
			return View();
		}
		public async Task<IActionResult> UpdateOrder(OrderDTO order)
		{
			string data = JsonSerializer.Serialize(order);
			HttpResponseMessage response = await client.GetAsync(
				 $"Order/UpdateAOrder/{data}");
			//HttpResponseMessage response = await client.GetAsync($"Order/UpdateAOrder/{data}");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return View("DashBoard");
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.orders = await response.Content.ReadAsAsync<OrderDTO>();
			}
			return RedirectToAction("DashBoard","Admin");
		}
		[HttpGet("Admin/DeleteOrder/{Id}")]
		public async Task<IActionResult> DeleteOrder(int Id)
		{
			HttpResponseMessage responseDetail = await client.DeleteAsync(
				$"OrderDetail/DeleteAllOrderDetail?OrderId=" + Id);
			HttpResponseMessage response = await client.DeleteAsync(
				$"Order/DeleteAOrder?Id=" + Id);
			
			try
			{
				response.EnsureSuccessStatusCode();
				responseDetail.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await responseDetail.Content.ReadAsStringAsync();
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				return RedirectToAction("DashBoard", "Admin");
			}
			return RedirectToAction("DashBoard", "Admin");//TO-DO:Dat Duong dan rang tra ve
		}
		[HttpGet("Admin/EditProductForm/{proId}")]
		public async Task<IActionResult> EditProductForm(int proId)
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAProduct?Id=" + proId);
			HttpResponseMessage responseCate = await client.GetAsync("Category/GetAllCategory");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				ViewBag.Error = await responseCate.Content.ReadAsStringAsync();
				return View();
			}
			if (response.IsSuccessStatusCode && responseCate.IsSuccessStatusCode)
			{
				ViewBag.products = await response.Content.ReadAsAsync<ProductDTO>();
				ViewBag.categories = await responseCate.Content.ReadAsAsync<List<CategoryDTO>>();
			}
			return View();
		}
		public async Task<IActionResult> EditProduct(ProductDTO product)
		{
			string data = JsonSerializer.Serialize(product);
			HttpResponseMessage response = await client.GetAsync(
				 $"Product/UpdateAProduct/{data}");
			HttpResponseMessage responseCate = await client.GetAsync("Category/GetAllCategory");
			try
			{
				response.EnsureSuccessStatusCode();

			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync();
				ViewBag.Error = await responseCate.Content.ReadAsStringAsync();
				return Redirect($"./EditProductForm/{product.Id}");
			}
			ViewBag.categories = await response.Content.ReadAsAsync<List<CategoryDTO>>();
			// Deserialize the updated product from the response body.
			ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			return RedirectToAction("ListProduct", "Admin"); ;
		}
		public async Task<IActionResult> AddProductForm()
		{
			HttpResponseMessage response = await client.GetAsync("Category/GetAllCategory");
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
				ViewBag.categories = await response.Content.ReadAsAsync<List<CategoryDTO>>();
			}
			return View();
		}

		public async Task<IActionResult> AddProduct(ProductDTO product)
		{
			string data = JsonSerializer.Serialize(product);
			HttpResponseMessage response = await client.GetAsync(
				 $"Product/AddAProduct/{data}");
			HttpResponseMessage responseCate = await client.GetAsync("Category/GetAllCategory");
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); 
				// return URI of the created resource.
				return RedirectToAction("AddProductForm","Admin"); 
			}
			//Tra ve san pham da add
			ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			ViewBag.categories = await responseCate.Content.ReadAsAsync<List<CategoryDTO>>();

			// return URI of the created resource.
			return View("./ListProduct");
		}
		public async Task<IActionResult> GetProductDTOAsync(int Id)
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAProduct?Id=" + Id);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View("./ListProduct");
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			}
			return View("./ListProduct");//TO-DO:Dat Duong dan rang tra ve
		}
		[HttpGet("Admin/DeleteProduct/{Id}")]
		public async Task<IActionResult> DeleteProduct(int Id)
		{
			HttpResponseMessage response = await client.DeleteAsync(
				$"Product/DeleteAProduct?Id=" + Id);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
			}
			return RedirectToAction("ListProduct", "Admin");//TO-DO:Dat Duong dan rang tra ve
		}
	}
}
