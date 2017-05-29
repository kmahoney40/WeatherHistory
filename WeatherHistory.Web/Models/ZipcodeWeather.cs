using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherHistory.Web.Models
{
    public class ZipcodeWeather
    {
        public string City { get; set; }
        public string State { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public List<HistoricalTemperature> HistoricalTemperatures { get; set; }
    
        public ZipcodeWeather()
        {
            HistoricalTemperatures = new List<HistoricalTemperature>();
        }
    }
        
    public class HistoricalTemperature
    {
        public DateTime Date { get; set; }
        public float Low { get; set; }
        public float High { get; set; }
    }

    public class ZipCodeApiResponse
    {
        public string Zipcode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}