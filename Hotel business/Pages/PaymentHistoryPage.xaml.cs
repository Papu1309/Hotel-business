using Hotel_business.Connect;
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
    /// Логика взаимодействия для PaymentHistoryPage.xaml
    /// </summary>
    public partial class PaymentHistoryPage : Page
    {
        public PaymentHistoryPage()
        {
            InitializeComponent();
            LoadPayments();
        }

        private void LoadPayments()
        {
            var payments = Connection.entities.Payments
                .Where(p => p.Bookings.UserId == UserSession.CurrentUser.UserId)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();
            lvPayments.ItemsSource = payments;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
