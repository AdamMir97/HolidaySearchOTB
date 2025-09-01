using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace OTBHolidaySearch.Models
{
    public class Hotel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("arrival_date")]
        public DateTime ArrivalDate { get; set; }

        [JsonPropertyName("price_per_night")]
        public decimal PricePerNight { get; set; }

        [JsonPropertyName("local_airports")]
        public string[] LocalAirports { get; set; }

        [JsonPropertyName("nights")]
        public int LengthOfStay { get; set; }
        public decimal TotalCost => PricePerNight * LengthOfStay;
    }
}
