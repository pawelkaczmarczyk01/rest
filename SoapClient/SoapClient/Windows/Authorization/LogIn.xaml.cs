using Contracts.Models;
using Contracts.ViewModels.Admin;
using Contracts.ViewModels.HotelsListModels;
using Contracts.WebClientModels.Requests;
using Contracts.WebClientModels.Responses;
using Newtonsoft.Json;
using SoapClient.HotelSoap;
using SoapClient.Windows.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;

namespace SoapClient.Windows.Authorization
{

    public partial class LogIn : Window
    {
        private readonly string Resources = "F:\\git\\rest\\SoapClient\\SoapClient\\Resources";
        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointLogin = "auth/login";
        private readonly string EndpointGetAllUsers = "user/findAll";
        private readonly string EndpointGetAllHotels = "hotel/findAll";

        public LogIn()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Application.Current.Resources["resources"] = Resources;
        }


        private void Login(object sender, EventArgs e)
        {
            if (LoginTextBox.Text.Equals("") ||
                LoginTextBox.Text.Equals("Login") ||
                LoginTextBox.Text == null ||
                PasswordTextBoxP.Password.Equals("") ||
                PasswordTextBoxP.Password.Equals("Hasło") ||
                PasswordTextBoxP.Password == null)
            {
                MessageBox.Show("Błędny login lub hasło.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                var user = GetUserByLoginAndPassword(LoginTextBox.Text, PasswordTextBoxP.Password);

                if (user != null)
                {
                    if (user.IsAdmin)
                    {
                        Application.Current.Resources["user"] = user;
                        var usersList = PrepareUsers();
                        var hotelsList = PrepareHotelsList();
                        var window = new AdminPanel(usersList, hotelsList);
                        Close();
                        window.Show();
                    }
                    else
                    {
                        Application.Current.Resources["user"] = user;
                        var list = PrepareHotelsList();
                        var window = new HotelsList(list);
                        Close();
                        window.Show();
                    }
                }
            }
        }

        private Account GetUserByLoginAndPassword(string login, string password)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var request = JsonConvert.SerializeObject(new LoginRequest(login, password));
                    var response = client.UploadString(BaseAddress + EndpointLogin, request);
                    var user = JsonConvert.DeserializeObject<LoginResponse>(response);
                    var account = new Account(user.id, user.userLogin, user.userName, user.userLastName, user.isAdmin);
                    return account;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd podczas logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
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
                    MessageBox.Show("Błąd", ex.Data.ToString(), MessageBoxButton.OK);
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

        private List<User> PrepareUsers()
        {

            using (WebClient client = new WebClient())
            {
                try
                {
                    var response = client.DownloadString(BaseAddress + EndpointGetAllUsers);
                    var user = JsonConvert.DeserializeObject<List<LoginResponse>>(response);

                    var list = new List<User>();
                    foreach (var item in user)
                    {
                        var newUser = new User(item.id, item.userName, item.userLastName);
                        if (newUser.UserId == 1 && newUser.Name.Equals("admin"))
                        {
                            newUser.IsAdmin = true;
                        }
                        list.Add(newUser);
                    }

                    return list;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        private void SignUp(object sender, EventArgs e)
        {
            Registration window = new Registration();
            Close();
            window.Show();
        }

        private void LoginEnter(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Login")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void LoginLeave(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "" || LoginTextBox.Text == null)
            {
                LoginTextBox.Text = "Login";
                LoginTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void PasswordEnter(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "Hasło")
            {
                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void PasswordLeave(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBoxP.Password == "" || PasswordTextBoxP.Password == null)
            {
                PasswordTextBox.Text = "Hasło";
                PasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }
    }
}
