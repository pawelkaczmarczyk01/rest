using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using Contracts.WebClientModels.Responses;
using Newtonsoft.Json;
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
    /// Interaction logic for RoomsList.xaml
    /// </summary>
    public partial class RoomsList : Window
    {
        private List<Room> ListOfRooms { get; set; }
        private int HotelId { get; set; }
        private Account CurrentUser { get; set; }

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointFindHotelById = "hotel/findById/";
        private readonly string EndpointFindRoomById = "room/findById/";
        private readonly string EndpointGetAllHotels = "hotel/findAll";
        private readonly string EndpointDeleteRoomById = "room/delete/";

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
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointFindHotelById + HotelId.ToString());
                    var hotel = JsonConvert.DeserializeObject<HotelResponse>(response);


                    return hotel.hotelName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Data.ToString(), "Błąd", MessageBoxButton.OK);
                    return "Hotel";
                }
            }
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
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.DownloadString(BaseAddress + EndpointFindRoomById + roomId.ToString());
                    var roomResponse = JsonConvert.DeserializeObject<RoomByHotelIdResponse>(response);

                    var room = new RoomDetails(
                        roomResponse.id,
                        roomResponse.hotelId.id,
                        roomResponse.roomName,
                        roomResponse.roomDescription,
                        ImageConversion(roomResponse.roomImagePath),
                        roomResponse.roomPrice,
                        roomResponse.roomQuantityOfPeople,
                        roomResponse.assortmentId.roomBathroom,
                        roomResponse.assortmentId.roomDesk,
                        roomResponse.assortmentId.roomFridge,
                        roomResponse.assortmentId.roomSafe,
                        roomResponse.assortmentId.roomTv);

                    return room;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd pobrania szczegółów pokoju", MessageBoxButton.OK);
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
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var response = client.DownloadString(BaseAddress + EndpointGetAllHotels);
                    var hotels = JsonConvert.DeserializeObject<List<HotelResponse>>(response);

                    var list = new List<Hotel>();
                    foreach (var item in hotels)
                    {
                        var hotel = new Hotel(item.id, item.hotelName, ImageConversion(item.hotelImagePath));
                        list.Add(hotel);
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Data.ToString(), "Błąd", MessageBoxButton.OK);
                    return new List<Hotel>();
                }
            }

        }
    }
}
