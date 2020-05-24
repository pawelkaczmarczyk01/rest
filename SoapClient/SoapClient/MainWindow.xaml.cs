using SoapClient.HotelSoap;
using SoapClient.Windows;
using SoapClient.Windows.Authorization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoapClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
           // GetValue();
        }

        //private string GetValue()
        //{
        //    var client = new HotelsPortClient();
        //    var request = new getNameRequest();
        //    var response = client.getName(request);
        //    return response.name;
        //}

        private void Start(object sender, RoutedEventArgs e)
        {
            var window = new LogIn();
            Close();
            window.Show();
        }
    }
}
