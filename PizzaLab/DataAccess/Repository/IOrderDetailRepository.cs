using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetails(int orderId);
        bool SaveOrderDetail(OrderDetail odd);
        bool UpdateOrderDetail(OrderDetail odd);
        bool DeleteOrderDetail(int orderId, int productId);
        bool DeleteOrderDetail(int orderId);
        OrderDetail findByAnOrderDetail(int OrderId,int ProductId);
    }
}
