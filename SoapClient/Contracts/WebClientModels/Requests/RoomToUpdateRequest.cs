using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Requests
{
    public class RoomToUpdateRequest
    {
        public int hotelId { get; set; }
        public string roomName { get; set; }
        public string roomDescription { get; set; }
        public int roomQuantityOfPeople { get; set; }
        public double roomPrice { get; set; }
        public string roomImagePath { get; set; }
        public bool? roomBathroom { get; set; }
        public bool? roomDesk { get; set; }
        public bool? roomFridge { get; set; }
        public bool? roomSafe { get; set; }
        public bool? roomTv { get; set; }
    }
}
