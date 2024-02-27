using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;
using System.Security.Claims;

namespace PizzaClient.Controllers
{
	public class ProductController : BaseController
	{
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> Menu()
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAllProduct");
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
				List<ProductDTO> pizza = await response.Content.ReadAsAsync<List<ProductDTO>>();
				ViewBag.pro1 = pizza.Where(p => p.CategoryId == 1).ToList();
				ViewBag.pro2 = pizza.Where(p => p.CategoryId == 2).ToList();
				ViewBag.pro3 = pizza.Where(p => p.CategoryId == 3).ToList();
				ViewBag.pro4 = pizza.Where(p => p.CategoryId == 4).ToList();

			}
			return View();
		}
		[Authorize(Roles = "Member")]
		public async Task<IActionResult> Review()
		{
			HttpContext hc = HttpContext;
			ClaimsPrincipal claimsPrincipal = hc.User;
			string userId = claimsPrincipal.FindFirstValue("UserId");
			string email = claimsPrincipal.FindFirstValue("Email");
			string userName = claimsPrincipal.FindFirstValue("UserName");
			HttpResponseMessage response = await client.GetAsync("Review/GetAllReview");
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
				ViewBag.reviews = await response.Content.ReadAsAsync<List<ReviewDTO>>();
			}
			return View();
		}

		[Authorize(Roles = "Member")]
		public async Task<IActionResult> PostReview(ReviewDTO review)
		{
			HttpContext hc = HttpContext;
			ClaimsPrincipal claimsPrincipal = hc.User;
			string userId = claimsPrincipal.FindFirstValue("UserId");
			string email = claimsPrincipal.FindFirstValue("Email");
			string userName = claimsPrincipal.FindFirstValue("UserName");
			HttpResponseMessage response = await client.PostAsJsonAsync(
			   "Review/AddAReviewDTO", review);
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
			ViewBag.ReviewDTO = await response.Content.ReadAsAsync<ReviewDTO>();
			// return URI of the created resource.
			return View();
		}

		public async Task<IActionResult> CreateProductDTOAsync(ProductDTO product)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(
				"Product/AddAProduct", product);
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
			ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			// return URI of the created resource.
			return View();//TO-DO: Dat Duong dan rang tra ve
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
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
			}
			if (response.IsSuccessStatusCode)
			{
				ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			}
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> GetAllProductDTOAsync()
		{
			HttpResponseMessage response = await client.GetAsync("Product/GetAllProduct");
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
				ViewBag.ProductDTO = await response.Content.ReadAsAsync<List<ProductDTO>>();
			}
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> UpdateProductDTOAsync(ProductDTO product)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync(
				$"Product/UpdateAProduct", product);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				//Tra ve thong bao loi
				ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
			}

			// Deserialize the updated product from the response body.
			ViewBag.ProductDTO = await response.Content.ReadAsAsync<ProductDTO>();
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> DeleteProductDTOAsync(int Id)
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
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

	}
}
