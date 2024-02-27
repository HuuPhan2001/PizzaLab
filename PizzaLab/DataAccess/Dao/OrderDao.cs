using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
    public class OrderDao
    {
        private static OrderDao instance = null;
        public static readonly object instanceLock = new object();
        private static PizzaLabContext dbcontext = new PizzaLabContext();

        public static OrderDao Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new OrderDao();
                }
                dbcontext = new PizzaLabContext();
                return instance;
            }
        }
        public IEnumerable<Order> listOrder()
        {
            var listOd = new List<Order>();
            try
            {
                listOd = dbcontext.Orders.Include(o => o.OrderDetails).ThenInclude(od=> od.Product).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOd;

        }
        public Order findById(int Id)
        {
            Order order = new Order();
            try
            {
                order = dbcontext.Orders.Include(o => o.OrderDetails).SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return order;
        }
        public bool addOrder(Order order)
        {
            try
            {
                dbcontext.Orders.Add(order);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool updateOrder(Order order)
        {
            try
            {
                Order od = findById(order.Id);
                if (od != null)
                {
                    dbcontext.Entry(od).CurrentValues.SetValues(order);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Order does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public IEnumerable<Order> findMember(int userId)
        {
            var listOrder = new List<Order>();
            try
            {
                listOrder = dbcontext.Orders.Include(o => o.OrderDetails).Where(x => x.UserId == userId).ToList();
                if (listOrder is null)
                {
                    throw new Exception("User does not have order!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrder;
        }

        public bool deleteAllOrder(IEnumerable<Order> orders)
        {
            try
            {
                if (orders is not null)
                {
                    dbcontext.Orders.RemoveRange(orders);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Member does not have order!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        public bool deleteOrder(Order order)
        {
            try
            {
                if (order != null)
                {
                    dbcontext.Orders.Remove(order);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Order does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public IEnumerable<Order> report(DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Order> listReport;
            try
            {
                if (startDate is null && endDate is null)
                {
                    listReport = dbcontext.Orders.ToList();
                }
                else if (startDate is null && endDate is not null)
                {
                    listReport = dbcontext.Orders.Where(x => x.OrderDate <= endDate).ToList();
                }
                else if (startDate is not null && endDate is null)
                {
                    listReport = dbcontext.Orders.Where(x => x.OrderDate >= startDate).ToList();
                }
                else if (startDate is not null && endDate is not null && startDate < endDate)
                {
                    listReport = dbcontext.Orders.Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate).ToList();
                }
                else
                {
                    throw new Exception("Invalid start - end date!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listReport;
        }
    }
}
