using System;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class ProductTest
    {
        [Fact]
        public void CreateProduct_NonEmptyValues()
        {
            string name = "Batteries";
            int id = 1;
            decimal price = 10.0m;
            string description = "AA Batteries";
            int orderLimit = 5;
            Product product = new Product(name, id, price, description, orderLimit);

            Assert.Equal(name, product.Name);
            Assert.Equal(id, product.Id);
            Assert.Equal(price, product.Price);
            Assert.Equal(description, product.Description);
            Assert.Equal(orderLimit, product.OrderLimit);
        }
    }
}
