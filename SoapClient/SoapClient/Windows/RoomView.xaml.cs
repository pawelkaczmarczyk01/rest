using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.ReservationView;
using Contracts.ViewModels.RoomView;
using Contracts.WebClientModels.Responses;
using Newtonsoft.Json;
using SoapClient.Windows.Admin;
using SoapClient.Windows.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

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

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointAllRoomFindByHotelId = "room/findByRoomId/";
        private readonly string EndpointReservation = "reservation/findAll";

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
            if (CurrentUser.IsAdmin)
            {
                var window = new AdminRoomsList(list, HotelId);
                Close();
                window.Show();
            }
            else
            {
                var window = new RoomsList(list, HotelId);
                Close();
                window.Show();
            }
        }

        private List<Room> PrepareRoomsList()
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.DownloadString(BaseAddress + EndpointAllRoomFindByHotelId + Room.HotelId.ToString());
                    var roomResponse = JsonConvert.DeserializeObject<List<RoomByHotelIdResponse>>(response);
                    var list = new List<Room>();
                    foreach (var item in roomResponse)
                    {
                        var room = new Room(item.id, item.roomName, item.roomDescription, ImageConversion(item.roomImagePath), item.roomPrice);
                        list.Add(room);
                    }

                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Błąd pobrania pokoi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
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

        private void ReserveRoom(object sender, RoutedEventArgs e)
        {
            var reservations = PrepareReservations();

            var window = new ReservationView(reservations, Room.RoomId);
            window.ShowDialog();
        }

        private List<ReservationVM> PrepareReservations()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.DownloadString(BaseAddress + EndpointReservation);
                    var reservations = JsonConvert.DeserializeObject<List<ReservationResponse>>(response);

                    var list = new List<ReservationVM>();
                    foreach (var item in reservations)
                    {
                        if (item.roomId.id == Room.RoomId)
                        {
                            if (item.reservationFrom.Year >= DateTime.Now.Year)
                            {
                                if (item.reservationFrom.Day >= DateTime.Now.Day)
                                {
                                    var reservation = new ReservationVM(item.id, item.roomId.id, item.reservationFrom, item.roomReservationTo);
                                    list.Add(reservation);
                                }
                            }
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd", ex.Data.ToString(), MessageBoxButton.OK);
                    return new List<ReservationVM>();
                }
            }
        }
    }
}
