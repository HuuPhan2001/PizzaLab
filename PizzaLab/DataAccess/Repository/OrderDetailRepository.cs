using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public bool DeleteOrderDetail(int orderId,int productId) => OrderDetailDao.Instance.deleteOrderDetail(orderId,productId);
        public bool DeleteOrderDetail(int orderId) => OrderDetailDao.Instance.deleteOrderDetail(orderId);

        public OrderDetail findByAnOrderDetail(int OrderId, int ProductId) => OrderDetailDao.Instance.getDetail(OrderId, ProductId);

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId) => OrderDetailDao.Instance.listOrderDetail(orderId);

        public bool SaveOrderDetail(OrderDetail odd) => OrderDetailDao.Instance.addOrderDetail(odd);

        public bool UpdateOrderDetail(OrderDetail odd) => OrderDetailDao.Instance.updateOrderDetail(odd);
    }
}
