using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Contracts.ViewModels.RoomView
{
    public class RoomDetails
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public string PriceToShow { get; set; }
        public double Price { get; set; }
        public int RoomQuantityOfPeople { get; set; }
        public int HotelId { get; set; }
        public bool IsBathroom { get; set; }
        public bool IsDesk { get; set; }
        public bool IsFridge { get; set; }
        public bool IsSafe { get; set; }
        public bool IsTv { get; set; }

        public RoomDetails(
            int roomId, 
            int hotelId,
            string name, 
            string description, 
            byte[] imageData, 
            double price, 
            int roomQuantityOfPeople,
            bool isBathroom,
            bool isDesk,
            bool isFridge,
            bool isSafe,
            bool isTv)
        {
            RoomId = roomId;
            HotelId = hotelId;
            Name = name;
            Description = description;
            Image = LoadImage(imageData);
            PriceToShow = "Koszt za dobę: " + price.ToString() + " zł";
            Price = price;
            RoomQuantityOfPeople = roomQuantityOfPeople;
            IsBathroom = isBathroom;
            IsDesk = isDesk;
            IsFridge = isFridge;
            IsSafe = isSafe;
            IsTv = isTv;
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();

            using (var memory = new MemoryStream(imageData))
            {
                memory.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memory;
                image.EndInit();
            }

            image.Freeze();

            return image;
        }
    }
}
