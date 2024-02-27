using AutoMapper;
using Azure;
using BusinessObject.Models;
using DataAccess.Dao;
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
	public class OrderDetailController : ControllerBase
	{
		private IOrderDetailRepository orderDetailRepository;
		private IProductRepository proRepository;
		private readonly IMapper mapper;
		MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
		public OrderDetailController(IOrderDetailRepository orderDetailRepository, IProductRepository proRepository)
		{
			this.orderDetailRepository = orderDetailRepository;
			mapper = config.CreateMapper();
			this.proRepository = proRepository;
		}

		[HttpGet("[action]/{OrderId}")]
		public IActionResult GetAllOrderDetail(int OrderId)
		{
			return Ok(orderDetailRepository.GetOrderDetails(OrderId).Select(u => mapper.Map<OrderDetailDTO>(u)));
		}
		[HttpGet("[action]")]
		public IActionResult GetAnOrderDetail(int OrderId, int ProductId)
		{
			OrderDetail orderDetail;
			try
			{
				orderDetail = orderDetailRepository.findByAnOrderDetail(OrderId, ProductId);
			}
			catch (Exception ex)
			{
				return Conflict("No OrderDetail In DB");
			}
			if (orderDetail == null)
				return NotFound("OrderDetail doesnt exist");
			return Ok(mapper.Map<OrderDetailDTO>(orderDetail));
		}
		[HttpGet("[action]/{orderDetailDTO}")]
		public IActionResult AddAOrderDetail(string orderDetailDTO)
		{
			OrderDetailDTO detail = JsonSerializer.Deserialize<OrderDetailDTO>(orderDetailDTO);
			try
			{
				OrderDetail orderDetail = mapper.Map<OrderDetail>(detail);
				OrderDetail exist = orderDetailRepository.findByAnOrderDetail(orderDetail.OrderId, orderDetail.ProductId);
				if (exist != null)
				{
					exist.Quantity += orderDetail.Quantity;
					exist.Price = exist.Quantity * proRepository.findById(orderDetail.ProductId).Price;
					orderDetailRepository.UpdateOrderDetail(exist);
				}
				else
				{
					orderDetail.Price = orderDetail.Quantity * proRepository.findById(orderDetail.ProductId).Price;
					orderDetailRepository.SaveOrderDetail(orderDetail);
				}
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
			return Ok(detail);


		}
		[HttpGet("[action]/{orderDetailDTO}")]
		public IActionResult UpdateAOrderDetail(string orderDetailDTO)
		{
			OrderDetailDTO detail = JsonSerializer.Deserialize<OrderDetailDTO>(orderDetailDTO);
			try
			{
				OrderDetail orderDetail = mapper.Map<OrderDetail>(detail);
				orderDetail.Price = orderDetail.Quantity * proRepository.findById(orderDetail.ProductId).Price;
				orderDetailRepository.UpdateOrderDetail(orderDetail);

			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
			return Ok(detail);
		}
		[HttpDelete("[action]")]
		public IActionResult DeleteAnOrderDetail(int OrderId, int ProductId)
		{
			OrderDetail orderDetail;
			try
			{
				orderDetail = orderDetailRepository.findByAnOrderDetail(OrderId, ProductId);
				if (orderDetail == null)
					return NotFound("OrderDetail doesnt exist");
				orderDetailRepository.DeleteOrderDetail(OrderId, ProductId);
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}

			return Ok(orderDetailRepository.GetOrderDetails(OrderId).Select(u => mapper.Map<OrderDetailDTO>(u)));
		}

		[HttpDelete("[action]")]
		public IActionResult DeleteAllOrderDetail(int OrderId)
		{
			IEnumerable<OrderDetail> orderDetail;
			try
			{
				orderDetail = orderDetailRepository.GetOrderDetails(OrderId);
				if (orderDetail == null)
					return NotFound("OrderDetail Of This Order doesnt exist");
				orderDetailRepository.DeleteOrderDetail(OrderId);
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}

			return Ok();
		}
	}
}
