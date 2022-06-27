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
        public void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string accountEndPoint = "https://purchase-statistic-app.documents.azure.com:443/";
            string dbName = "statistic-db";
            string containerName = "vendingPurchases";

            CosmosClient client = new(accountEndPoint, new DefaultAzureCredential());
            Container container = client.GetContainer(dbName, containerName);
            CosmosDbRepository repository = new(container);

            PurchaseService service = new(repository, log);

            service.BuyProduct();
        }
    }
}

