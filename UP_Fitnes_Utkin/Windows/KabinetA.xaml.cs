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
using UP_Fitnes_Utkin.Data;
using UP_Fitnes_Utkin.Model;

namespace UP_Fitnes_Utkin.Windows
{
    /// <summary>
    /// Логика взаимодействия для KabinetA.xaml
    /// </summary>
    public partial class KabinetA : Window
    {
        private readonly User? _user = null;

        public KabinetA(User user)
        {
            InitializeComponent();

            _user = user;

            Koshelek.Text = _user.Money.ToString() + " ₽";

           
            using (var context = new DbContact())
            {
                StackPanel stackPanel = KatalogSpisok;

                foreach (var item in context.kategorTovaras)
                {
                    RadioButton radioButton = new RadioButton();
                    radioButton.GroupName = "re";
                    radioButton.IsChecked = false;

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = item.Name;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.FontSize = 18;
                    radioButton.Content = textBlock;

                    stackPanel.Children.Add(radioButton);
                }

            }
            

        }

        private void Korzina_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KoshelPopoln_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
