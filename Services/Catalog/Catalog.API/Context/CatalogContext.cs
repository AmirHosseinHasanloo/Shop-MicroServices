using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Context
{
    public class CatalogContext : ICatalogContext
    {
        
        public IMongoCollection<Product> Products { get; }
        public CatalogContext(IConfiguration _configuration)
        {
            var client = new MongoClient(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var dataBase = client.GetDatabase(_configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = dataBase.GetCollection<Product>(_configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeedData.seedData(Products);
        }
       
       
    }
}
