using BuyerFunction.DAL;
using BuyerFunction.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BuyerFunction.Services
{
    public class PurchaseService
    {
        private readonly CosmosDbRepository _repository;
        private readonly ILogger _logger;

        private readonly Dictionary<int, Company> _companies = new();

        public PurchaseService(CosmosDbRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;

            InitCompanies();
        }

        public void BuyProduct()
        {
            PurchaseDao purchase = BuildPurchase();

            _repository.WriteItemAsync(purchase).Wait();

            _logger.LogInformation($"New purchase '{ purchase.ProductName }' was made, on { purchase.Date }");
        }

        private void InitCompanies()
        {
            var companies = new Company[] 
            {
                new Company
                {
                    Id = 1,
                    Name = "Cola",
                    Products = new Dictionary<int, Product>{ 
                        { 
                            1,
                            new Product
                            {
                                Id = 1,
                                Name = "Coca-Cola",
                                Cost = 4.56M
                            }  
                        },
                        {
                            2,
                            new Product
                            {
                                Id = 2,
                                Name = "Fanta",
                                Cost = 2.12M
                            }
                        },
                        {
                            3,
                            new Product
                            {
                                Id = 3,
                                Name = "Sprite",
                                Cost = 3.95M
                            }
                        } 
                    }
                },
                new Company
                {
                    Id = 2,
                    Name = "Pepsi",
                    Products = new Dictionary<int, Product>{
                        {
                            1,
                            new Product
                            {
                                Id = 1,
                                Name = "7Up",
                                Cost = 3.45M
                            }
                        },
                        {
                            2,
                            new Product
                            {
                                Id = 2,
                                Name = "Pepsi",
                                Cost = 3.56M
                            }
                        },
                     }
                },
                new Company
                {
                    Id = 3,
                    Name = "Nestle",
                    Products = new Dictionary<int, Product>{
                        {
                            1,
                            new Product
                            {
                                Id = 1,
                                Name = "Kit-Kat",
                                Cost = 1.45M
                            }
                        },
                        {
                            2,
                            new Product
                            {
                                Id = 2,
                                Name = "Nesquik",
                                Cost = 1.55M
                            }
                        }
                    }
                }
            };

            foreach(var company in companies)
            {
                _companies.Add(company.Id, company);
            }
        }

        private PurchaseDao BuildPurchase()
        {
            Random random = new Random();

            int companyId = random.Next(1, _companies.Count);
            Company company = _companies[companyId];

            int productId = random.Next(2, company.Products.Count);
            Product product = company.Products[productId];

            string date = $"{DateTime.UtcNow:dd-MMM-yyyy}";

            return new PurchaseDao 
            { 
                Id = $"{Guid.NewGuid()}",
                Company = company.Name,
                ProductName = product.Name,
                Cost = product.Cost,
                Date = date 
            };
        }
    }
}
