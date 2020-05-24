using Contracts.Models;
using Contracts.ViewModels.Admin;
using Contracts.ViewModels.HotelsListModels;
using Contracts.ViewModels.RoomView;
using Microsoft.Win32;
using SoapClient.HotelSoap;
using SoapClient.Windows.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            var client = new HotelsPortClient();
            var requestReservation = new findAllReservationsRequest();
            var responseReservation = client.findAllReservations(requestReservation);

            var list = new List<Reservation>();
            foreach (var item in responseReservation)
            {
                var requestUser = new findUserByIdRequest();
                requestUser.id = item.userId;
                var responseUser = client.findUserById(requestUser);
                var reservation =
                    new Reservation(
                    responseUser.user.userName,
                    responseUser.user.userLastName,
                    item.id, item.roomId,
                    item.roomReservationFrom,
                    item.roomReservationTo);
                list.Add(reservation);
            }

            return list;
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
            var client = new HotelsPortClient();
            var request = new findAllReservationsByUserIdRequest();
            request.userId = userId;
            var response = client.findAllReservationsByUserId(request);
            var list = new List<ReservationByUser>();
            foreach (var item in response)
            {
                var reservation = new ReservationByUser(item.id, item.roomId, item.roomReservationFrom, item.roomReservationTo);
                list.Add(reservation);
            }
            ReservationsListByUser = list;
            listOfReservationsByUser.ItemsSource = ReservationsListByUser;
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
            var client = new HotelsPortClient();
            var request = new deleteUserByIdRequest();
            request.id = SelectedUserId.Value;
            try
            {
                var response = client.deleteUserById(request);
                UserList.Remove(user);
                listOfUsers.ItemsSource = UserList;
                listOfUsers.Items.Refresh();
                SelectedUserId = null;
                MessageBox.Show(response.info, "Usuwanie zakończone", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie można usunąć użytkownika, który posiada rezerwacje", "Usuwanie zakończone", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var client = new HotelsPortClient();
            var request = new findAllRoomsByHotelIdRequest();
            request.hotelId = hotelId;
            var response = client.findAllRoomsByHotelId(request);

            var list = new List<Room>();
            foreach (var item in response)
            {
                var room = new Room(item.id, item.roomName, item.roomDescription, ImageConversion(item.roomImagePath), item.roomPrice);
                list.Add(room);
            }

            return list;
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

        private List<User> PrepareUsers()
        {
            try
            {
                var client = new HotelsPortClient();
                var request = new findAllUsersRequest();
                var response = client.findAllUsers(request);
                var list = new List<User>();
                foreach (var item in response)
                {
                    var user = new User(item.id, item.userName, item.userLastName);
                    if (user.UserId == 1 && user.Name.Equals("admin"))
                    {
                        user.IsAdmin = true;
                    }
                    list.Add(user);
                }

                return list;
            }

            catch (Exception e)
            {
                MessageBox.Show("Błąd", e.Data.ToString(), MessageBoxButton.OK);
                return new List<User>();
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
                MessageBox.Show("Podaj nazwę hotelu", "Nie podano nazwy hotelu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveImage(ImageHotelPath, HotelImageName.Text);

            var client = new HotelsPortClient();
            var request = new addHotelRequest();
            var reqHotel = new hotelRequest();
            reqHotel.hotelName = HotelName.Text;
            reqHotel.hotelImagePath = HotelImageName.Text;
            request.hotel = reqHotel;
            var response = client.addHotel(request);

            HotelName.Text = "Podaj nazwę hotelu";
            HotelName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            HotelImageName.Text = "Nie dodano zdjęcia";
            AddHotelButton.IsEnabled = false;
            HotelImageName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("IndianRed"));
            HotelsSelector.Items.Refresh();
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
                MessageBox.Show("Podaj nazwę pokoju", "Nie podano nazwy pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var client = new HotelsPortClient();
            var request = new addRoomRequest();
            var reqRoom = new roomRequest();

            reqRoom.roomName = RoomName.Text;
            reqRoom.roomDescription = RoomDescription.Text;
            reqRoom.roomImagePath = RoomImageName.Text;

            double price = 0;
            if(double.TryParse(RoomPrice.Text, out price))
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

            SaveImage(ImageRoomPath, RoomImageName.Text);

            var response = client.addRoom(request);

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
