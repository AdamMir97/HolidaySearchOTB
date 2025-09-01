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
        IEnumerable<(Flight, Hotel)> Match(IEnumerable<Flight> flights, IEnumerable<Hotel> hotels, int duration);
    }
    public class HolidayMatchingService : IMatchingService
    {
        public IEnumerable<(Flight, Hotel)> Match(IEnumerable<Flight> flights, IEnumerable<Hotel> hotels, int duration)
        {
            return from flight in flights
                   from hotel in hotels
                   where hotel.LocalAirports != null &&
                         hotel.LocalAirports.Contains(flight.DestinationName) &&
                         hotel.ArrivalDate != null &&
                         hotel.ArrivalDate == flight.DepartureDate.Date &&
                         hotel.LengthOfStay == duration
                   select (flight, hotel);
        }

    }
}
