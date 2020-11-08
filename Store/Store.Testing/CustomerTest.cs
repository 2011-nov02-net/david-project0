using System;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class CustomerTest
    {
        [Fact]
        public void CustomerFirstName_NonEmptyValue_StoreCorrectly()
        {
            string randomFirstName = "Sherlock";
            Customer cust = new Customer();
            cust.FirstName = randomFirstName;

            Assert.Equal(randomFirstName, cust.FirstName);
        }

        [Fact]
        public void CustomerFirstName_EmptyValue_ThrowsExecption()
        {
            Customer cust = new Customer();
            Assert.ThrowsAny<ArgumentException>(() => cust.FirstName = string.Empty);
        }

        [Fact]
        public void CustomerLastName_NonEmptyValue_StoreCorrectly()
        {
            string randomLastName = "Holmes";
            Customer cust = new Customer();
            cust.LastName = randomLastName;

            Assert.Equal(randomLastName, cust.LastName);
        }

        [Fact]
        public void CustomerLastName_EmptyValue_ThrowsExecption()
        {
            Customer cust = new Customer();
            Assert.ThrowsAny<ArgumentException>(() => cust.LastName = string.Empty);
        }

    }
}
