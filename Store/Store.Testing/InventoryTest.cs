﻿using System;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class InventoryTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void RestockInventory_ValueAboveZero(int value)
        {
            int productId = 1;
            // using this constructor we can be assured that the 
            // starting inventory is zero.  means
            // that we can check the current quantity with
            //the value that the Test runs
            Inventory inventory = new Inventory(productId);
            inventory.AddInventory(value);

            Assert.Equal(value, inventory.Quantity);
        }
    }
}