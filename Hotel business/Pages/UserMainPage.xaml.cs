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

namespace Hotel_business.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserMainPage.xaml
    /// </summary>
    public partial class UserMainPage : Page
    {
        public UserMainPage()
        {
            InitializeComponent();
        }

        private void BtnAvailableRooms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookingPage());
        }

        private void BtnMyBookings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MyBookingsPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }

        private void BtnPaymentHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaymentHistoryPage());
        }
    }
}
