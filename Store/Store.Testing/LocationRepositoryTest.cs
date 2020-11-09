using System;
using System.Collections.Generic;
using Store.Library;
using Xunit;

namespace Store.Testing
{
    public class LocationRepositoryTest
    {
        public static string randomLocationName = "Wally World";

        /// <summary>
        /// Test to make sure that the repository will instatiate and be able to 
        /// save locations properly
        /// </summary>
        /// <remarks>
        /// This test works by initizaling an empty repo, add a location and ensure 
        /// that the next added location has an Id of 1.
        /// </remarks>
        [Fact]
        public void CreateLocationRepository_NoPreviousListLoaded_StoreCorrectly()
        {
            // create the repo
            LocationRepository db = new LocationRepository();
            // create and add a location
            db.AddLocation(db.CreateLocation(randomLocationName));

            // for efficeny get the location back
            Location location = db.GetLocation(1);

            Assert.Equal(1, location.Id);
            Assert.Equal(randomLocationName, location.Name);
        }

        [Fact]
        public void CreateLocationRepository_WithPreviousList_StoreCorrectly()
        {
            // create list of i locations
            List<Location> LocationList = new List<Location>();
            for (int i = 1; i < 4; i++)
            {
                Location loc = new Location(randomLocationName, i);
                LocationList.Add(loc);
            }

            // add locations to the LocationRepository db
            LocationRepository db = new LocationRepository(LocationList);

            // check each location to make sure it has the right name and id
            for(int i = 1; i < 4; i++)
            {
                // get location
                Location loc = db.GetLocation(i);
                Assert.Equal(i, loc.Id);
                Assert.Equal(randomLocationName, loc.Name);
            }
        }

        [Fact]
        public void GetLocationFromRepository_WithID()
        {
            // create repo and add location
            LocationRepository db = new LocationRepository();
            db.AddLocation(db.CreateLocation(randomLocationName));

            // get location with id 1 as it should be the first and only item in repo
            Location loc = db.GetLocation(1);

            Assert.Equal(1, loc.Id);
            Assert.Equal(randomLocationName, loc.Name);
        }

        [Fact]
        public void GetLocationFromRepository_WithName()
        {
            // create repo and add location
            LocationRepository db = new LocationRepository();
            db.AddLocation(db.CreateLocation(randomLocationName));

            // get location with id 1 as it should be the first and only item in repo
            Location loc = db.GetLocation(randomLocationName);

            Assert.Equal(1, loc.Id);
            Assert.Equal(randomLocationName, loc.Name);
        }
    }
}
