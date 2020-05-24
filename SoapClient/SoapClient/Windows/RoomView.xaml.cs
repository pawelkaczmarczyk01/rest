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
using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.ReservationView;
using Contracts.ViewModels.RoomView;
using SoapClient.HotelSoap;
using SoapClient.Windows.Authorization;

namespace SoapClient.Windows
{
    /// <summary>
    /// Interaction logic for RoomView.xaml
    /// </summary>
    public partial class RoomView : Window
    {
        private RoomDetails Room { get; set; }
        private Account CurrentUser { get; set; }
        private int HotelId { get; set; }

        public RoomView(RoomDetails room, int hotelId)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Room = room;
            HotelId = hotelId;
            CurrentUser = (Account)Application.Current.Resources["user"];
            MenuData.DataContext = CurrentUser;
            ButtonData.DataContext = CurrentUser;
            RoomDetails.DataContext = Room;
            ReservationButton.Visibility = CurrentUser.IsAdmin == true ? Visibility.Hidden : Visibility.Visible;
            ButtonData.Visibility = CurrentUser.IsAdmin == true ? Visibility.Hidden : Visibility.Visible;
            MenuFirstTab.Visibility = CurrentUser.IsAdmin == true ? Visibility.Hidden : Visibility.Visible;
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
            var list = PrepareRoomsList();
            var window = new RoomsList(list, HotelId);
            Close();
            window.Show();
        }

        private List<Room> PrepareRoomsList()
        {
            var client = new HotelsPortClient();
            var request = new findAllRoomsByHotelIdRequest();
            request.hotelId = Room.HotelId;
            var response = client.findAllRoomsByHotelId(request);

            var list = new List<Room>();
            foreach (var item in response)
            {
                var room = new Room(item.id, item.roomName, item.roomDescription, ImageConversion(item.roomImagePath), item.roomPrice);
                list.Add(room);
            }
            return list;
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

        private void ReserveRoom(object sender, RoutedEventArgs e)
        {
            var reservations = PrepareReservations();

            var window = new ReservationView(reservations, Room.RoomId);
            window.ShowDialog();
        }

        private List<ReservationVM> PrepareReservations()
        {
            var client = new HotelsPortClient();
            var request = new findAllReservationsRequest();
            var response = client.findAllReservations(request);

            var list = new List<ReservationVM>();
            foreach (var item in response)
            {
                if (item.roomId == Room.RoomId)
                {
                    if (item.roomReservationFrom.Year >= DateTime.Now.Year)
                    {
                        if (item.roomReservationFrom.Day >= DateTime.Now.Day)
                        {
                            var reservation = new ReservationVM(item.id, item.roomId, item.roomReservationFrom, item.roomReservationTo);
                            list.Add(reservation);
                        }
                    }                 
                }        
            }
            return list;
        }
    }
}
