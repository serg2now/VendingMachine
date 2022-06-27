using System.Collections.Generic;

namespace BuyerFunction.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<int, Product> Products { get; set; }
    }
}
