using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using BuyerFunction.DAL;
using BuyerFunction.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;
using Microsoft.Azure.WebPubSub.Common;
using Microsoft.Extensions.Logging;

namespace BuyerFunction
{
    public class BuyerFunction
    {
        [FunctionName("BuyerFunction")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [WebPubSub(Hub = "notification")] IAsyncCollector<WebPubSubAction> actions,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
            string dbName = Environment.GetEnvironmentVariable("COSMOS_DB_NAME");
            string containerName = Environment.GetEnvironmentVariable("COSMOS_DB_CONTAINER_NAME");

            CosmosClient client = new(connectionString);
            Container container = client.GetContainer(dbName, containerName);
            CosmosDbRepository repository = new(container);

            PurchaseService service = new(repository, log);

            PurchaseDao purchase = service.BuyProduct();

            string purchaseJson = JsonSerializer.Serialize(purchase);
            SendToAllAction action = new()
            { 
                Data = BinaryData.FromString(purchaseJson),
                DataType = WebPubSubDataType.Text
            };
            await actions.AddAsync(action);
        }
    }
}

