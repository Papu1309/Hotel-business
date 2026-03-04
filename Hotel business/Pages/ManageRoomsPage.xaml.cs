using Hotel_business.Connect;
using Hotel_business.Windows;
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
    /// Логика взаимодействия для ManageRoomsPage.xaml
    /// </summary>
    public partial class ManageRoomsPage : Page
    {
        public ManageRoomsPage()
        {
            InitializeComponent();
            LoadRooms();
        }

        private void LoadRooms()
        {
            lvRooms.ItemsSource = Connection.entities.Rooms.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditRoomWindow();
            if (dialog.ShowDialog() == true)
            {
                LoadRooms();
            }
        }

        private void BtnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvRooms.SelectedItem as Rooms;
            if (selected == null)
            {
                MessageBox.Show("Выберите номер.");
                return;
            }

            if (selected.Status == "Available")
                selected.Status = "Maintenance";
            else if (selected.Status == "Maintenance")
                selected.Status = "Available";

            Connection.entities.SaveChanges();
            LoadRooms();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = lvRooms.SelectedItem as Rooms;
            if (selected == null) return;

            // Проверяем, есть ли связанные бронирования
            var hasBookings = Connection.entities.Bookings.Any(b => b.RoomId == selected.RoomId);
            if (hasBookings)
            {
                MessageBox.Show("Невозможно удалить номер, так как на него есть бронирования.\nСначала удалите или перенесите бронирования.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить номер? Это действие необратимо.", "Подтверждение",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Connection.entities.Rooms.Remove(selected);
                Connection.entities.SaveChanges();
                LoadRooms(); // обновляем список
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
