using OTBHolidaySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OTBHolidaySearch.Repositories
{
    public class JsonFlightRepository : IRepository<Flight>
    {
        private readonly string _filePath;

        public JsonFlightRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<Flight> GetAll()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Flight>>(json);
        }
    }
}
