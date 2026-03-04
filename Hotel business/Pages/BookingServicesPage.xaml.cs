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
    /// Логика взаимодействия для BookingServicesPage.xaml
    /// </summary>
    public partial class BookingServicesPage : Page
    {
        private Bookings _booking;
        private List<ServiceViewModel> _serviceList;

        public class ServiceViewModel : Services
        {
            public int SelectedQuantity { get; set; }
        }

        public BookingServicesPage(Bookings booking)
        {
            InitializeComponent();
            _booking = booking;
            LoadServices();
        }

        private void LoadServices()
        {
            var services = Connection.entities.Services.ToList();
            _serviceList = services.Select(s => new ServiceViewModel
            {
                ServiceId = s.ServiceId,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                SelectedQuantity = 0
            }).ToList();
            lvServices.ItemsSource = _serviceList;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем выбранные услуги в сессии или временном объекте
            var selectedServices = _serviceList.Where(s => s.SelectedQuantity > 0).ToList();
            if (selectedServices.Any())
            {
                // Передаём список в PaymentPage
                NavigationService.Navigate(new PaymentPage(_booking, selectedServices));
            }
            else
            {
                // Если услуги не выбраны, просто идём на оплату
                NavigationService.Navigate(new PaymentPage(_booking, null));
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
