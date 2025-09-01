using OTBHolidaySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OTBHolidaySearch.Repositories
{
    public class JsonHotelRepository : IRepository<Hotel>
    {
        private readonly string _filePath;

        public JsonHotelRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<Hotel> GetAll()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Hotel>>(json);
        }
    }
}
