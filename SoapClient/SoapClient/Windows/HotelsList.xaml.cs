using Contracts.Models;
using Contracts.ViewModels.HotelsListModels;
using SoapClient.HotelSoap;
using SoapClient.Windows.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for HotelsList.xaml
    /// </summary>
    public partial class HotelsList : Window
    {
        private List<Hotel> ListOfHotels { get; set; }
        private Account CurrentUser { get; set; }

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
