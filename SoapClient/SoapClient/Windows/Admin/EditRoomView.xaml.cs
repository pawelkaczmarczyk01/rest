using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using Microsoft.Win32;
using SoapClient.HotelSoap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoapClient.Windows.Admin
{
    /// <summary>
    /// Interaction logic for EditRoomView.xaml
    /// </summary>
    public partial class EditRoomView : Window
    {
        private RoomDetails Room { get; set; }
        private string ImageRoomPathWithName { get; set; }
        private string ImageRoomPath { get; set; }
        private List<Hotel> HotelsList { get; set; }

        public EditRoomView(int roomId)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            var client = new HotelsPortClient();
            var request = new findRoomByIdRequest();
            request.id = roomId;
            var response = client.findRoomById(request);
            var room = response.room;
            Room = new RoomDetails(room.id, 
                room.hotelId, 
                room.roomName, 
                room.roomDescription, 
                ImageConversion(room.roomImagePath), 
                room.roomPrice, 
                room.roomQuantityOfPeople, 
                room.roomBathroom, 
                room.roomDesk, 
                room.roomFridge, 
                room.roomSafe, 
                room.roomTv);

            HotelsList = PrepareHotelsList();
            HotelsSelector.ItemsSource = HotelsList;
            ImageRoomPath = room.roomImagePath;

            RoomName.Text = Room.Name;
            RoomDescription.Text = Room.Description;
            RoomImageName.Text = ImageRoomPath;
            RoomPrice.Text = Room.Price.ToString();
            RoomQuantityOfPeople.Text = Room.RoomQuantityOfPeople.ToString();
            Bathroom.IsChecked = Room.IsBathroom;
            Desk.IsChecked = Room.IsDesk;
            Fridge.IsChecked = Room.IsFridge;
            Safe.IsChecked = Room.IsSafe;
            Tv.IsChecked = Room.IsTv;
        }

        private List<Hotel> PrepareHotelsList()
        {
            try
            {
                var client = new HotelsPortClient();
                var request = new findAllHotelsRequest();
                var response = client.findAllHotels(request);


                var list = new List<Hotel>();
                foreach (var item in response)
                {
                    var hotel = new Hotel(item.id, item.hotelName, ImageConversion(item.hotelImagePath));
                    list.Add(hotel);
                }

                return list;
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd", e.Data.ToString(), MessageBoxButton.OK);
                return new List<Hotel>();
            }
        }

        private byte[] ImageConversion(string imageName)
        {
            var resourcePath = Application.Current.Resources["resources"] + "\\";
            FileStream fs = new FileStream(resourcePath + imageName, FileMode.Open, FileAccess.Read);
            byte[] imgByteArr = new byte[fs.Length];
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return imgByteArr;
        }

        private void RoomNameEnter(object sender, RoutedEventArgs e)
        {
            if (RoomName.Text == "Podaj nazwę pokoju")
            {
                RoomName.Text = "";
                RoomName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RoomNameLeave(object sender, RoutedEventArgs e)
        {
            if (RoomName.Text == "" || RoomName.Text == null)
            {
                RoomName.Text = "Podaj nazwę pokoju";
                RoomName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void RoomDescriptionEnter(object sender, RoutedEventArgs e)
        {
            if (RoomDescription.Text == "Wpisz opis pokoju")
            {
                RoomDescription.Text = "";
                RoomDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RoomDescriptionLeave(object sender, RoutedEventArgs e)
        {
            if (RoomDescription.Text == "" || RoomDescription.Text == null)
            {
                RoomDescription.Text = "Wpisz opis pokoju";
                RoomDescription.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void RoomPriceEnter(object sender, RoutedEventArgs e)
        {
            if (RoomPrice.Text == "Podaj cenę za dobę")
            {
                RoomPrice.Text = "";
                RoomPrice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RoomPriceLeave(object sender, RoutedEventArgs e)
        {
            if (RoomPrice.Text == "" || RoomPrice.Text == null)
            {
                RoomPrice.Text = "Podaj cenę za dobę";
                RoomPrice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void RoomQuantityOfPeopleEnter(object sender, RoutedEventArgs e)
        {
            if (RoomQuantityOfPeople.Text == "Podaj liczbę osób")
            {
                RoomQuantityOfPeople.Text = "";
                RoomQuantityOfPeople.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RoomQuantityOfPeopleLeave(object sender, RoutedEventArgs e)
        {
            if (RoomQuantityOfPeople.Text == "" || RoomQuantityOfPeople.Text == null)
            {
                RoomQuantityOfPeople.Text = "Podaj liczbę osób";
                RoomQuantityOfPeople.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }
        private void SaveImage(string imagePath, string imageName)
        {
            var resourcePath = Application.Current.Resources["resources"];
            string[] filePaths = Directory.GetFiles(imagePath);

            foreach (var fileName in filePaths)
            {
                if (fileName.Contains(imageName))
                {
                    if (File.Exists(resourcePath + "\\" + imageName))
                    {
                        File.Delete(resourcePath + "\\" + imageName);
                    }
                    File.Copy(fileName, resourcePath + "\\" + imageName);
                }
            }
        }

        private void AddRoomImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PNG (*.png)|*.png|" +
                "JPEG (*.jpeg)|*.jpeg|" +
                "JPG (*.jpg)|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                ImageRoomPathWithName = dialog.FileName;
                var fileName = ImageRoomPathWithName.Split('\\').Last();
                ImageRoomPath = ImageRoomPathWithName.Substring(0, ImageRoomPathWithName.Length - fileName.Length - 1);
                RoomImageName.Text = fileName;
                AddRoomButton.IsEnabled = true;
                RoomImageName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void AddRoom(object sender, RoutedEventArgs e)
        {
            if (RoomName.Text == "" || RoomName.Text == null || RoomName.Text.Equals("Podaj nazwę pokoju"))
            {
                MessageBox.Show("Podaj nazwę pokoju", "Nie podano nazwy pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var client = new HotelsPortClient();
            var request = new updateRoomRequest();
            var reqRoom = new roomRequest();

            reqRoom.roomName = RoomName.Text;
            reqRoom.roomDescription = RoomDescription.Text;
            reqRoom.roomImagePath = RoomImageName.Text;

            double price = 0;
            if (double.TryParse(RoomPrice.Text, out price))
            {
                reqRoom.roomPrice = price;
            }
            else
            {
                MessageBox.Show("Podany niepoprawny format ceny", "Nie prawidłowy format ceny", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int quantityOfPeople = 0;
            if (int.TryParse(RoomQuantityOfPeople.Text, out quantityOfPeople))
            {
                reqRoom.roomQuantityOfPeople = quantityOfPeople;
            }
            else
            {
                MessageBox.Show("Podany niedopuszczalne znaki przy liczbie osób", "Nie prawidłowy format liczby osób", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var index = HotelsSelector.SelectedIndex;
            reqRoom.hotelId = HotelsList.Find(h => h.Name == HotelsList.ElementAt(index).Name).Id;
            reqRoom.roomBathroom = Bathroom.IsChecked;
            reqRoom.roomDesk = Desk.IsChecked;
            reqRoom.roomFridge = Fridge.IsChecked;
            reqRoom.roomSafe = Safe.IsChecked;
            reqRoom.roomTv = Tv.IsChecked;
            request.room = reqRoom;
            request.id = Room.RoomId;

            if (ImageRoomPath.Contains("\\"))
            {
                SaveImage(ImageRoomPath, RoomImageName.Text);
            }

            var response = client.updateRoom(request);

            MessageBox.Show("Zaaktulizowano pokój", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }
    }
}
