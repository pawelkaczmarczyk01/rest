using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Responses
{
    public class ReservationResponse
    {
        public ReservationResponse(
            int id,
            UserResponse userId,
            RoomByHotelIdResponse roomId, 
            DateTime reservationFrom, 
            DateTime roomReservationTo)
        {
            this.id = id;
            this.userId = userId;
            this.roomId = roomId;
            this.reservationFrom = reservationFrom;
            this.roomReservationTo = roomReservationTo;
        }

        public int id { get; set; }
        public UserResponse userId { get; set; }
        public RoomByHotelIdResponse roomId { get; set; }
        public DateTime reservationFrom { get; set; }
        public DateTime roomReservationTo { get; set; }


    }

    public class UserResponse
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string userLastName { get; set; }
        public string userLogin { get; set; }
        public string userPassword { get; set; }
        public bool isAdmin { get; set; }
    }
}
