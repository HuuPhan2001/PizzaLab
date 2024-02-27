using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using PizzaLabApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PizzaClient.Controllers
{
	public class UserController : BaseController
	{
		public IConfiguration _configuration;

		public UserController(IConfiguration configuration) : base()
		{
			_configuration = configuration;
		}
		/// <summary>
		/// targetURL la url ma nguoi dung truoc do nhan vao nhung khong co quyen truy cap
		/// </summary>
		/// <param name="targetURL"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> OpenLogin(string targetURL)
		{
			ViewBag.TargetURL = targetURL;
			return View("./Login");
		}
		[HttpGet]
		public async Task<IActionResult> LogOut()
		{
			Response.Cookies.Delete("jwt");
			return View("./Login");
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login(string email, string password, string targetURL)
		{
			UserDTO user;
			if (email != null && password != null)
			{
				HttpResponseMessage response = await client.PostAsJsonAsync(
				$"User/GetAnUserLogin?email={email}&&password={password}", "");
				try
				{
					response.EnsureSuccessStatusCode();
				}
				catch
				{
					//Tra ve thong bao loi
					ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();


                }
				user = await response.Content.ReadAsAsync<UserDTO>();
				if (user != null)
				{
					//create claims details based on the user information
					var claims = new[] {
						new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
						new Claim(ClaimTypes.Role, user.Role.Name),
						new Claim("UserId", user.Id.ToString()),
						new Claim("Email", user.Email),
						new Claim("UserName",user.FirstName + user.LastName),
					};

					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var token = new JwtSecurityToken(
						_configuration["Jwt:Issuer"],
						_configuration["Jwt:Audience"],
						claims,
						expires: DateTime.UtcNow.AddDays(10),
						signingCredentials: signIn);
                    Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(10)
                    });
                  
					if (user.RoleId == 2)
					{
						return RedirectToAction("Dashboard","Admin");
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
					//return View();
				}
				else
				{
					ViewBag.Error = "Invalid credentials";
					return View();
				}
			}
			else
			{
				ViewBag.Error = "Data is invalid";
				return View();
			}
		}
		public async Task<IActionResult> CreateUserAsync(UserDTO user)
		{

			HttpResponseMessage response = await client.PostAsJsonAsync(
				"User/AddAnUser", user);
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
			//Tra ve san pham da add
			ViewBag.UserDTO = await response.Content.ReadAsAsync<UserDTO>();
			// return URI of the created resource.
			return View();//TO-DO: Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> GetUserAsync(int Id)
		{
			HttpResponseMessage response = await client.GetAsync("User/GetAnUser?Id=" + Id);
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
				ViewBag.UserDTO = await response.Content.ReadAsAsync<UserDTO>();
			}
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> GetAllUserAsync()
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
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> UpdateUserAsync(UserDTO user)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync(
				$"User/UpdateAnUser", user);
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

			// Deserialize the updated user from the response body.
			ViewBag.UserDTO = await response.Content.ReadAsAsync<UserDTO>();
			return View();//TO-DO:Dat Duong dan rang tra ve
		}

		public async Task<IActionResult> DeleteUserAsync(int Id)
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
				return View();
			}
			return View();//TO-DO:Dat Duong dan rang tra ve
		}
	}
}
