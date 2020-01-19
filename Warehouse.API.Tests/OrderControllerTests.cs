using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Warehouse.API.Controllers;
using Warehouse.Infrastructure.Data;

namespace Warehouse.API.Tests
{
    public class Tests
    {
        [Test]
        public async Task GetAsync()
        {
            var dbContext = await GetDatabaseContext();
            var orderController = new OrderController(dbContext);           
            
            //Act
            var orders = await orderController.GetOrder();
            //Assert
            Assert.NotNull(orders.Value);            
            Assert.Pass();
        }

        private async Task<WarehouseDBContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<WarehouseDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new WarehouseDBContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Order.CountAsync() <= 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    databaseContext.Order.Add(new Infrastructure.Entities.Order()
                    {
                        Id = i,
                        Sku = "Test"+i.ToString(),
                        Modified = DateTime.Now.AddDays(-1),
                        Status = 1,
                        Quantity = i*2,
                        UserId = 1

                    }) ;
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
    }

}