using System;
using Store.Library;
using Xunit;
using System.Linq;

namespace Store.Testing
{
    public class Test
    {
        [Fact]
        public void SearchForCustomerByName()
        {
            //set up (I know this name is in the db for testing reasons
            string randomFirstName = "Sherlock";
            string randomLastName = "Holmes";
            Session session = new Session();

            //action
            Assert.True(session.IsCustomer(randomFirstName, randomLastName));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void SearchForCustomerById(int value)
        {
            // set up
            Session session = new Session();

            //action
            Assert.True(session.IsCustomer(value));
        }

        [Fact]
        public void SetSessionCustomer()
        {
            // set up
            Session session = new Session();
            // set session customer to id 1 which is sherlock holmes
            session.SetCurrentCustomer(1);

            Assert.Equal("Sherlock", session.CurrentCustomer.FirstName);
            Assert.Equal("Holmes", session.CurrentCustomer.LastName);
        }

        [Fact]
        public void SearchForLocationById()
        {
            // set up
            int id = 10001;
            Session session = new Session();

            //action
            Assert.True(session.IsLocation(id));
        }

        [Fact]
        public void SetSessionLocation()
        {
            //set up
            Session session = new Session();
            session.SetCurrentLocation(10001);

            Assert.Equal("Walmart", session.CurrentLocation.Name);
        }

        [Fact]
        public void AddInventoryToExistingItemInLocation()
        {
            //Set up
            Session session = new Session();
            session.SetCurrentLocation(10001);
            var beforeInventory = session.GetLocationInventory().Where(i => i.ProductObj.Id == 5001).First();
            session.AddLocationInventory(5001, 2);

            var inventory = session.GetLocationInventory().Where(i => i.ProductObj.Id == 5001).First();

            Assert.Equal(beforeInventory.Quantity, inventory.Quantity - 2);
        }

        [Fact]
        public void AddOrder()
        {
            // set up
            Session ses = new Session();
            ses.SetCurrentCustomer(1);
            ses.SetCurrentLocation(10001);

            // get old count of orders in system
            int beforeAmount = ses.AllOrders.Count;

            // add an item to the sales list
            var sale = new Sale(5001, 2);
            ses.AddSaleToOrder(sale);

            //commit the order to the db
            ses.AddOrder();

            //get new count
            int afterAmount = ses.AllOrders.Count;

            Assert.Equal(beforeAmount + 1, afterAmount);
        }

        [Fact]
        public void CheckSalesList()
        {
            // set up
            Session ses = new Session();
            ses.SetCurrentCustomer(1);
            ses.SetCurrentLocation(10001);

            // add two items to the sales list
            var sale = new Sale(5001, 2);
            ses.AddSaleToOrder(sale);
            sale = new Sale(5002, 2);
            ses.AddSaleToOrder(sale);

            Assert.Equal(2, ses.GetCurrentOrderSales().Count);
        }
    }
}
