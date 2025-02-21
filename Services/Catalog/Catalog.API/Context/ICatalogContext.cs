using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Context
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
    }
}
