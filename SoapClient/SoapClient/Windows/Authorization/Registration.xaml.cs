using Contracts.WebClientModels.Requests;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;
using System.Windows.Media;

namespace SoapClient.Windows.Authorization
{

    public partial class Registration : Window
    {
        private readonly string Endpoint = "http://localhost:8080/";
        private readonly string EndpointAdd = "auth/register";

        public Registration()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            Close();
            logIn.Show();
        }

        private void FinishRegistration(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text.Length < 3 || LoginTextBox.Text == null || LoginTextBox.Text.Equals("Login"))
            {
                MessageBox.Show("Login musi mieć przynajmniej 3 znaki.", "Błędny login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (PasswordTextBoxP.Password.Length < 3 || PasswordTextBoxP.Password == null)
            {
                MessageBox.Show("Hasło musi mieć przynajmniej 3 znaki.", "Błędne hasło", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!RepeatPasswordTextBoxP.Password.Equals(PasswordTextBoxP.Password))
            {
                MessageBox.Show("Hasła są różne.", "Błędne hasło", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (NameTextBox.Text.Length < 3 || NameTextBox.Text == null || NameTextBox.Text.Equals("Imię"))
            {
                MessageBox.Show("Imię musi mieć przynajmniej 3 znaki.", "Błędne imię", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (SurnameTextBox.Text.Length < 3 || SurnameTextBox.Text == null || SurnameTextBox.Text.Equals("Nazwisko"))
            {
                MessageBox.Show("Nazwisko musi mieć przynajmniej 3 znaki.", "Błędne nazwisko", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                        var request = JsonConvert.SerializeObject(new RegistrationUser(NameTextBox.Text, SurnameTextBox.Text, LoginTextBox.Text, PasswordTextBoxP.Password, RepeatPasswordTextBoxP.Password));
                        client.UploadString(Endpoint + EndpointAdd, request);
                        MessageBox.Show("Konto założono poprawnie.", "Rejestracja zakończona", MessageBoxButton.OK);
                        var window = new LogIn();
                        Close();
                        window.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Błąd podczas rejestracji", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LoginEnter(object sender, EventArgs e)
        {
            if (LoginTextBox.Text == "Login")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void LoginLeave(object sender, EventArgs e)
        {
            if (LoginTextBox.Text == "" || LoginTextBox.Text == null)
            {
                LoginTextBox.Text = "Login";
                LoginTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void PasswordEnter(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == "Hasło")
            {
                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void PasswordLeave(object sender, EventArgs e)
        {
            if (PasswordTextBoxP.Password == "" || PasswordTextBoxP.Password == null)
            {
                PasswordTextBox.Text = "Hasło";
                PasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RepeatPasswordEnter(object sender, EventArgs e)
        {
            if (RepeatPasswordTextBox.Text == "Powtórz hasło")
            {
                RepeatPasswordTextBox.Text = "";
                RepeatPasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void RepeatPasswordLeave(object sender, EventArgs e)
        {
            if (RepeatPasswordTextBoxP.Password == "" || RepeatPasswordTextBoxP.Password == null)
            {
                RepeatPasswordTextBox.Text = "Powtórz hasło";
                RepeatPasswordTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void NameEnter(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "Imię")
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void NameLeave(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "" || NameTextBox.Text == null)
            {
                NameTextBox.Text = "Imię";
                NameTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }

        private void SurnameEnter(object sender, EventArgs e)
        {
            if (SurnameTextBox.Text == "Nazwisko")
            {
                SurnameTextBox.Text = "";
                SurnameTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
            }
        }

        private void SurnameLeave(object sender, EventArgs e)
        {
            if (SurnameTextBox.Text == "" || SurnameTextBox.Text == null)
            {
                SurnameTextBox.Text = "Nazwisko";
                SurnameTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Silver"));
            }
        }
    }
}
