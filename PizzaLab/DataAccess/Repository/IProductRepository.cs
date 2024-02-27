using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        bool SaveProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        IEnumerable<Product> searchProduct(int? Id, string? name, decimal? price, double? weight);
        IEnumerable<Product> findByCate(int cateId);
        Product findById(int Id);
    }
}
