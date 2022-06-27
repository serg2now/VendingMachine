using System;
using Azure.Identity;
using BuyerFunction.DAL;
using BuyerFunction.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BuyerFunction
{
    public class BuyerFunction
    {
        [FunctionName("BuyerFunction")]
        public void Run([TimerTrigger("0 */15 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
            string dbName = Environment.GetEnvironmentVariable("COSMOS_DB_NAME");
            string containerName = Environment.GetEnvironmentVariable("COSMOS_DB_CONTAINER_NAME");

            CosmosClient client = new(connectionString);
            Container container = client.GetContainer(dbName, containerName);
            CosmosDbRepository repository = new(container);

            PurchaseService service = new(repository, log);

            service.BuyProduct();
        }
    }
}

