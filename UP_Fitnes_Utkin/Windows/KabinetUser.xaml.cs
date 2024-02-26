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
using UP_Fitnes_Utkin.Model;

namespace UP_Fitnes_Utkin.Windows
{
    /// <summary>
    /// Логика взаимодействия для KabinetUser.xaml
    /// </summary>
    public partial class KabinetUser : Window
    {
        private readonly User? _user = null;
        public KabinetUser(User user)
        {
            InitializeComponent();

            _user = user;
        }
    }
}
