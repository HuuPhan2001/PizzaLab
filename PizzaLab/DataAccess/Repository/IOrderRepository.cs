using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        bool SaveOrder(Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);
        bool DeleteOrderByMember(IEnumerable<Order> order);
        IEnumerable<Order> Report(DateTime? startDate, DateTime? endDate);
        IEnumerable<Order> GetAllOrdersByMember(int memberId);
    }
}
