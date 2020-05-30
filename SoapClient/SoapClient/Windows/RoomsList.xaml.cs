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
using MongoDB.Bson;
using MongoDB.Driver;

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

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("user");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie nazwy hotelu" },
                        { "Content", "Pobieranie nazwy hotelu zakończono pomyślnie" },
                        { "Context", "RoomsList" },
                        { "Data", new BsonDocument
                            {
                                { "HotelName", hotel.hotelName }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);

                    return hotel.hotelName;
                }
                catch (Exception ex)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie nazwy hotelu" },
                        { "Content", ex.Message },
                        { "Context", "RoomsList" },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Pobranie nazwy hotelu", MessageBoxButton.OK);
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
                    client.Encoding = System.Text.Encoding.UTF8;
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

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("user");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie szczegółów pokoju" },
                        { "Content", "Pobieranie szczegółów pokoju zakończono pomyślnie" },
                        { "Context", "RoomsList" },
                        { "Data", new BsonDocument
                            {
                                { "RoomId", roomId },
                                { "HotelId", roomResponse.hotelId.id },
                                { "RoomName",  roomResponse.roomName }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    return room;
                }
                catch (Exception ex)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie szczegółów pokoju" },
                        { "Content", ex.Message },
                        { "Context", "RoomsList" },
                        { "Data", new BsonDocument
                            {
                                { "RoomId", roomId },
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Pobranie szczegółów pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointGetAllHotels);
                    var hotels = JsonConvert.DeserializeObject<List<HotelResponse>>(response);
                    var hotelsId = new BsonArray();
                    var list = new List<Hotel>();
                    foreach (var item in hotels)
                    {
                        var hotel = new Hotel(item.id, item.hotelName, ImageConversion(item.hotelImagePath));
                        list.Add(hotel);
                        hotelsId.Add(item.id);
                    }

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("user");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie hoteli" },
                        { "Content", "Pobieranie hoteli zakończono pomyślnie" },
                        { "Context", "RoomsList" },
                        { "Data", new BsonDocument
                            {
                                { "HotelIds", hotelsId }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);

                    return list;
                }
                catch (Exception ex)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie hoteli" },
                        { "Content", ex.Message },
                        { "Context", "RoomsList" }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Data.ToString(), "Pobranie hoteli", MessageBoxButton.OK);
                    return new List<Hotel>();
                }
            }
        }
    }
}
