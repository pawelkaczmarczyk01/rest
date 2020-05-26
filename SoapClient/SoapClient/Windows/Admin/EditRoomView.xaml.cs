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
                    MessageBox.Show(ex.Message, "Błąd pobrania szczegółów pokoju", MessageBoxButton.OK);
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
            if (RoomName.Text == "" || RoomName.Text == null || RoomName.Text.Equals("Podaj nazwę pokoju"))
            {
                MessageBox.Show("Podaj nazwę pokoju", "Nie podano nazwy pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
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
                        MessageBox.Show("Podany niepoprawny format ceny", "Nie prawidłowy format ceny", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int quantityOfPeople = 0;
                    if (int.TryParse(RoomQuantityOfPeople.Text, out quantityOfPeople))
                    {
                        roomToUpdateRequest.roomQuantityOfPeople = quantityOfPeople;
                    }
                    else
                    {
                        MessageBox.Show("Podany niedopuszczalne znaki przy liczbie osób", "Nie prawidłowy format liczby osób", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    MessageBox.Show("Zaaktulizowano pokój", "Edycja pokoju", MessageBoxButton.OK, MessageBoxImage.Information);

                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd edycji pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }
    }
}
