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
    /// Логика взаимодействия для MyBookingsPage.xaml
    /// </summary>
    public partial class MyBookingsPage : Page
    {
        public MyBookingsPage()
        {
            InitializeComponent();
            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = Connection.entities.Bookings
                .Include("Rooms") // или "Room" – смотрите вашу модель
                .Where(b => b.UserId == UserSession.CurrentUser.UserId)
                .OrderByDescending(b => b.StartDate)
                .ToList();
            lvBookings.ItemsSource = bookings;
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int bookingId;
            if (!int.TryParse(button.Tag.ToString(), out bookingId))
            {
                MessageBox.Show("Ошибка идентификатора бронирования.");
                return;
            }

            var booking = Connection.entities.Bookings.Find(bookingId);
            if (booking != null)
            {
                NavigationService.Navigate(new PaymentPage(booking));
            }
        }

        private void BtnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int bookingId;
            if (!int.TryParse(button.Tag.ToString(), out bookingId))
            {
                MessageBox.Show("Ошибка идентификатора бронирования.");
                return;
            }

            var booking = Connection.entities.Bookings.Find(bookingId);
            if (booking == null) return;

            bool canCancel = false;
            string message = "";

            if (booking.Status == "Pending")
            {
                canCancel = true;
            }
            else if (booking.Status == "Confirmed")
            {
                if (booking.StartDate > DateTime.Today.AddDays(1))
                {
                    canCancel = true;
                }
                else
                {
                    message = "Нельзя отменить бронирование менее чем за 24 часа до заезда.";
                }
            }
            else
            {
                message = "Это бронирование уже отменено или завершено.";
            }

            if (!canCancel)
            {
                MessageBox.Show(message, "Отмена невозможна", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите отменить бронирование?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                booking.Status = "Cancelled";
                Connection.entities.SaveChanges();
                LoadBookings(); // обновляем список
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

