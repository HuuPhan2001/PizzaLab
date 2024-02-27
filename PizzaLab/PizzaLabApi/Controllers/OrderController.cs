using AutoMapper;
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
    public class OrderController : ControllerBase
    {
        private IOrderRepository orderRepository;
        private readonly IMapper mapper;
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        private IOrderDetailRepository orderDetailRepository;
        private IProductRepository productRepository;
        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.productRepository = productRepository;
            mapper = config.CreateMapper();
        }

        [HttpGet("[action]")]
        public IActionResult GetAllOrder()
        {
            return Ok(orderRepository.GetOrders().Where(o => o.IsCart == false).Select(u => mapper.Map<OrderDTO>(u)));
        }
        [HttpGet("[action]")]
        public IActionResult GetAOrder(int Id)
        {
            Order order = null;
            try
            {
                List<Order> orders = orderRepository.GetOrders().ToList();
                order = orders.FirstOrDefault(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                return Conflict("No Order In DB");
            }
            if (order == null)
                return NotFound("Order doesnt exist");
            return Ok(mapper.Map<OrderDTO>(order));
        }
        [HttpGet("[action]/{orderDTO}")]
        public IActionResult AddAOrder(string orderDTO)
        {
            OrderDTO od = JsonSerializer.Deserialize<OrderDTO>(orderDTO);
            try
            {
                Order order = mapper.Map<Order>(od);
                orderRepository.SaveOrder(order);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok(od);


        }
        [HttpGet("[action]/{UserId:int}/{ProductId:int}")]
        public IActionResult AddToCart(int UserId, int ProductId)
        {
            Random random = new Random();
            try
            {
                Order order = orderRepository.GetAllOrdersByMember(UserId).FirstOrDefault(o => o.IsCart == true);
                if (order == null) orderRepository.SaveOrder(new Order {UserId = UserId, Status = false, OrderDate = DateTime.Now, RequiredDate = DateTime.Now.AddDays(1), ShippedDate = DateTime.Now.AddDays(1), ShipAddress = "UnKnown",Freight = random.Next(5),IsCart = true});
                Order orderUpdate = orderRepository.GetAllOrdersByMember(UserId).FirstOrDefault(o => o.IsCart == true);
                Product product = productRepository.GetProducts().FirstOrDefault(p => p.Id == ProductId);
                if (orderDetailRepository.GetOrderDetails(orderUpdate.Id) != null && orderDetailRepository.GetOrderDetails(orderUpdate.Id).Count() != 0)
                {
                    OrderDetail orderdetailExist = orderDetailRepository.GetOrderDetails(orderUpdate.Id).FirstOrDefault(od => od.ProductId == ProductId);
                    if (orderdetailExist != null)
                    {
                        orderdetailExist.Quantity = orderdetailExist.Quantity + 1;
                        orderdetailExist.Price = orderdetailExist.Quantity * product.Price;
                        orderDetailRepository.UpdateOrderDetail(orderdetailExist);
                        return Ok();
                    }
                    else
                    {
                        orderDetailRepository.SaveOrderDetail(new OrderDetail { OrderId = orderUpdate.Id, ProductId = ProductId, Price = product.Price, Quantity = 1 });
                    }
                }
                else
                {
                    orderDetailRepository.SaveOrderDetail(new OrderDetail { OrderId = orderUpdate.Id, ProductId = ProductId, Price = product.Price, Quantity = 1 });
                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok();


        }
        [HttpGet("[action]/{UserId:int}/{ProductId:int}")]
        public IActionResult RemoveFromCart(int UserId, int ProductId)
        {
            try
            {
                Order order = orderRepository.GetAllOrdersByMember(UserId).FirstOrDefault(o => o.IsCart == true);
                orderDetailRepository.DeleteOrderDetail(order.Id, ProductId);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok();


        }
        [HttpGet("[action]/{UserId:int}")]
        public IActionResult GetCart(int UserId)
        {

            try
            {
                Order order = orderRepository.GetOrders().FirstOrDefault(o => o.IsCart == true&&o.UserId == UserId);
                return Ok(mapper.Map<OrderDTO>(order));
            }
            catch (Exception ex)
            {
                return Ok(null);
            }

        }
        [HttpGet("[action]/{orderDTO}")]
        public IActionResult CheckOut(string orderDTO)
        {
            OrderDTO order = JsonSerializer.Deserialize<OrderDTO>(orderDTO);
            try
            {
                order.IsCart = false;
                orderRepository.UpdateOrder(mapper.Map<Order>(order));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok();


        }


        [HttpGet("[action]/{userId:int}")]
        public IActionResult LoadAllOrder(int userId)
        {
            try
            {
                return Ok(orderRepository.GetOrders().Where(o => o.IsCart == false&&o.UserId == userId).Select(u => mapper.Map<OrderDTO>(u)));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }


        }
        [HttpGet("[action]/{orderDTO}")]
        public IActionResult UpdateAOrder(string orderDTO)
        {
            OrderDTO od = JsonSerializer.Deserialize<OrderDTO>(orderDTO);
                try
                {
                    Order order = mapper.Map<Order>(od);
                    orderRepository.UpdateOrder(order);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
                return Ok(od);
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteAOrder(int Id)
        {
            Order order = null;
            try
            {
                List<Order> orders = orderRepository.GetOrders().ToList();
                order = orders.FirstOrDefault(u => u.Id == Id);
                if (order == null)
                    return NotFound("Order doesnt exist");
                orderRepository.DeleteOrder(order);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(orderRepository.GetOrders());
        }
    }
}
