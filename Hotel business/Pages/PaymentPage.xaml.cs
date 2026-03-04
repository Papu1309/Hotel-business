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
    /// Логика взаимодействия для PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        private Bookings _booking;
        private List<BookingServicesPage.ServiceViewModel> _selectedServices;


        public PaymentPage(Bookings booking, List<BookingServicesPage.ServiceViewModel> selectedServices = null)
        {
            InitializeComponent();
            _booking = booking;
            _selectedServices = selectedServices;

            // Подгружаем связанные данные (Room)
            Connection.entities.Entry(booking).Reference(b => b.Rooms).Load();

            int nights = (booking.EndDate - booking.StartDate).Days;
            decimal roomAmount = booking.Rooms.PricePerNight * nights;

            decimal servicesAmount = 0;
            if (_selectedServices != null)
            {
                servicesAmount = _selectedServices.Sum(s => s.Price * s.SelectedQuantity);
            }

            decimal totalAmount = roomAmount + servicesAmount;
            txtAmount.Text = totalAmount.ToString("C");
        }



        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            decimal amount = decimal.Parse(txtAmount.Text, System.Globalization.NumberStyles.Currency);

            if (rbCard.IsChecked == true)
            {
                CardPaymentWindow cardWindow = new CardPaymentWindow();
                if (cardWindow.ShowDialog() == true)
                {
                    ProcessPayment(amount, "Card");
                }
            }
            else
            {
                ProcessPayment(amount, "Cash");
            }
        }

        private void ProcessPayment(decimal amount, string method)
        {
            try
            {
                // Создаём платёж
                var payment = new Payments
                {
                    BookingId = _booking.BookingId,
                    Amount = amount,
                    PaymentMethod = method,
                    PaymentDate = DateTime.Now,
                    Status = "Completed",
                    Description = "Оплата бронирования"
                };
                Connection.entities.Payments.Add(payment);

                // Если были выбраны услуги, создаём записи в BookingServices
                if (_selectedServices != null && _selectedServices.Any())
                {
                    foreach (var service in _selectedServices)
                    {
                        var bookingService = new BookingServices
                        {
                            BookingId = _booking.BookingId,
                            ServiceId = service.ServiceId,
                            Quantity = service.SelectedQuantity,
                            PriceAtBooking = service.Price
                        };
                        Connection.entities.BookingServices.Add(bookingService);
                    }
                }

                _booking.Status = "Confirmed";
                Connection.entities.SaveChanges();

                string address = "г. Москва, ул. Тверская, д. 1";
                DateTime checkInTime = _booking.StartDate.Date.AddHours(14);
                string message = $"Оплата прошла успешно!\nАдрес гостиницы: {address}\nВы можете заехать {checkInTime:dd.MM.yyyy} после {checkInTime:HH:mm}.";
                MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.Navigate(new UserMainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении платежа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var booking = Connection.entities.Bookings.Find(_booking.BookingId);
            if (booking != null && booking.Status == "Pending")
            {
                Connection.entities.Bookings.Remove(booking);
                Connection.entities.SaveChanges();
            }
            NavigationService.Navigate(new UserMainPage());
        }
    }
}
