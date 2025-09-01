using System.Text.Json;
using OTBHolidaySearch;
using OTBHolidaySearch.Models;
using OTBHolidaySearch.Repositories;

namespace HolidaySearchTests
{
    public class HolidaySearchUnitTest
    {
        //Tests to prove model works
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

        //Tests for proving JSON is loaded correctly
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


        //Tests for matching
        //Basic matching
        [Fact]
        public void FlightDestinationMatchesWithHotelAirport()
        {
            //Arrange- use Cyprus for the sake of argument
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "PFO" }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, LengthOfStay = 1 },
                new Hotel { Name = "TestHotel2", LocalAirports = new[] { "LCA"}, LengthOfStay = 1 }
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels, 1).ToList();
            //Assert
            Assert.Equal(1, matches[0].Item1.Id);
            Assert.Equal("TestHotel1", matches[0].Item2.Name);
        }

        [Fact]
        public void FlightDestinationNoMatchesWithHotelAirport_ReturnsEmpty()
        {
            //Arrange
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "AYT" }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, LengthOfStay = 1 },
                new Hotel { Name = "TestHotel2", LocalAirports = new[] { "LCA"}, LengthOfStay = 1 }
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels, 1).ToList();
            //Assert
            Assert.Empty(matches);
        }
        //Matching with dates
        [Fact]
        public void FlightDestinationMatchesWithHotelAirportAndDates()
        {
            //Arrange
            var flightDate = new DateTime(2025, 09, 01);
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "PFO", DepartureDate = flightDate }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, ArrivalDate = flightDate.AddDays(20) , LengthOfStay = 1},
                new Hotel { Name = "TestHotel2", LocalAirports = new[] { "LCA"}, ArrivalDate = flightDate , LengthOfStay = 1},
                new Hotel { Name = "TestHotel3", LocalAirports = new[] { "PFO"}, ArrivalDate = flightDate , LengthOfStay = 1}
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels, 1).ToList();
            //Assert that the departure date is the same as arrival date
            Assert.Equal(1, matches[0].Item1.Id);
            Assert.Equal("TestHotel3", matches[0].Item2.Name);
        }

        [Fact]
        public void FlightDestinationMatchesWithHotelAirportAndWrongDates_ReturnsEmpty()
        {
            //Arrange
            var flightDate = new DateTime(2025, 09, 01);
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "PFO", DepartureDate = flightDate }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, ArrivalDate = flightDate.AddDays(20), LengthOfStay = 1 }
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels , 1).ToList();
            //Assert
            Assert.Empty(matches);
        }
        //Matching with duration
        [Fact]
        public void FlightDestinationMatchesWithHotelAirportAndDatesAndDuration()
        {
            //Arrange
            int duration = 10;
            var flightDate = new DateTime(2025, 09, 01);
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "PFO", DepartureDate = flightDate }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, ArrivalDate = flightDate.AddDays(20) },
                new Hotel { Name = "TestHotel2", LocalAirports = new[] { "PFO"}, ArrivalDate = flightDate, LengthOfStay = 7 },
                new Hotel { Name = "TestHotel3", LocalAirports = new[] { "PFO"}, ArrivalDate = flightDate, LengthOfStay = 10  }
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels, duration).ToList();
            //Assert that the correct package is chosen according to the duration
            Assert.Equal(1, matches[0].Item1.Id);
            Assert.Equal("TestHotel3", matches[0].Item2.Name);
        }

        [Fact]
        public void FlightDestinationMatchesWithHotelAirportAndDatesAndWrongDuration_ReturnsEmpty()
        {
            //Arrange
            int duration = 10;
            var flightDate = new DateTime(2025, 09, 01);
            var flights = new List<Flight>
            {
                new Flight { Id = 1, DestinationName = "PFO", DepartureDate = flightDate }
            };
            var hotels = new List<Hotel>
            {
                new Hotel { Name = "TestHotel1", LocalAirports = new[] {"PFO", "LCA"}, ArrivalDate = flightDate, LengthOfStay = 7 }
            };

            var service = new HolidayMatchingService();

            //Act
            var matches = service.Match(flights, hotels, duration).ToList();
            //Assert
            Assert.Empty(matches);
        }

        public void SearchResultsAreOrderedByValue()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}