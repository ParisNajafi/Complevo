
namespace Core.Dtos.Product
{
    public class UpdateProductModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
