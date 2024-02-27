using AutoMapper;
using BusinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.DTO;
using PizzaLabApi.Models;
using System.Text.Json;

namespace PizzaLabApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private IProductRepository productRepository;
		private readonly IMapper mapper;
		MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
		public ProductController(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
			mapper = config.CreateMapper();
		}

		[HttpGet("[action]")]
		public IActionResult GetAllProduct()
		{
			return Ok(productRepository.GetProducts().Select(u => mapper.Map<ProductDTO>(u)));
		}
		[HttpGet("[action]")]
		public IActionResult GetAProduct(int Id)
		{
			Product product = null;
			try
			{
				List<Product> products = productRepository.GetProducts().ToList();
				product = products.FirstOrDefault(u => u.Id == Id);
			}
			catch (Exception ex)
			{
				return Conflict("No Product In DB");
			}
			if (product == null)
				return NotFound("Product doesnt exist");
			return Ok(mapper.Map<ProductDTO>(product));
		}

		[HttpGet("[action]")]
		public IActionResult GetAllWithSearch(int? id, string? name, decimal? price, double? weight)
		{
			return Ok(productRepository.searchProduct(id, name, price, weight).Select(u => mapper.Map<ProductDTO>(u)));
		}
		[HttpGet("[action]/{productDTO}")]
		public IActionResult AddAProduct(string productDTO)
		{
			ProductDTO prod = JsonSerializer.Deserialize<ProductDTO>(productDTO);
			try
			{
				Product product = mapper.Map<Product>(prod);
				productRepository.SaveProduct(product);
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
			return Ok(prod);

		}
		[HttpGet("[action]/{productDTO}")]
		public IActionResult UpdateAProduct(string productDTO)
		{
			ProductDTO prod = JsonSerializer.Deserialize<ProductDTO>(productDTO);
			try
			{
				Product product = mapper.Map<Product>(prod);
				productRepository.UpdateProduct(product);
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
			return Ok(prod);

		}
		[HttpDelete("[action]")]
		public IActionResult DeleteAProduct(int Id)
		{
			Product product = null;
			try
			{
				List<Product> products = productRepository.GetProducts().ToList();
				product = products.FirstOrDefault(u => u.Id == Id);
				if (product == null)
					return NotFound("Product doesnt exist");
				productRepository.DeleteProduct(product);
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}

			return Ok(productRepository.GetProducts());
		}
	}
}
