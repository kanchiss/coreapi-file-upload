using Amazon.Runtime.Internal;
using coreapi_file_upload.Models;
using MongoDB.Driver;

namespace coreapi_file_upload.Repo
{
    public class ProductRepo : IProduct
    {
        private IConfiguration _configuration;
        private static string PRODUCT = "PRODUCT";
        public ProductRepo(IConfiguration configuration) { _configuration = configuration; }
        private IMongoDatabase CreateNoSqlContext()
        {
            var client = new MongoClient(_configuration.GetSection("MongoDB").GetSection("DBConnectionString").Value);
            var database = client.GetDatabase(_configuration.GetSection("MongoDB").GetSection("DatabaseName").Value);
            return database;
        }


        public async Task<IList<Product>> GetProducts()
        {
            return await CreateNoSqlContext().GetCollection<Product>(PRODUCT).AsQueryable().ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await CreateNoSqlContext().GetCollection<Product>(PRODUCT).Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddProduct(Product product)
        {
            await CreateNoSqlContext().GetCollection<Product>(PRODUCT).InsertOneAsync(product);
        }

        public async Task UpdateProduct(Product product)
        {
            await CreateNoSqlContext().GetCollection<Product>(PRODUCT).ReplaceOneAsync(x => x.Id == product.Id, product);
        }

        public async Task DeleteProduct(string id)
        {
            await CreateNoSqlContext().GetCollection<Product>(PRODUCT).DeleteOneAsync(x => x.Id == id);
        }

        public async Task AttachProductImage(string id, string fileName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var update = Builders<Product>.Update
                                    .Set(p => p.ProductImageName, fileName);

            await CreateNoSqlContext().GetCollection<Product>(PRODUCT).FindOneAndUpdateAsync(filter, update);
        }
    }
}
