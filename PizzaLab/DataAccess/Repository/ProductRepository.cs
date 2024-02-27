using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public bool DeleteProduct(Product product) => ProductDao.Instance.deleteProduct(product);

        public IEnumerable<Product> findByCate(int cateId) => ProductDao.Instance.findByCate(cateId);

        public Product findById(int Id) => ProductDao.Instance.findById(Id);

        public IEnumerable<Product> GetProducts() => ProductDao.Instance.listProduct();

        public bool SaveProduct(Product product) => ProductDao.Instance.addProduct(product);

        public IEnumerable<Product> searchProduct(int? Id, string? name, decimal? price, double? weight) => ProductDao.Instance.searchProduct(Id, name, price, weight);

        public bool UpdateProduct(Product product) => ProductDao.Instance.updateProduct(product);
    }
}
