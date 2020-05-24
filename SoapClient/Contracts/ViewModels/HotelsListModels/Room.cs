using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Contracts.ViewModels.HotelsListModels
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public BitmapImage Image { get; set; }
        public string PriceToShow { get; set; }
        public double Price { get; set; }
        public int HotelId { get; set; }

        public Room(int roomId, string name, string description, byte[] imageData, double price)
        {
            RoomId = roomId;
            Name = name;
            Description = description;
            if (description.Length > 635)
            {
                ShortDescription = description.Substring(0, 635) + "...";
            }
            Image = LoadImage(imageData);
            PriceToShow = "Koszt za dobę: " + price.ToString() + " zł";
            Price = price;
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
