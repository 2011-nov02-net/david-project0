using System;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class LocationTest
    {
        [Fact]
        public void LocationName_NonEmptyValue_StoreCorrectly()
        {
            string randomLocationName = "Wally World";
            Location location = new Location();
            location.Name = randomLocationName;
            Assert.Equal(randomLocationName, location.Name);
        }

        [Fact]
        public void LocationName_EmptyValue_ThrowException()
        {
            Location location = new Location();
            Assert.ThrowsAny<ArgumentException>(() => location.Name = string.Empty);
        }

        [Fact]
        public void LocationNameAndIdConstructor_NonEmptyValues()
        {
            string randomLocationName = "Wally World";
            int id = 1;
            Location location = new Location(randomLocationName, id);
            Assert.Equal(randomLocationName, location.Name);
            Assert.Equal(id, location.Id);
        }

        [Fact]
        public void LocationNameAndIdConstuctor_EmptyName()
        {
            int id = 1;
            Location location;
            Assert.ThrowsAny<ArgumentException>(() => location = new Location(string.Empty, id));
        }

        [Fact]
        public void LocationNameAndIdConstructor_NegativeId()
        {
            int id = -1;
            string randomLocationName = "Wally World";
            Location location;
            Assert.ThrowsAny<ArgumentException>(() => location = new Location(randomLocationName, id));
        }
    }
}
