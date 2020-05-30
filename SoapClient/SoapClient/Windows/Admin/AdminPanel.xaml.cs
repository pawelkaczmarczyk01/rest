using Contracts.Models;
using Contracts.ViewModels.Admin;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using Contracts.WebClientModels.Requests;
using Contracts.WebClientModels.Responses;
using Microsoft.Win32;
using Newtonsoft.Json;
using SoapClient.Windows.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Contracts.LogModels.AdminPanel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SoapClient.Windows.Admin
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private List<User> UserList { get; set; }
        private List<Hotel> HotelsList { get; set; }
        private List<ReservationByUser> ReservationsListByUser { get; set; }
        private List<Reservation> ReservationsList { get; set; }
        private Account CurrentUser { get; set; }
        private RoomDetails Room { get; set; }
        private RoomDetails ReservationRoom { get; set; }
        private int? SelectedUserId { get; set; }
        private int? SelectedReservationId { get; set; }
        private string ImageHotelPathWithName { get; set; }
        private string ImageHotelPath { get; set; }
        private string ImageRoomPathWithName { get; set; }
        private string ImageRoomPath { get; set; }

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointReservation = "reservation/findAll";
        private readonly string EndpointFindRoomById = "room/findById/";
        private readonly string EndpointReservationByUserId = "reservation/findByRoomId/";
        private readonly string EndpointFindUserById = "user/findById/";
        private readonly string EndpointDeleteUserById = "user/delete/";
        private readonly string EndpointAllRoomFindByHotelId = "room/findByRoomId/";
        private readonly string EndpointGetAllUsers = "user/findAll";
        private readonly string EndpointGetAllHotels = "hotel/findAll";
        private readonly string EndpointAddHotel = "hotel/add";
        private readonly string EndpointAddRoom = "room/add";

        public AdminPanel(List<User> users, List<Hotel> hotels)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            HotelsList = hotels;
            UserList = users;
            listOfUsers.ItemsSource = UserList;
            CurrentUser = (Account)Application.Current.Resources["user"];
            UserList.Remove(UserList.Find(a => a.IsAdmin));
            MenuData.DataContext = CurrentUser;
            ButtonData.DataContext = CurrentUser;
            BorderBox.DataContext = null;
            listOfHotels.ItemsSource = HotelsList;
            ReservationsList = PrepareReservationsList();
            listOfReservations.ItemsSource = ReservationsList;
            HotelsSelector.ItemsSource = HotelsList;
        }

        private List<Reservation> PrepareReservationsList()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointReservation);
                    var reservations = JsonConvert.DeserializeObject<List<ReservationResponse>>(response);
                    var bsonDocument = new BsonDocument();
                    var bsonList = new List<ReservationDB>();
                    var list = new List<Reservation>();
                    foreach (var item in reservations)
                    {
                        var response2 = client.DownloadString(BaseAddress + EndpointFindUserById + item.userId.id.ToString());
                        var responseUser = JsonConvert.DeserializeObject<LoginResponse>(response2);

                        var reservation =
                            new Reservation(
                            responseUser.userName,
                            responseUser.userLastName,
                            item.id, item.roomId.id,
                            item.reservationFrom,
                            item.roomReservationTo);
                        list.Add(reservation);
                        bsonList.Add(new ReservationDB(item.id, item.userId.id, item.roomId.id));
                    }
                    bsonDocument.Add("Reservations", new BsonArray(bsonList.Select(i => i.ToBsonDocument())));
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie rezerwacji" },
                        { "Content", "Pobieranie rezerwacji zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data", bsonDocument },
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
                        { "Title", "Pobranie rezerwacji" },
                        { "Content", ex.Data.ToString() },
                        { "Context", "AdminPanel" },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Data.ToString(), "Błąd pobrania rezerwacji", MessageBoxButton.OK);
                    return new List<Reservation>();
                }
            }
        }

        private void ChooseReservationByUser(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfReservationsByUser.SelectedIndex != i)
            {
                i++;
            }
            var reservation = ReservationsListByUser[i];
            Room = SetRoomByReservation(reservation.RoomId);
            RoomDetailsGrid.DataContext = Room;
            BorderBox.Visibility = Visibility.Visible;
            SelectedReservationId = reservation.ReservationId;
        }

        private RoomDetails SetRoomByReservation(int roomId)
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
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie szczegółów pokoju" },
                        { "Content", "Pobieranie szczegółów pokoju zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data",  new BsonDocument
                            {
                                { "RoomId", roomId },
                                { "HotelId", roomResponse.hotelId.id },
                                { "RoomName", roomResponse.roomName }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    return room;
                }
                catch (Exception e)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie szczegółów pokoju" },
                        { "Content", e.Message },
                        { "Context", "AdminPanel" },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(e.Message, "Pobranie szczegółów pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        private void ChooseUser(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfUsers.SelectedIndex != i)
            {
                i++;
            }
            var user = UserList[i];
            SelectedUserId = user.UserId;
            SetReservationsByUser(user.UserId);
            SelectedReservationId = null;
        }

        private void SetReservationsByUser(int userId)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointReservationByUserId + userId.ToString());
                    var reservationResponse = JsonConvert.DeserializeObject<List<ReservationResponse>>(response);
                    var list = new List<ReservationByUser>();
                    var reservationsIds = new BsonArray();
                    foreach (var item in reservationResponse)
                    {
                        var reservation = new ReservationByUser(item.id, item.roomId.id, item.reservationFrom, item.roomReservationTo);
                        list.Add(reservation);
                        reservationsIds.Add(item.id);
                    }
                    ReservationsListByUser = list;
                    listOfReservationsByUser.ItemsSource = ReservationsListByUser;
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie rezerwacji" },
                        { "Content", "Pobieranie rezerwacji zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data",  new BsonDocument
                            {
                                { "UserId", userId },
                                { "ReservationIds", reservationsIds }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                }
                catch (Exception e)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie rezerwacji" },
                        { "Content", e.Message },
                        { "Context", "AdminPanel" },
                        { "Data",  new BsonDocument
                            {
                                { "UserId", userId }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show("Wystąpił problem z pobraniem rezerwacji wybranego użytkownika", "Pobranie rezerwacji", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (SelectedUserId == null)
            {
                MessageBox.Show("Wybierz najpierw użytkownika", "Nie wybrano użytkownika", MessageBoxButton.OK);
                return;
            }
            MessageBoxResult boxResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć użytkownika?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxResult == MessageBoxResult.No)
            {
                return;
            }
            var user = UserList.Find(u => u.UserId == SelectedUserId);
            using (var client = new HttpClient())
            {
                try
                {
                    var task = client.DeleteAsync(BaseAddress + EndpointDeleteUserById + user.UserId.ToString());
                    task.Wait();
                    var result = task.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        UserList.Remove(user);
                        listOfUsers.ItemsSource = UserList;
                        listOfUsers.Items.Refresh();
                        SelectedUserId = null;
                        const string connectionString = "mongodb://localhost:27017";
                        var clientDB = new MongoClient(connectionString);
                        var database = clientDB.GetDatabase("project");
                        var collection = database.GetCollection<BsonDocument>("admin");
                        var document = new BsonDocument
                        {
                            { "Title", "Usuwanie użytkownika" },
                            { "Content", "Usuwanie użytkownika zakończono pomyślnie" },
                            { "Context", "AdminPanel" },
                            { "Data",  new BsonDocument
                                {
                                    { "UserId", user.UserId },
                                }
                            },
                            { "Date", DateTime.Now }
                        };
                        collection.InsertOne(document);
                        MessageBox.Show("Usunięto poprawnie użytkownika", "Usuwanie użytkownika", MessageBoxButton.OK);
                    }
                    else
                    {
                        const string connectionString = "mongodb://localhost:27017";
                        var clientDB = new MongoClient(connectionString);
                        var database = clientDB.GetDatabase("project");
                        var collection = database.GetCollection<BsonDocument>("errors");
                        var document = new BsonDocument
                        {
                            { "Title", "Usuwanie użytkownika" },
                            { "Content", "Usuwanie użytkownika anulowano. Nieznany błąd." },
                            { "Context", "AdminPanel" },
                            { "Data",  new BsonDocument
                                {
                                    { "UserId", user.UserId },
                                }
                            },
                            { "Date", DateTime.Now }
                        };
                        collection.InsertOne(document);
                        MessageBox.Show("Nieznany błąd.", "Usuwanie anulowane", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Usuwanie użytkownika" },
                        { "Content", ex.Message },
                        { "Context", "AdminPanel" },
                        { "Data",  new BsonDocument
                            {
                                { "UserId", user.UserId },
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Usuwanie użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChooseHotel(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfHotels.SelectedIndex != i)
            {
                i++;
            }
            var hotel = HotelsList[i];
            var list = PrepareRoomsList(hotel.Id);
            var window = new AdminRoomsList(list, hotel.Id);
            window.ShowDialog();
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
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie pokoi" },
                        { "Content", "Pobieranie pokoi zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
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
                        { "Context", "AdminPanel" },
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

        private void Refresh(object sender, MouseButtonEventArgs e)
        {
            HotelsList = PrepareHotelsList();
            UserList = PrepareUsers();
            listOfUsers.ItemsSource = UserList;
            listOfUsers.Items.Refresh();
            listOfHotels.ItemsSource = HotelsList;
            listOfHotels.Items.Refresh();
            HotelsSelector.ItemsSource = HotelsList;
            HotelsSelector.Items.Refresh();
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
                        { "Context", "AdminPanel" },
                        { "Data", new BsonDocument
                            {
                                { "HotelIds", hotelsId }
                            }                        },
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
                        { "Context", "AdminPanel" }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Data.ToString(), "Pobranie hoteli", MessageBoxButton.OK);
                    return new List<Hotel>();
                }
            }
        }

        private List<User> PrepareUsers()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var response = client.DownloadString(BaseAddress + EndpointGetAllUsers);
                    var user = JsonConvert.DeserializeObject<List<LoginResponse>>(response);
                    var userIds = new BsonArray();
                    var list = new List<User>();
                    foreach (var item in user)
                    {
                        var newUser = new User(item.id, item.userName, item.userLastName);
                        if (newUser.UserId == 1 && newUser.Name.Equals("admin"))
                        {
                            newUser.IsAdmin = true;
                        }
                        list.Add(newUser);
                        userIds.Add(item.id);
                    }

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Pobranie użytkowników" },
                        { "Content", "Pobieranie użytkowników zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data", new BsonDocument
                            {
                                { "UserIds", userIds }
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
                        { "Title", "Pobranie użytkowników" },
                        { "Content", e.Message },
                        { "Context", "AdminPanel" }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(e.Message, "Pobrania użytkowników", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        private void ChooseReservation(object sender, MouseButtonEventArgs e)
        {
            int i = 0;
            while (listOfReservations.SelectedIndex != i)
            {
                i++;
            }
            var reservation = ReservationsList[i];
            ReservationRoom = SetRoomByReservation(reservation.RoomId);
            ReservationDetailsGrid.DataContext = ReservationRoom;
            ReservationBorderBox.Visibility = Visibility.Visible;
        }

        private void HotelNameEnter(object sender, RoutedEventArgs e)
        {
            if (HotelName.Text == "Podaj nazwę hotelu")
            {
                HotelName.Text = "";
                HotelName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void HotelNameLeave(object sender, RoutedEventArgs e)
        {
            if (HotelName.Text == "" || HotelName.Text == null)
            {
                HotelName.Text = "Podaj nazwę hotelu";
                HotelName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
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

        private void AddHotelImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PNG (*.png)|*.png|" +
                "JPEG (*.jpeg)|*.jpeg|" +
                "JPG (*.jpg)|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                ImageHotelPathWithName = dialog.FileName;
                var fileName = ImageHotelPathWithName.Split('\\').Last();
                ImageHotelPath = ImageHotelPathWithName.Substring(0, ImageHotelPathWithName.Length - fileName.Length - 1);
                HotelImageName.Text = fileName;
                AddHotelButton.IsEnabled = true;
                HotelImageName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void AddHotel(object sender, RoutedEventArgs e)
        {
            if (HotelName.Text == "" || HotelName.Text == null || HotelName.Text.Equals("Podaj nazwę hotelu"))
            {
                const string connectionString = "mongodb://localhost:27017";
                var clientDB = new MongoClient(connectionString);
                var database = clientDB.GetDatabase("project");
                var collection = database.GetCollection<BsonDocument>("errors");
                var document = new BsonDocument
                {
                    { "Title", "Dodawanie hotelu" },
                    { "Content", "Podaj nazwę hotelu" },
                    { "Context", "AdminPanel" },
                    { "Date", DateTime.Now }
                };
                collection.InsertOne(document);
                MessageBox.Show("Podaj nazwę hotelu", "Dodawanie hotelu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveImage(ImageHotelPath, HotelImageName.Text);

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var hotelToAddRequest = new HotelToAddRequest(HotelName.Text, HotelImageName.Text);
                    var request = JsonConvert.SerializeObject(hotelToAddRequest);
                    client.UploadString(BaseAddress + EndpointAddHotel, request);

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Dodawanie hotelu" },
                        { "Content", "Dodawanie hotelu zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data", new BsonDocument
                            {
                                { "HotelName", HotelName.Text },
                                { "HotelImage", HotelImageName.Text }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                }
                catch (Exception ex)
                {

                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Dodawanie hotelu" },
                        { "Content", ex.Message },
                        { "Context", "AdminPanel" },
                        { "Data", new BsonDocument
                            {
                                { "HotelName", HotelName.Text },
                                { "HotelImage", HotelImageName.Text }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Dodawanie hotelu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            HotelName.Text = "Podaj nazwę hotelu";
            HotelName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            HotelImageName.Text = "Nie dodano zdjęcia";
            AddHotelButton.IsEnabled = false;
            HotelImageName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("IndianRed"));
            HotelsSelector.Items.Refresh();
            MessageBox.Show("Dodano hotel prawidłowo.", "Dodawanie hotelu", MessageBoxButton.OK);
        }

        private void SaveImage(string imagePath, string imageName)
        {
            var resourcePath = Application.Current.Resources["resources"];
            string[] filePaths = Directory.GetFiles(imagePath);

            foreach (var fileName in filePaths)
            {
                if (fileName.Contains(imageName))
                {
                    if(File.Exists(resourcePath + "\\" + imageName))
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
                const string connectionString = "mongodb://localhost:27017";
                var clientDB = new MongoClient(connectionString);
                var database = clientDB.GetDatabase("project");
                var collection = database.GetCollection<BsonDocument>("errors");
                var document = new BsonDocument
                {
                    { "Title", "Dodawanie pokoju" },
                    { "Content", "Podaj nazwę pokoju" },
                    { "Context", "AdminPanel" },
                    { "Date", DateTime.Now }
                };
                collection.InsertOne(document);
                MessageBox.Show("Podaj nazwę pokoju", "Nie podano nazwy pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (WebClient client = new WebClient())
            {
                try
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.Encoding = System.Text.Encoding.UTF8;
                    var roomToAddRequest = new RoomToAddRequest();
                    roomToAddRequest.roomName = RoomName.Text;
                    roomToAddRequest.roomDescription = RoomDescription.Text;
                    roomToAddRequest.roomImagePath = RoomImageName.Text;

                    double price = 0;
                    if (double.TryParse(RoomPrice.Text, out price))
                    {
                        roomToAddRequest.roomPrice = price;
                    }
                    else
                    {
                        var collection1 = database.GetCollection<BsonDocument>("errors");
                        var document1 = new BsonDocument
                        {
                            { "Title", "Dodawanie pokoju" },
                            { "Content", "Podany niepoprawny format ceny" },
                            { "Context", "AdminPanel" },
                            { "Date", DateTime.Now }
                        };
                        collection1.InsertOne(document1);
                        MessageBox.Show("Podany niepoprawny format ceny", "Dodawanie pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int quantityOfPeople = 0;
                    if (int.TryParse(RoomQuantityOfPeople.Text, out quantityOfPeople))
                    {
                        roomToAddRequest.roomQuantityOfPeople = quantityOfPeople;
                    }
                    else
                    {
                        var collection2 = database.GetCollection<BsonDocument>("errors");
                        var document2 = new BsonDocument
                        {
                            { "Title", "Dodawanie pokoju" },
                            { "Content", "Podany niedopuszczalne znaki przy liczbie osób" },
                            { "Context", "AdminPanel" },
                            { "Date", DateTime.Now }
                        };
                        collection2.InsertOne(document2);
                        MessageBox.Show("Podany niedopuszczalne znaki przy liczbie osób", "Dodawanie pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var index = HotelsSelector.SelectedIndex;
                    roomToAddRequest.hotelId = HotelsList.Find(h => h.Name == HotelsList.ElementAt(index).Name).Id;
                    roomToAddRequest.roomBathroom = Bathroom.IsChecked;
                    roomToAddRequest.roomDesk = Desk.IsChecked;
                    roomToAddRequest.roomFridge = Fridge.IsChecked;
                    roomToAddRequest.roomSafe = Safe.IsChecked;
                    roomToAddRequest.roomTv = Tv.IsChecked;

                    SaveImage(ImageRoomPath, RoomImageName.Text);


                    client.Encoding = System.Text.Encoding.UTF8;
                    var request = JsonConvert.SerializeObject(roomToAddRequest);
                    client.UploadString(BaseAddress + EndpointAddRoom, request);

                    var collection = database.GetCollection<BsonDocument>("admin");
                    var document = new BsonDocument
                    {
                        { "Title", "Dodawanie pokoju" },
                        { "Content", "Dodawanie pokoju zakończono pomyślnie" },
                        { "Context", "AdminPanel" },
                        { "Data", new BsonDocument
                            {
                                { "Name", roomToAddRequest.roomName },
                                { "Description", roomToAddRequest.roomDescription },
                                { "Image", roomToAddRequest.roomImagePath },
                                { "HotelId", roomToAddRequest.hotelId },
                                { "Bathroom", roomToAddRequest.roomBathroom },
                                { "Desk", roomToAddRequest.roomDesk },
                                { "Fridge", roomToAddRequest.roomFridge },
                                { "Safe", roomToAddRequest.roomSafe },
                                { "Tv", roomToAddRequest.roomTv }
                            }
                        },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);

                    MessageBox.Show("Dodano nowy pokój", "Dodawanie pokoju", MessageBoxButton.OK, MessageBoxImage.Information);
                    RoomName.Text = "";
                    RoomDescription.Text = "";
                    RoomImageName.Text = "";
                    RoomPrice.Text = "";
                    RoomQuantityOfPeople.Text = "";
                    Bathroom.IsChecked = false;
                    Desk.IsChecked = false;
                    Fridge.IsChecked = false;
                    Safe.IsChecked = false;
                    Tv.IsChecked = false;
                    AddRoomButton.IsEnabled = false;

                }
                catch (Exception ex)
                {
                    const string connectionString = "mongodb://localhost:27017";
                    var clientDB = new MongoClient(connectionString);
                    var database = clientDB.GetDatabase("project");
                    var collection = database.GetCollection<BsonDocument>("errors");
                    var document = new BsonDocument
                    {
                        { "Title", "Dodawanie pokoju" },
                        { "Content", ex.Message },
                        { "Context", "AdminPanel" },
                        { "Date", DateTime.Now }
                    };
                    collection.InsertOne(document);
                    MessageBox.Show(ex.Message, "Dodawanie pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }          
        }

        //private void DeleteReservation(object sender, MouseButtonEventArgs e)
        //{
        //    if (SelectedReservationId == null)
        //    {
        //        MessageBox.Show("Wybierz najpierw reserwację", "Nie wybrano reserwacji", MessageBoxButton.OK);
        //        return;
        //    }
        //    MessageBoxResult boxResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć rezerwację?", "Potwierdzenie", MessageBoxButton.YesNo);
        //    if (boxResult == MessageBoxResult.No)
        //    {
        //        return;
        //    }
        //    var reservation = ReservationsListByUser.Find(u => u.ReservationId == SelectedReservationId);
        //    var client = new HotelsPortClient();
        //    var request = new deleteReservationByIdRequest();
        //    request.reservationId = SelectedReservationId.Value;
        //    request.userId = 1;
        //    var response = client.deleteReservationById(request);
        //    ReservationsListByUser.Remove(reservation);
        //    listOfReservationsByUser.ItemsSource = UserList;
        //    listOfReservationsByUser.Items.Refresh();
        //    SelectedReservationId = null;
        //    MessageBox.Show(response.info, "Usuwanie zakończone", MessageBoxButton.OK);
        //}
    }
}
