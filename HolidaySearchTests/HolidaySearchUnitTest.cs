using System.Text.Json;
using OTBHolidaySearch.Models;
using OTBHolidaySearch.Repositories;

namespace HolidaySearchTests
{
    public class HolidaySearchUnitTest
    {
        [Fact]
        public void DeserializeJSONToFlight()
        {
            //Arrange: sample json following the structure of the json file
            string json = @"[
               {
                ""id"": 1,
                ""airline"": ""First Class Air"",
                ""from"": ""MAN"",
                ""to"": ""TFS"",
                ""price"": 470,
                ""departure_date"": ""2023-07-01""
                }
             ]";

            //Act
            var flights = JsonSerializer.Deserialize<List<Flight>>(json);

            //Assert: verify mappings
            Assert.NotNull(flights);
            Assert.Single(flights);

            var flight = flights[0];
            Assert.Equal(1, flight.Id);
            Assert.Equal("MAN", flight.DepartingFrom);
            Assert.Equal("TFS", flight.DestinationName);
            Assert.Equal(470, flight.Cost);
            Assert.Equal(new DateTime(2023, 07, 01, 0, 0, 0), flight.DepartureDate);
            
        }

        [Fact]
        public void DeserializeJSONToHotel()
        {
            //Arrange: sample json following the structure of the json file
            string json = @"[
               {
                ""id"": 1,
                ""name"": ""Iberostar Grand Portals Nous"",
                ""arrival_date"": ""2022-11-05"",
                ""price_per_night"": 100,
                ""local_airports"": [ ""TFS"" ],
                ""nights"": 7
                }
             ]";

            //Act
            var hotels = JsonSerializer.Deserialize<List<Hotel>>(json);

            //Assert: verify mappings
            Assert.NotNull(hotels);
            Assert.Single(hotels);

            var hotel = hotels[0];
            Assert.Equal(1, hotel.Id);
            Assert.Equal("Iberostar Grand Portals Nous", hotel.Name);
            Assert.Equal(new DateTime(2022, 11, 05, 0, 0, 0), hotel.ArrivalDate);
            Assert.Equal(100, hotel.PricePerNight);
            Assert.Equal(7, hotel.LengthOfStay);
            Assert.Equal(700, hotel.TotalCost);

            //Assert local airports
            Assert.Contains("TFS", hotel.LocalAirports);
            
        }

        [Fact]
        public void JsonFlightRepositoryReadsFile()
        {
            var repo = new JsonFlightRepository("JSONdata/flightdata.json");
            var flights = repo.GetAll();

            Assert.NotEmpty(flights);
            Assert.All(flights, f => Assert.NotNull(f.Id));
        }

        [Fact]
        public void JsonHotelRepositoryReadsFile()
        {
            var repo = new JsonHotelRepository("JSONdata/hoteldata.json");
            var hotels = repo.GetAll();

            Assert.NotEmpty(hotels);
            Assert.All(hotels, f => Assert.NotNull(f.Id));
        }

        public void FlightDestinationMatchesWithHotelAirport()
        {
            //Arrange
            //Act
            //Assert
        }

        public void FlightDestinationMatchesWithHotelAirportAndDates()
        {
            //Arrange
            //Act
            //Assert
            //Assert that the departure date is the same as arrival date
        }

        public void SearchResultsAreOrderedByValue()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}