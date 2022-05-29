using Core.Dtos.Product;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductManager : Repository<Product>,IProductManager
    {
        private readonly IRepository<Product> _productRepository;
        public ProductManager(ApplicationDbContext _context, IRepository<Product> productRepository) : base(_context) {
            _productRepository = productRepository;
        }
        public async Task AddProduct(AddProductModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
                throw new ArgumentValidationException("Title and description should be filled.");

            if (IsDuplicateProduct(model.Title))
                throw new ArgumentValidationException("Item is duplicate.");

            await _productRepository.AddAsync(new Product()
            {
                Title = model.Title,
                Description = model.Description,
                Quantity = model.Quantity
            });
        }
        public async Task UpdateProductAsync(UpdateProductModel model)
        {
            var product = await _productRepository.GetByIdAsync(model.Id);
            if (product == null)
                throw new ArgumentValidationException("Item does not exist.");

            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
                throw new ArgumentValidationException("Title and description should be filled.");

            product.Title = model.Title;
            product.Description = model.Description;
            product.Quantity = model.Quantity;

            await _productRepository.UpdateAsync(product);
        }
        public async Task DeleteProduct(long productId)
        {
            var allproducts = await _productRepository.GetAllAsync();
            var product = allproducts.FirstOrDefault(a => a.Id == productId);
            if (product == null)
                throw new ArgumentValidationException("Item does not exist.");

            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductInfoModel>> GetAll()
        {
            var list = await _productRepository.GetAllAsync();
            return list.Select(x => new ProductInfoModel
            {
                Description = x.Description,
                Id = x.Id,
                Quantity = x.Quantity,
                Title = x.Title,
            }).ToList();
        }
        public bool IsDuplicateProduct(string title)
        {
            return _context.Products.Any(a=>a.Title == title);
        }
    }
}
