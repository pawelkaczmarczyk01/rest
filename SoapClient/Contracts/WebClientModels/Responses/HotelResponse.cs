using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Responses
{
    public class HotelResponse
    {
        public HotelResponse(int id, string hotelName, string hotelImagePath)
        {
            this.id = id;
            this.hotelName = hotelName;
            this.hotelImagePath = hotelImagePath;
        }

        public int id { get; set; }
        public string hotelName { get; set; }
        public string hotelImagePath { get; set; }

    }
}
