using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public bool DeleteOrder(Order order) => OrderDao.Instance.deleteOrder(order);

        public bool DeleteOrderByMember(IEnumerable<Order> order) => OrderDao.Instance.deleteAllOrder(order);

        public IEnumerable<Order> GetAllOrdersByMember(int memberId) => OrderDao.Instance.findMember(memberId);

        public IEnumerable<Order> GetOrders() => OrderDao.Instance.listOrder();

        public IEnumerable<Order> Report(DateTime? startDate, DateTime? endDate) => OrderDao.Instance.report(startDate, endDate);

        public bool SaveOrder(Order order) => OrderDao.Instance.addOrder(order);

        public bool UpdateOrder(Order order) => OrderDao.Instance.updateOrder(order);
    }
}
