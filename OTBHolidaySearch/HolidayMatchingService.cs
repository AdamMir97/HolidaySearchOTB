using OTBHolidaySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBHolidaySearch
{
    public interface IMatchingService
    {
        IEnumerable<(Flight, Hotel)> Match(IEnumerable<Flight> flights, IEnumerable<Hotel> hotels, string departingFrom, DateTime departureDate, int duration);
    }
    public class HolidayMatchingService : IMatchingService
    {
        public IEnumerable<(Flight, Hotel)> Match(IEnumerable<Flight> flights, IEnumerable<Hotel> hotels, string departingFrom, DateTime departureDate, int duration)
        {
            return from flight in flights
                   from hotel in hotels
                   where departingFrom == flight.DepartingFrom &&
                         hotel.LocalAirports.Contains(flight.DestinationName) &&
                         departureDate == flight.DepartureDate.Date &&
                         hotel.ArrivalDate == flight.DepartureDate.Date &&
                         hotel.LengthOfStay == duration
                   let packageCost = flight.Cost + hotel.TotalCost
                   orderby packageCost
                   select (flight, hotel);
        }

    }
}
