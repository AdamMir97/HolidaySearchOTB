using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OTBHolidaySearch.Models
{
    public class Flight
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("airline")]
        public string Airline { get; set; }

        [JsonPropertyName("from")]
        public string DepartingFrom { get; set; }

        [JsonPropertyName("to")]
        public string DestinationName { get; set; }

        [JsonPropertyName("price")]
        public decimal Cost { get; set; }

        [JsonPropertyName("departure_date")]
        public DateTime DepartureDate { get; set; }

    }
}
