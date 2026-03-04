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
    /// Логика взаимодействия для BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page
    {
        public BookingPage()
        {
            InitializeComponent();

            // Устанавливаем даты в коде, а не в XAML
            dpStart.SelectedDate = DateTime.Today;
            dpEnd.SelectedDate = DateTime.Today.AddDays(1);

            LoadRooms();
        }

        private void LoadRooms()
        {
            var rooms = Connection.entities.Rooms.Where(r => r.Status == "Available").ToList();
            lvRooms.ItemsSource = rooms;
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = lvRooms.SelectedItem as Rooms;
            if (selectedRoom == null)
            {
                MessageBox.Show("Выберите номер.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты заезда и выезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime start = dpStart.SelectedDate.Value;
            DateTime end = dpEnd.SelectedDate.Value;

            if (start >= end)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (start < DateTime.Today)
            {
                MessageBox.Show("Дата заезда не может быть раньше сегодняшнего дня.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка пересечения дат с существующими бронированиями
            bool isBooked = Connection.entities.Bookings.Any(b => b.RoomId == selectedRoom.RoomId &&
                                                                   b.Status != "Cancelled" &&
                                                                   ((start >= b.StartDate && start < b.EndDate) ||
                                                                    (end > b.StartDate && end <= b.EndDate) ||
                                                                    (start <= b.StartDate && end >= b.EndDate)));
            if (isBooked)
            {
                MessageBox.Show("Номер уже забронирован на выбранные даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var booking = new Bookings
            {
                UserId = UserSession.CurrentUser.UserId,
                RoomId = selectedRoom.RoomId,
                StartDate = start,
                EndDate = end,
                Status = "Pending"
            };
            Connection.entities.Bookings.Add(booking);
            Connection.entities.SaveChanges();

            // Переходим на страницу выбора услуг, передавая booking
            NavigationService.Navigate(new BookingServicesPage(booking));
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
