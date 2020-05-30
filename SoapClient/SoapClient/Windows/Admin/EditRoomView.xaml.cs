using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using Contracts.WebClientModels.Requests;
using Contracts.WebClientModels.Responses;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SoapClient.Windows.Admin
{

    public partial class EditRoomView : Window
    {
        private RoomDetails Room { get; set; }
        private string ImageRoomPathWithName { get; set; }
        private string ImageRoomPath { get; set; }
        private List<Hotel> HotelsList { get; set; }

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointFindRoomById = "room/findById/";
        private readonly string EndpointUpdateRoom = "room/update/";
        private readonly string EndpointGetAllHotels = "hotel/findAll";

        public EditRoomView(int roomId)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointFindRoomById + roomId.ToString());
                    var roomResponse = JsonConvert.DeserializeObject<RoomByHotelIdResponse>(response);

                    Room = new RoomDetails(roomResponse.id,
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
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie szczegółów pokoju" },
                        { "Content", "Pobieranie szczegółów pokoju zakończono pomyślnie" },
                        { "Context", "EditRoomView" },
                        { "Data",  new BsonDocument
                            {
                                { "RoomId", roomId },
                                { "HotelId", Room.HotelId },
                                { "RoomName", Room.Name }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);

                    ImageRoomPath = roomResponse.roomImagePath;
                    HotelsList = PrepareHotelsList();
                    HotelsSelector.ItemsSource = HotelsList;
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
                        { "Context", "EditRoomView" },
                        { "Data",  new BsonDocument
                            {
                                { "RoomId", roomId }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Pobranie szczegółów pokoju", MessageBoxButton.OK);
                }
            }
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
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie hoteli" },
                        { "Content", "Pobieranie hoteli zakończono pomyślnie" },
                        { "Context", "EditRoomView" },
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
                        { "Context", "EditRoomView" }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Data.ToString(), "Pobranie hoteli", MessageBoxButton.OK);
                    return new List<Hotel>();
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
            const string connectionString = "mongodb://localhost:27017";
            var clientDB = new MongoClient(connectionString);
            var database = clientDB.GetDatabase("project");

            if (RoomName.Text == "" || RoomName.Text == null || RoomName.Text.Equals("Podaj nazwę pokoju"))
            {
                var collection1 = database.GetCollection<BsonDocument>("errors");
                var document1 = new BsonDocument
                {
                    { "Title", "Edycja pokoju" },
                    { "Content", "Podaj nazwę pokoju" },
                    { "Context", "EditRoomView" },
                    { "Date", DateTime.Now }
                };
                collection1.InsertOne(document1);
                MessageBox.Show("Podaj nazwę pokoju", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var roomToUpdateRequest = new RoomToUpdateRequest();
                    roomToUpdateRequest.roomName = RoomName.Text;
                    roomToUpdateRequest.roomDescription = RoomDescription.Text;
                    roomToUpdateRequest.roomImagePath = RoomImageName.Text;

                    double price = 0;
                    if (double.TryParse(RoomPrice.Text, out price))
                    {
                        roomToUpdateRequest.roomPrice = price;
                    }
                    else
                    {
                        var collection1 = database.GetCollection<BsonDocument>("errors");
                        var document1 = new BsonDocument
                        {
                            { "Title", "Edycja pokoju" },
                            { "Content", "Podany niepoprawny format ceny" },
                            { "Context", "EditRoomView" },
                            { "Date", DateTime.Now }
                        };
                        collection1.InsertOne(document1);
                        MessageBox.Show("Podany niepoprawny format ceny", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int quantityOfPeople = 0;
                    if (int.TryParse(RoomQuantityOfPeople.Text, out quantityOfPeople))
                    {
                        roomToUpdateRequest.roomQuantityOfPeople = quantityOfPeople;
                    }
                    else
                    {
                        var collection1 = database.GetCollection<BsonDocument>("errors");
                        var document1 = new BsonDocument
                        {
                            { "Title", "Edycja pokoju" },
                            { "Content", "Podano niedopuszczalne znaki przy liczbie osób" },
                            { "Context", "EditRoomView" },
                            { "Date", DateTime.Now }
                        };
                        collection1.InsertOne(document1);
                        MessageBox.Show("Podano niedopuszczalne znaki przy liczbie osób", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var index = HotelsSelector.SelectedIndex;
                    roomToUpdateRequest.hotelId = HotelsList.Find(h => h.Name == HotelsList.ElementAt(index).Name).Id;
                    roomToUpdateRequest.roomBathroom = Bathroom.IsChecked;
                    roomToUpdateRequest.roomDesk = Desk.IsChecked;
                    roomToUpdateRequest.roomFridge = Fridge.IsChecked;
                    roomToUpdateRequest.roomSafe = Safe.IsChecked;
                    roomToUpdateRequest.roomTv = Tv.IsChecked;

                    if (ImageRoomPath.Contains("\\"))
                    {
                        SaveImage(ImageRoomPath, RoomImageName.Text);
                    }

                    var request = JsonConvert.SerializeObject(roomToUpdateRequest);
                    client.UploadString(BaseAddress + EndpointUpdateRoom + Room.RoomId.ToString(), WebRequestMethods.Http.Put, request);

                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Edycja pokoju" },
                        { "Content", "Edycję pokoju zakończono pomyślnie" },
                        { "Context", "EditRoomView" },
                        { "Data", new BsonDocument("Room", roomToUpdateRequest.ToBsonDocument()) },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);

                    MessageBox.Show("Zaaktulizowano pokój", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Information);

                    Close();
                }
                catch (Exception ex)
                {
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Edycja pokoju" },
                        { "Content", ex.Message },
                        { "Context", "EditRoomView" },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }
    }
}
