using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using SoapClient.HotelSoap;
using SoapClient.Windows.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SoapClient.Windows
{
    /// <summary>
    /// Interaction logic for RoomsList.xaml
    /// </summary>
    public partial class RoomsList : Window
    {
        private List<Room> ListOfRooms { get; set; }
        private int HotelId { get; set; }
        private Account CurrentUser { get; set; }

        public RoomsList(List<Room> list, int hotelId)
        {
            InitializeComponent();
            HotelId = hotelId;
            this.Title = GetHotelName();
            ListOfRooms = list;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            listOfHotels.ItemsSource = ListOfRooms;
            CurrentUser = (Account)Application.Current.Resources["user"];
            MenuData.DataContext = CurrentUser;
            ButtonData.DataContext = CurrentUser;
            ButtonData.Visibility = CurrentUser.IsAdmin == true ? Visibility.Hidden : Visibility.Visible;
            MenuFirstTab.Visibility = CurrentUser.IsAdmin == true ? Visibility.Hidden : Visibility.Visible;
        }

        private string GetHotelName()
        {
            var client = new HotelsPortClient();
            var request = new findHotelByIdRequest();
            request.id = HotelId;
            var response = client.findHotelById(request);

            return response.hotel.hotelName;
        }

        private void ChooseRoom(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfHotels.SelectedIndex != i)
            {
                i++;
            }
            var room = ListOfRooms[i];

            var roomDetails = GetRoom(room.RoomId);

            if (roomDetails == null)
            {
                return;
            }

            var window = new RoomView(roomDetails, HotelId);
            Close();
            window.Show();
        }

        private RoomDetails GetRoom(int roomId)
        {
            try
            {
                var client = new HotelsPortClient();
                var request = new findRoomByIdRequest();
                request.id = roomId;
                var response = client.findRoomById(request);

                var room = new RoomDetails(
                    response.room.id,
                    response.room.hotelId,
                    response.room.roomName,
                    response.room.roomDescription,
                    ImageConversion(response.room.roomImagePath),
                    response.room.roomPrice,
                    response.room.roomQuantityOfPeople,
                    response.room.roomBathroom,
                    response.room.roomDesk,
                    response.room.roomFridge,
                    response.room.roomSafe,
                    response.room.roomTv);

                return room;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Błąd pobrania szczegółów pokoju", MessageBoxButton.OK);
                return null;
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

        private void Logout(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["user"] = null;
            var window = new LogIn();
            window.Show();
            Close();
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            var list = PrepareHotelsList();
            var window = new HotelsList(list);
            Close();
            window.Show();
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

        //private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("[^0-9]+");
        //    e.Handled = regex.IsMatch(e.Text);
        //}
    }
}
