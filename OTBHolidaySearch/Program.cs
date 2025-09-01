using OTBHolidaySearch.Repositories;

namespace OTBHolidaySearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Run the tests to see it work

            var flightRepo = new JsonFlightRepository("JSONdata/flightdata.json");
            var hotelRepo = new JsonHotelRepository("JSONdata/hoteldata.json");

            var matcher = new HolidayMatchingService();

            var holidaySearch = new HolidaySearch(flightRepo, hotelRepo, matcher); 

            holidaySearch.Run();

            
        }
    }
}
