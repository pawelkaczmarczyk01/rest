using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.WebClientModels.Responses
{
    public class RoomResponse
    {
        public RoomResponse(
            int id, 
            int hotelId, 
            int assortmentId, 
            string roomName, 
            string roomDescription, 
            int roomQuantityOfPeople, 
            double roomPrice, 
            string roomImagePath, 
            bool roomBathroom, 
            bool roomDesk, 
            bool roomFridge, 
            bool roomSafe, 
            bool roomTv)
        {
            this.id = id;
            this.hotelId = hotelId;
            this.assortmentId = assortmentId;
            this.roomName = roomName;
            this.roomDescription = roomDescription;
            this.roomQuantityOfPeople = roomQuantityOfPeople;
            this.roomPrice = roomPrice;
            this.roomImagePath = roomImagePath;
            this.roomBathroom = roomBathroom;
            this.roomDesk = roomDesk;
            this.roomFridge = roomFridge;
            this.roomSafe = roomSafe;
            this.roomTv = roomTv;
        }

        public int id { get; set; }
        public int hotelId { get; set; }
        public int assortmentId { get; set; }
        public string roomName { get; set; }
        public string roomDescription { get; set; }
        public int roomQuantityOfPeople { get; set; }
        public double roomPrice { get; set; }
        public string roomImagePath { get; set; }
        public bool roomBathroom { get; set; }
        public bool roomDesk { get; set; }
        public bool roomFridge { get; set; }
        public bool roomSafe { get; set; }
        public bool roomTv { get; set; }
    }
}
