using Newtonsoft.Json;

namespace BuyerFunction.DAL
{
    public class PurchaseDao
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public decimal Cost { get; set; }
    }
}
