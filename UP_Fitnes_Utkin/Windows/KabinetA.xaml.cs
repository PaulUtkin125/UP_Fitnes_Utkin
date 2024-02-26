using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UP_Fitnes_Utkin.Data;
using UP_Fitnes_Utkin.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.Forms.MessageBox;
using Panel = System.Windows.Forms.Panel;
using RadioButton = System.Windows.Controls.RadioButton;

namespace UP_Fitnes_Utkin.Windows
{
    /// <summary>
    /// Логика взаимодействия для KabinetA.xaml
    /// </summary>
    public partial class KabinetA : System.Windows.Window
    {
        private readonly User? _user = null;

        public KabinetA(User user)
        {
            InitializeComponent();

            _user = user;

            Koshelek.Text = _user.Money.ToString() + " ₽";


            using (var context = new DbContact())
            {
                ScrollViewer scrollViewer = KatalogSpisok;
                StackPanel stackPanel = new StackPanel();


                int triger = 1;
                foreach (var item in context.kategorTovaras)
                {
                    RadioButton radioButton = new RadioButton();
                    TextBlock textBlock = new TextBlock();
                    if (triger == 1)
                    {
                        radioButton.GroupName = "re";
                        radioButton.IsChecked = false;
                        textBlock.Text = "Все товары";
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        textBlock.FontSize = 18;
                        textBlock.VerticalAlignment = VerticalAlignment.Top;
                        radioButton.Content = textBlock;
                        radioButton.Click += VuborKategorii_Click;

                        stackPanel.Children.Add(radioButton);
                        triger = 0;
                    }
                    else
                    {
                        radioButton.GroupName = "re";
                        radioButton.IsChecked = false;
                        textBlock.Text = item.Name;
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        textBlock.FontSize = 18;
                        textBlock.VerticalAlignment = VerticalAlignment.Top;
                        radioButton.Content = textBlock;
                        radioButton.Click += VuborKategorii_Click;

                        stackPanel.Children.Add(radioButton);
                    }
                }
                scrollViewer.Content = stackPanel;

                WrapPanel Spisok = SpisokTovarov;
                foreach (var item in context.tovar)
                {
                    StackPanel stackPanelTovar = new StackPanel();
                    BitmapImage bitmapImage = new BitmapImage(new Uri(item.Photo));
                    Image image = new Image();
                    image.Source = bitmapImage;
                    image.Width = 100;
                    image.Height = 100;
                    stackPanelTovar.Children.Add(image);

                    TextBlock textBlockNameTovar = new TextBlock();
                    textBlockNameTovar.Text = item.Name_tovar;
                    textBlockNameTovar.FontSize = 18;
                    stackPanelTovar.Children.Add(textBlockNameTovar);
                    TextBlock textBlockKolTovara = new TextBlock();
                    textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                    textBlockKolTovara.FontSize = 15;
                    stackPanelTovar.Children.Add(textBlockKolTovara);

                    System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                    button.Content = "В корзину";
                    button.FontSize = 12;
                    button.Width = 100;
                    button.Click += Vubor_Click;
                    stackPanelTovar.Children.Add(button);

                    stackPanelTovar.Margin = new Thickness(10, 5, 5, 10) ;
                    Spisok.Children.Add(stackPanelTovar);
                }
            }
        }

        private void Korzina_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KoshelPopoln_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Vubor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VuborKategorii_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanelKategori = KatalogSpisok.Content as StackPanel;
            string content;
            foreach (var child in stackPanelKategori.Children)
            {
                if (child is RadioButton radioButton)
                {
                    if (radioButton.IsChecked == true)
                    {
                        if (radioButton.Content is TextBlock textBlock)
                        {
                            content = textBlock.Text;

                            using (var context = new DbContact())
                            {
                                if (content != "Все товары")
                                {
                                    var SearchTovar = context.tovar
                                      .Where(x => x.Category.Name == content);

                                    SpisokTovarov.Children.Clear();
                                    WrapPanel Spisok = SpisokTovarov;
                                    foreach (var item in SearchTovar)
                                    {
                                        StackPanel stackPanelTovar = new StackPanel();
                                        BitmapImage bitmapImage = new BitmapImage(new Uri(item.Photo));
                                        Image image = new Image();
                                        image.Source = bitmapImage;
                                        image.Width = 100;
                                        image.Height = 100;
                                        stackPanelTovar.Children.Add(image);

                                        TextBlock textBlockNameTovar = new TextBlock();
                                        textBlockNameTovar.Text = item.Name_tovar;
                                        textBlockNameTovar.FontSize = 18;
                                        stackPanelTovar.Children.Add(textBlockNameTovar);
                                        TextBlock textBlockKolTovara = new TextBlock();
                                        textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                                        textBlockKolTovara.FontSize = 15;
                                        stackPanelTovar.Children.Add(textBlockKolTovara);

                                        System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                                        button.Content = "В корзину";
                                        button.FontSize = 12;
                                        button.Width = 100;
                                        button.Click += Vubor_Click;
                                        stackPanelTovar.Children.Add(button);

                                        stackPanelTovar.Margin = new Thickness(10, 5, 5, 10);
                                        Spisok.Children.Add(stackPanelTovar);
                                    }
                                }
                                else
                                {
                                    var SearchTovar = context.tovar;

                                    SpisokTovarov.Children.Clear();
                                    WrapPanel Spisok = SpisokTovarov;
                                    foreach (var item in SearchTovar)
                                    {
                                        StackPanel stackPanelTovar = new StackPanel();
                                        BitmapImage bitmapImage = new BitmapImage(new Uri(item.Photo));
                                        Image image = new Image();
                                        image.Source = bitmapImage;
                                        image.Width = 100;
                                        image.Height = 100;
                                        stackPanelTovar.Children.Add(image);

                                        TextBlock textBlockNameTovar = new TextBlock();
                                        textBlockNameTovar.Text = item.Name_tovar;
                                        textBlockNameTovar.FontSize = 18;
                                        stackPanelTovar.Children.Add(textBlockNameTovar);
                                        TextBlock textBlockKolTovara = new TextBlock();
                                        textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                                        textBlockKolTovara.FontSize = 15;
                                        stackPanelTovar.Children.Add(textBlockKolTovara);

                                        System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                                        button.Content = "В корзину";
                                        button.FontSize = 12;
                                        button.Width = 100;
                                        button.Click += Vubor_Click;
                                        stackPanelTovar.Children.Add(button);

                                        stackPanelTovar.Margin = new Thickness(10, 5, 5, 10);
                                        Spisok.Children.Add(stackPanelTovar);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
