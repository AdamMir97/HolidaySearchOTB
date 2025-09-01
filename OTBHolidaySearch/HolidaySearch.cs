using OTBHolidaySearch.Models;
using OTBHolidaySearch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTBHolidaySearch
{
    public class HolidaySearch
    {
        private readonly IRepository<Flight> _flightRepo;
        private readonly IRepository<Hotel> _hotelRepo;
        private readonly HolidayMatchingService _matcher;

        public HolidaySearch(IRepository<Flight> flightRepo, IRepository<Hotel> hotelRepo, HolidayMatchingService matcher)
        {
            _flightRepo = flightRepo;
            _hotelRepo = hotelRepo;
            _matcher = matcher;
        }

        public void Run()
        {
            var flights = _flightRepo.GetAll();
            var hotels = _hotelRepo.GetAll();

            string[] departingFrom = {"MAN" };
            var flightDate = new DateTime(2023, 07, 01);
            var duration = 7;

            var service = _matcher;

            var matches = service.Match(flights, hotels, departingFrom, flightDate, duration).ToList();

            foreach (var match in matches)
            {
                Console.WriteLine($"Total cost: {match.Item1.Cost + match.Item2.TotalCost} " + 
                                  $"\nFlight Id: {match.Item1.Id} " +
                                  $"\nDeparting from: {match.Item1.DepartingFrom} " +
                                  $"\nArriving to: {match.Item1.DestinationName} " +
                                  $"\nFlight Cost: {match.Item1.Cost} " +
                                  $"\nHotel Id: {match.Item2.Id} " +
                                  $"\nHotel Name: {match.Item2.Name} " +
                                  $"\nHotel total Cost: {match.Item2.TotalCost} "
                                  );
            }

        }
    }
}
