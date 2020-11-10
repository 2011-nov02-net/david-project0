using System;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class InventoryTest
    {
        int productId = 1;
        Product prod = new Product("Battery", 1, 10.0m, "Batteries", 10);

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void AddInventory_ValueAboveZero(int value)
        {
            // using this constructor we can be assured that the 
            // starting inventory is zero.  means
            // that we can check the current quantity with
            //the value that the Test runs
            Inventory inventory = new Inventory(productId);
            inventory.AddInventory(value);

            Assert.Equal(value, inventory.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void AddInventory_ValueAtAndBelowZero(int value)
        {
            Inventory inventory = new Inventory(productId);
            Assert.ThrowsAny<ArgumentException>(() => inventory.AddInventory(value));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void SellInventory_ValueInStock_NoError(int value)
        {
            Inventory inventory = new Inventory(productId, value);
            inventory.SellInventory(value);

            Assert.Equal(0, inventory.Quantity);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void SellInventory_MoreThanInStock_Error(int value)
        {
            Inventory inventory = new Inventory(productId, value);

            Assert.ThrowsAny<ArgumentException>(() => inventory.SellInventory(value + 1));
        }

        [Fact]
        public void GetProductFromInventory_ProductInInventoryWithQuantity()
        {
            // store product
            Inventory inventory = new Inventory(prod, 10);

            // get product
            Product getProd = inventory.GetProduct();

            Assert.Equal(prod.Name, getProd.Name);
            Assert.Equal(prod.Id, getProd.Id);
            Assert.Equal(prod.Price, getProd.Price);
            Assert.Equal(prod.Description, getProd.Description);
            Assert.Equal(prod.OrderLimit, getProd.OrderLimit);
        }

        [Fact]
        public void GetProductFromInventory_ProductInInventoryWithNoQuantity()
        {
            // store product
            Inventory inventory = new Inventory(prod);

            // get product
            Product getProd = inventory.GetProduct();

            Assert.Equal(prod.Name, getProd.Name);
            Assert.Equal(prod.Id, getProd.Id);
            Assert.Equal(prod.Price, getProd.Price);
            Assert.Equal(prod.Description, getProd.Description);
            Assert.Equal(prod.OrderLimit, getProd.OrderLimit);
        }

    }
}
