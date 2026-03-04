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
    /// Логика взаимодействия для ManageBookingsPage.xaml
    /// </summary>
    public partial class ManageBookingsPage : Page
    {
        public ManageBookingsPage()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = Connection.entities.Bookings
                .Include("Users") // строковая версия
                .Include("Rooms")
                .OrderByDescending(b => b.StartDate)
                .ToList();
            lvBookings.ItemsSource = bookings;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvBookings.SelectedItem as Bookings;
            if (selected == null)
            {
                MessageBox.Show("Выберите бронирование.");
                return;
            }

            selected.Status = "Confirmed";
            Connection.entities.SaveChanges();
            LoadBookings(); // обновляем список
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvBookings.SelectedItem as Bookings;
            if (selected == null)
            {
                MessageBox.Show("Выберите бронирование.");
                return;
            }

            selected.Status = "Cancelled";
            Connection.entities.SaveChanges();
            LoadBookings();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
