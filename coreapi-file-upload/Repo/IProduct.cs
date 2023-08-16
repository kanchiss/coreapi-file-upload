using coreapi_file_upload.Models;

namespace coreapi_file_upload.Repo
{
    public interface IProduct
    {
        Task<IList<Product>> GetProducts();
        Task<Product> GetProductById(string id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string id);
        Task AttachProductImage(string id, string fileName);
    }
}
