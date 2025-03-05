using Catalog.API.Context;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Constructor
        private ICatalogContext _catalogContext;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        #endregion

        #region Product

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products
                .Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _catalogContext.Products
                .Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            FilterDefinition<Product> filter =
               Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _catalogContext.Products.Find(filter).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter =
                Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _catalogContext.Products.Find(filter).ToListAsync();
        }
        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var UpdateResult = await _catalogContext.Products.
                ReplaceOneAsync(filter: p => p.Id == product.Id
                , replacement: product);

            return UpdateResult.IsAcknowledged
                && UpdateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter =
                 Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult result = await _catalogContext.
                Products.DeleteOneAsync(filter);

            return result.IsAcknowledged
               && result.DeletedCount > 0;
        }
        #endregion
    }
}
