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
    /// Логика взаимодействия для AdminStatisticsPage.xaml
    /// </summary>
    public partial class AdminStatisticsPage : Page
    {
        public AdminStatisticsPage()
        {
            InitializeComponent();
 
            dpStart.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEnd.SelectedDate = DateTime.Today;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            DateTime start = dpStart.SelectedDate ?? DateTime.Today.AddMonths(-1);
            DateTime end = dpEnd.SelectedDate ?? DateTime.Today;

            var bookings = Connection.entities.Bookings
                .Where(b => b.Status == "Confirmed" && b.StartDate >= start && b.EndDate <= end)
                .ToList();

            int totalBookings = bookings.Count;
            decimal totalRevenue = 0;

            var bookingIds = bookings.Select(b => b.BookingId).ToList();
            var payments = Connection.entities.Payments
                .Where(p => bookingIds.Contains(p.BookingId) && p.Status == "Completed")
                .ToList();
            totalRevenue = payments.Sum(p => p.Amount);

            var rooms = Connection.entities.Rooms.ToList();
            int totalRooms = rooms.Count;
            if (totalRooms > 0 && (end - start).Days > 0)
            {
                double totalOccupiedDays = 0;
                for (DateTime date = start; date <= end; date = date.AddDays(1))
                {
                    int occupied = bookings.Count(b => b.StartDate <= date && b.EndDate > date);
                    totalOccupiedDays += occupied;
                }
                double avgOccupancy = (totalOccupiedDays / ((end - start).Days + 1)) / totalRooms * 100;
                txtAvgOccupancy.Text = $"Средняя загрузка номеров: {avgOccupancy:F1}%";
            }
            else
            {
                txtAvgOccupancy.Text = "Средняя загрузка номеров: 0%";
            }

            txtTotalBookings.Text = $"Всего бронирований: {totalBookings}";
            txtTotalRevenue.Text = $"Общий доход: {totalRevenue:C}";

            var stats = bookings.GroupBy(b => b.Rooms.Type)
                .Select(g => new
                {
                    RoomType = g.Key,
                    Count = g.Count(),
                    Revenue = payments.Where(p => g.Select(b => b.BookingId).Contains(p.BookingId)).Sum(p => p.Amount)
                }).ToList();
            lvStatsByRoomType.ItemsSource = stats;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
