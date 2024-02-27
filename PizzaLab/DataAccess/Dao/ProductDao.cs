using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
	public class ProductDao
	{
		private static ProductDao instance = null;
		public static readonly object instanceLock = new object();
		private static PizzaLabContext dbcontext = new PizzaLabContext();

		public static ProductDao Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
						instance = new ProductDao();
				}
				dbcontext = new PizzaLabContext();
				return instance;
			}
		}
		public IEnumerable<Product> listProduct()
		{
			var listMem = new List<Product>();
			try
			{
				listMem = dbcontext.Products.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return listMem;

		}
		public Product findById(int Id)
		{
			Product product = new Product();
			try
			{
				product = dbcontext.Products.SingleOrDefault(x => x.Id == Id);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return product;
		}
		public IEnumerable<Product> findByCate(int cateId)
		{
			var product = new List<Product>();
			try
			{
				product = dbcontext.Products.Where(x => x.CategoryId == cateId).ToList();

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return product;
		}
		public bool addProduct(Product product)
		{
			try
			{
				dbcontext.Products.Add(product);
				dbcontext.SaveChanges();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}

		public bool updateProduct(Product product)
		{
			try
			{
				Product pro = findById(product.Id);
				if (pro != null)
				{
					dbcontext.Entry(pro).CurrentValues.SetValues(product);
					dbcontext.SaveChanges();
				}
				else
				{
					throw new Exception("Product does not exist!");
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}

		public bool deleteProduct(Product product)
		{
			try
			{
				Product pro = findById(product.Id);
				if (pro != null)
				{
					dbcontext.Products.Remove(pro);
					dbcontext.SaveChanges();
				}
				else
				{
					throw new Exception("Product does not exist!");
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}

		public IEnumerable<Product> searchProduct(int? Id, string? name, decimal? price, double? weight)
		{
			var listSearch = dbcontext.Products.AsQueryable();
			try
			{
				if (Id.HasValue && Id != 0)
				{
					listSearch = listSearch.Where(x => x.CategoryId == Id);
				}
				if (!string.IsNullOrEmpty(name))
				{
					listSearch = listSearch.Where(x => x.Name.Contains(name));
				}
				if (price.HasValue && price != 0)
				{
					listSearch = listSearch.Where(x => x.Price == price);
				}
				if (weight.HasValue && weight != 0)
				{
					listSearch = listSearch.Where(x => x.Weight == weight);
				}
				//listSearch = dbcontext.Products.Where(x => x.Id == Id ||
				//                                      x.Name.Contains(name) ||
				//                                      x.Price == price ||
				//                                      x.Weight == weight ||
				//                                      x.Id == Id && x.Name.Contains(name) && x.Price == price && x.Weight == weight)
				//                                      .ToList();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return listSearch.ToList();
		}
	}
}
