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
using System.Windows.Shapes;

namespace Hotel_business.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditRoomWindow.xaml
    /// </summary>
    public partial class AddEditRoomWindow : Window
    {
        public Rooms Room { get; private set; }

        public AddEditRoomWindow(Rooms room = null)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Room = room ?? new Rooms();

            if (room != null)
            {
                txtRoomNumber.Text = room.RoomNumber;
                txtType.Text = room.Type;
                txtPrice.Text = room.PricePerNight.ToString();
                cmbStatus.Text = room.Status;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string number = txtRoomNumber.Text.Trim();
            string type = txtType.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(priceText) || string.IsNullOrEmpty(status))
            {
                lblError.Text = "Заполните все поля.";
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                lblError.Text = "Цена должна быть числом.";
                return;
            }

            // Проверка уникальности номера комнаты
            bool exists;
            if (Room.RoomId == 0) // новый номер
            {
                exists = Connection.entities.Rooms.Any(r => r.RoomNumber == number);
            }
            else // редактирование
            {
                exists = Connection.entities.Rooms.Any(r => r.RoomNumber == number && r.RoomId != Room.RoomId);
            }

            if (exists)
            {
                lblError.Text = "Номер комнаты уже существует.";
                return;
            }

            Room.RoomNumber = number;
            Room.Type = type;
            Room.PricePerNight = price;
            Room.Status = status;

            try
            {
                if (Room.RoomId == 0)
                    Connection.entities.Rooms.Add(Room);

                Connection.entities.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "Ошибка при сохранении: " + ex.Message;
                if (ex.InnerException != null)
                    lblError.Text += "\n" + ex.InnerException.Message;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

