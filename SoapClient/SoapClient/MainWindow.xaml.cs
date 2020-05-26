using SoapClient.Windows.Authorization;
using System.Windows;

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
        }


        private void Start(object sender, RoutedEventArgs e)
        {
            var window = new LogIn();
            Close();
            window.Show();
        }
    }
}
