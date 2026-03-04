using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hotel_business.Windows
{
    /// <summary>
    /// Логика взаимодействия для CardPaymentWindow.xaml
    /// </summary>
    public partial class CardPaymentWindow : Window
    {
        public CardPaymentWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = txtCardNumber.Text.Replace(" ", "").Replace("-", "");
            string expiry = txtExpiry.Text.Trim();
            string cvv = txtCVV.Text.Trim();

            if (!Regex.IsMatch(cardNumber, @"^\d{16}$"))
            {
                lblError.Text = "Номер карты должен содержать 16 цифр.";
                return;
            }

            if (!Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                lblError.Text = "Срок действия должен быть в формате ММ/ГГ.";
                return;
            }

            if (!Regex.IsMatch(cvv, @"^\d{3}$"))
            {
                lblError.Text = "CVV должен содержать 3 цифры.";
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
