using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
    public class OrderDetailDao
    {
        private static OrderDetailDao instance = null;
        public static readonly object instanceLock = new object();
        private static PizzaLabContext dbcontext = new PizzaLabContext();

        public static OrderDetailDao Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new OrderDetailDao();
                }
                dbcontext = new PizzaLabContext();
                return instance;
            }
        }
        public IEnumerable<OrderDetail> listOrderDetail(int orderId)
        {
            var listMem = new List<OrderDetail>();
            try
            {
                listMem = dbcontext.OrderDetails.Include(od => od.Product).Where(x => x.OrderId == orderId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMem;
        }
        public OrderDetail getDetail(int orderId,int productId)
        {
            OrderDetail odd = new OrderDetail();
            try
            {
                odd = dbcontext.OrderDetails.Include(od => od.Product).SingleOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return odd;
        }

       
        public bool addOrderDetail(OrderDetail orderdetail)
        {
            try
            {
                dbcontext.OrderDetails.Add(orderdetail);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool updateOrderDetail(OrderDetail orderdetail)
        {
            try
            {
                OrderDetail odd = getDetail(orderdetail.OrderId,orderdetail.ProductId);
                if (odd != null)
                {
                    dbcontext.Entry(odd).CurrentValues.SetValues(orderdetail);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Order does not have detail!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool deleteOrderDetail(int orderId, int productId)
        {
            try
            {
                IEnumerable<OrderDetail> odd = dbcontext.OrderDetails.Where(x => x.OrderId == orderId && x.ProductId == productId).ToList();
                if (odd != null)
                {
                    dbcontext.OrderDetails.RemoveRange(odd);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Order does not have detail!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public bool deleteOrderDetail(int orderId)
        {
            try
            {
                IEnumerable<OrderDetail> odd = dbcontext.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                if (odd != null)
                {
                    dbcontext.OrderDetails.RemoveRange(odd);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Order does not have detail!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
