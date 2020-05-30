using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
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
    /// Interaction logic for HotelsList.xaml
    /// </summary>
    public partial class HotelsList : Window
    {
        private List<Hotel> ListOfHotels { get; set; }
        private Account CurrentUser { get; set; }

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointAllRoomFindByHotelId = "room/findByRoomId/";

        public HotelsList(List<Hotel> list)
        {
            ListOfHotels = list;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            listOfHotels.ItemsSource = ListOfHotels;
            CurrentUser = (Account)Application.Current.Resources["user"];
            MenuData.DataContext = CurrentUser;
            ButtonData.DataContext = CurrentUser;
        }

        private void ChooseHotel(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfHotels.SelectedIndex != i)
            {
                i++;
            }
            var hotel = ListOfHotels[i];
            var list = PrepareRoomsList(hotel.Id);
            var window = new RoomsList(list, hotel.Id);
            Close();
            window.Show();
        }

        private List<Room> PrepareRoomsList(int hotelId)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointAllRoomFindByHotelId + hotelId.ToString());
                    var roomResponse = JsonConvert.DeserializeObject<List<RoomByHotelIdResponse>>(response);
                    var list = new List<Room>();
                    var roomIds = new BsonArray();
                    foreach (var item in roomResponse)
                    {
                        var room = new Room(item.id, item.roomName, item.roomDescription, ImageConversion(item.roomImagePath), item.roomPrice);
                        list.Add(room);
                        roomIds.Add(item.id);
                    }

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("user");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie pokoi" },
                        { "Content", "Pobieranie pokoi zakończono pomyślnie" },
                        { "Context", "HotelsList" },
                        { "Data",  new BsonDocument
                            {
                                { "HotelId", hotelId },
                                { "RoomIds", roomIds }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    return list;
                }
                catch (Exception e)
                {

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie pokoi" },
                        { "Content", e.Message },
                        { "Context", "HotelsList" },
                        { "Data",  new BsonDocument
                            {
                                { "HotelId", hotelId },
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(e.Message, "Pobranie pokoi", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
