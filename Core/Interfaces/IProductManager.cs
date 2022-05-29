using Core.Dtos.Product;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductManager : IRepository<Product>
    {
        Task AddProduct(AddProductModel model);
        Task UpdateProductAsync(UpdateProductModel model);
        Task DeleteProduct(long productId);
        Task<IEnumerable<ProductInfoModel>> GetAll();
    }
}
