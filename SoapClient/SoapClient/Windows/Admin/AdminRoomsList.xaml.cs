using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
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
    /// Interaction logic for AdminRoomsList.xaml
    /// </summary>
    public partial class AdminRoomsList : Window
    {
        private List<Room> ListOfRooms { get; set; }
        private int HotelId { get; set; }
        private Account CurrentUser { get; set; }

        public AdminRoomsList(List<Room> list, int hotelId)
        {
            InitializeComponent();
            HotelId = hotelId;
            this.Title = GetHotelName();
            ListOfRooms = list;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            listOfRooms.ItemsSource = ListOfRooms;
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
            while (listOfRooms.SelectedIndex != i)
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

            FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
            byte[] imgByteArr = new byte[fs.Length];
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return imgByteArr;
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

        private void DeleteRoom(object sender, RoutedEventArgs e)
        {
            if (listOfRooms.SelectedIndex == -1)
            {
                return;
            }
            MessageBoxResult boxResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć pokój?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            int i = 0;
            while (listOfRooms.SelectedIndex != i)
            {
                i++;
            }
            var room = ListOfRooms[i];
            ListOfRooms.Remove(room);
            var client = new HotelsPortClient();
            var request = new deleteRoomByIdRequest();
            request.id = room.RoomId;
            var response = client.deleteRoomById(request);
            listOfRooms.ItemsSource = ListOfRooms;
            listOfRooms.Items.Refresh();
            MessageBox.Show(response.info, "Usuwanie zakończone", MessageBoxButton.OK);
            Close();
        }

        private void EditRoom(object sender, RoutedEventArgs e)
        {
            if (listOfRooms.SelectedIndex == -1)
            {
                return;
            }
            int i = 0;
            while (listOfRooms.SelectedIndex != i)
            {
                i++;
            }
            var room = ListOfRooms[i];
            var window = new EditRoomView(room.RoomId);
            window.ShowDialog();
        }

        //private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("[^0-9]+");
        //    e.Handled = regex.IsMatch(e.Text);
        //}
    }
}
