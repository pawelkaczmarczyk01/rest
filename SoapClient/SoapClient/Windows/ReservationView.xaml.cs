using Contracts.Models;
using Contracts.ViewModels.ReservationView;
using SoapClient.HotelSoap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            var client = new HotelsPortClient();
            var request = new addReservationRequest();
            var reservation = new reservationRequest();
            reservation.roomId = RoomId;
            reservation.roomReservationFrom = from.Value;
            reservation.roomReservationTo = to.Value;
            reservation.userId = CurrentUser.AccountId;
            request.reservation = reservation;
            var response = client.addReservation(request);

            if (response.info != null && !response.info.Equals("") && !response.info.Contains("Successfully"))
            {
                MessageBox.Show(response.info, "Błąd rezerwacji pokoju", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show("Zarezerowwano pokój od " + from.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) + " do " + to.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), "Rezerwacja pokoju", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }
    }
}
