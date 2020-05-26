namespace Contracts.WebClientModels.Responses
{
    public class RoomByHotelIdResponse
    {
        public RoomByHotelIdResponse(
          int id,
          HotelResponse hotelId,
          AssortmentResponse assortmentId,
          string roomName,
          string roomDescription,
          int roomQuantityOfPeople,
          double roomPrice,
          string roomImagePath)
        {
            this.id = id;
            this.hotelId = hotelId;
            this.assortmentId = assortmentId;
            this.roomName = roomName;
            this.roomDescription = roomDescription;
            this.roomQuantityOfPeople = roomQuantityOfPeople;
            this.roomPrice = roomPrice;
            this.roomImagePath = roomImagePath;
        }

        public int id { get; set; }
        public HotelResponse hotelId { get; set; }
        public AssortmentResponse assortmentId { get; set; }
        public string roomName { get; set; }
        public string roomDescription { get; set; }
        public int roomQuantityOfPeople { get; set; }
        public double roomPrice { get; set; }
        public string roomImagePath { get; set; }  
    }
}
