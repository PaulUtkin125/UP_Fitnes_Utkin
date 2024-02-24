using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP_Fitnes_Utkin.Data;
using UP_Fitnes_Utkin.Model;

namespace UP_Fitnes_Utkin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            string Password_ = PasswordInputVhodBox.Text;
            string Login_ = LoginInputVhodBox.Text;

            

            if (string.IsNullOrEmpty(Password_) && string.IsNullOrEmpty(Login_))
            {
                MessageBox.Show($"Пароль и логин должны быть заполнены!");
                return;
            }

            using (var context = new DbContact())
            {
                var userExists = context.users.SingleOrDefault(x => x.Login == Login_ && x.Password == Password_);
                if (userExists is not null)
                {

                    return;
                }
            }
            MessageBox.Show("Неправильный логин или пароль!");
        }

        private void Registeciiy_Click(object sender, RoutedEventArgs e)
        {
            string Password_ = PasswordInputRegBox.Text;
            string Login_ = LoginInputRegBox.Text;

           

            if (string.IsNullOrEmpty(Password_) && string.IsNullOrEmpty(Login_))
            {
                MessageBox.Show($"Пароль и логин должны быть заполнены!");
                return;
            }

            var user = new User() {Login = Login_, Password = Password_, RoleId = 1 };
            using (var context = new DbContact()) 
            {
                var userExists = context.users.SingleOrDefault(x => x.Login == Login_);
                if (userExists is not null) 
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                context.users.Add(user);
                context.SaveChanges();
            }
        }
    }
}