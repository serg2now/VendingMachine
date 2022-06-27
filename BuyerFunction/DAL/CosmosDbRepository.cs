using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace BuyerFunction.DAL
{
    public class CosmosDbRepository
    {
        private readonly Container _container;

        public CosmosDbRepository(Container container)
        {
            _container = container;
        }

        public async Task WriteItemAsync(PurchaseDao purchase)
        {
            await _container.CreateItemAsync(purchase, new PartitionKey(purchase.Id));
        }
    }
}
