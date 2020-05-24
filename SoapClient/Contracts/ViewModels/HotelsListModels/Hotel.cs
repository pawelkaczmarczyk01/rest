using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Contracts.ViewModels.HotelsListModels
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }

        public Hotel(int id, string name, byte[] imageData)
        {
            Id = id;
            Name = name;
            Image = LoadImage(imageData);
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
