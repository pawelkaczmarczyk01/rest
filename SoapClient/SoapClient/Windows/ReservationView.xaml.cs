using Contracts.Models;
using Contracts.ViewModels.ReservationView;
using Contracts.WebClientModels.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace SoapClient.Windows
{
    /// <summary>
    /// Interaction logic for ReservationView.xaml
    /// </summary>
    public partial class ReservationView : Window
    {
        private List<ReservationVM> ReservationsList { get; set; }
        private int RoomId { get; set; }
        public Account CurrentUser { get; set; }

        private readonly string BaseAddress = "http://localhost:8080/";
        private readonly string EndpointAddReservation = "reservation/add";

        public ReservationView(List<ReservationVM> reservations, int roomId)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ReservationsList = reservations;
            InitializeComponent();
            ListOfReservations.ItemsSource = ReservationsList;
            RoomId = roomId;
            CurrentUser = (Account)Application.Current.Resources["user"];
        }

        private void ReserveRoom(object sender, RoutedEventArgs e)
        {
            var from =  DateFromPicker.SelectedDate;
            if (!from.HasValue)
            {
                MessageBox.Show("Wybierz datę początku rezerwacji.", "Nie wybrano daty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var to = DateToPicker.SelectedDate;
            if (!to.HasValue)
            {
                MessageBox.Show("Wybierz datę końca rezerwacji.", "Nie wybrano daty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    var reservationToAddRequest = new ReservationToAddRequest();
                    reservationToAddRequest.roomId = RoomId;
                    reservationToAddRequest.reservationFrom = from.Value;
                    reservationToAddRequest.reservationTo = to.Value;
                    reservationToAddRequest.userId = CurrentUser.AccountId;
                    var request = JsonConvert.SerializeObject(reservationToAddRequest);
                    var response = client.UploadString(BaseAddress + EndpointAddReservation, request);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd rezerwacji pokoju", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //if (response.info != null && !response.info.Equals("") && !response.info.Contains("Successfully"))
            //{
            //    MessageBox.Show(response.info, "Błąd rezerwacji pokoju", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            MessageBox.Show("Zarezerowwano pokój od " + from.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) + " do " + to.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "Rezerwacja pokoju", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }
    }
}
