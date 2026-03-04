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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = Connection.entities.Users.Find(UserSession.CurrentUser.UserId);
            if (user != null)
            {
                txtLogin.Text = user.Login;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string newPass = txtNewPassword.Password.Trim();
            string confirm = txtConfirmPassword.Password.Trim();

            if (!string.IsNullOrEmpty(newPass) || !string.IsNullOrEmpty(confirm))
            {
                if (newPass != confirm)
                {
                    lblMessage.Text = "Пароли не совпадают.";
                    return;
                }

                if (newPass.Length < 3)
                {
                    lblMessage.Text = "Пароль должен быть не менее 3 символов.";
                    return;
                }

                var user = Connection.entities.Users.Find(UserSession.CurrentUser.UserId);
                user.Password = newPass;
                Connection.entities.SaveChanges();
                lblMessage.Text = "Пароль успешно изменён.";
                txtNewPassword.Password = "";
                txtConfirmPassword.Password = "";
            }
            else
            {
                lblMessage.Text = "Нет изменений для сохранения.";
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
