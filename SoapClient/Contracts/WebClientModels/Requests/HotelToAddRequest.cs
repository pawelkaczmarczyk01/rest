using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Requests
{
    public class HotelToAddRequest
    {
        public HotelToAddRequest(string hotelName, string hotelImagePath)
        {
            this.hotelName = hotelName;
            this.hotelImagePath = hotelImagePath;
        }
        public int id { get; set; } = 0;
        public string hotelName { get; set; }
        public string hotelImagePath { get; set; }
    }
}
