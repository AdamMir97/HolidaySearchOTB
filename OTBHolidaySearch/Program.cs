using OTBHolidaySearch.Repositories;

namespace OTBHolidaySearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var flightRepo = new JsonFlightRepository("JSONdata/flightdata.json");
            var hotelRepo = new JsonHotelRepository("JSONdata/hoteldata.json");
        }
    }
}
