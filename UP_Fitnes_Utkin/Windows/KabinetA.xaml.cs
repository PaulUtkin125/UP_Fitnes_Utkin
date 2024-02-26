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
        public string PhotoFileNeme;
        public KabinetA(User user)
        {
            InitializeComponent();

            _user = user;

            Koshelek.Text = _user.Money.ToString() + " ₽";


            using (var context = new DbContact())
            {
                ScrollViewer scrollViewer = KatalogSpisok;
                StackPanel stackPanel = new StackPanel();

                RadioButton radioButton1 = new RadioButton();
                TextBlock textBlock1 = new TextBlock();
                radioButton1.GroupName = "re";
                radioButton1.IsChecked = false;
                textBlock1.Text = "Все товары";
                textBlock1.TextWrapping = TextWrapping.Wrap;
                textBlock1.FontSize = 18;
                textBlock1.VerticalAlignment = VerticalAlignment.Top;
                radioButton1.Content = textBlock1;
                radioButton1.Click += VuborKategorii_Click;

                stackPanel.Children.Add(radioButton1);
                foreach (var item in context.kategorTovaras)
                {
                    RadioButton radioButton = new RadioButton();
                    TextBlock textBlock = new TextBlock();
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
                scrollViewer.Content = stackPanel;

                ScrollViewer scrollViewerTov = SpisTov; 
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
                scrollViewerTov.Content = Spisok;
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

        private void VnestiDannue_Click(object sender, RoutedEventArgs e)
        {
            if (boxVub.SelectedIndex == 0)       // товар
            {
                if (string.IsNullOrEmpty(CategOutputBox.Text))
                {
                    MessageBox.Show("Поле 'Категория' должно быть заполнено!", "Предупреждение");
                    return;
                }
                if (string.IsNullOrEmpty(CenaNewTovara.Text))
                {
                    MessageBox.Show("Поле 'Цена' должно быть заполнено!", "Предупреждение");
                    return;
                }
                if (string.IsNullOrEmpty(NameNewTovara.Text))
                {
                    MessageBox.Show("Поле 'Название' должно быть заполнено!", "Предупреждение");
                    return;
                }
                if (string.IsNullOrEmpty(KolNewInputBox.Text))
                {
                    MessageBox.Show("Поле 'Кол-во' должно быть заполнено!", "Предупреждение");
                    return;
                }
                if (PhotoFileNeme is null)
                {
                    MessageBox.Show("Фотографии товара не выбрана!", "Предупреждение");
                    return;
                }

                string neme = NameNewTovara.Text;
                string categor = CategOutputBox.Text;
                int kol = int.Parse(KolNewInputBox.Text);
                double cena = double.Parse(CenaNewTovara.Text);

                if (kol < 1)
                {
                    MessageBox.Show("Недопустимое значение поля 'Кол-во'!", "Предупреждение");
                    return;
                }
                if (cena < 0.01)
                {
                    MessageBox.Show("Недопустимое значение поля 'Цена'!", "Предупреждение");
                    return;
                }

                using (var context = new DbContact())
                {
                    var NotTovar = context.tovar.SingleOrDefault(x => x.Name_tovar == neme && x.Category.Name == categor);
                    if (NotTovar != null) { MessageBox.Show("Такой товар уже существут!", "Предупреждение"); return; }

                    int categID = 0;
                    foreach (var categItem in context.kategorTovaras)
                    {
                        if (categor == categItem.Name)
                        {
                            categID = categItem.Id;
                            break;
                        }
                    }

                    var NewTovar = new Tovar_Sklad { Name_tovar = neme, CategID = categID, Price_sht = cena, Count_tekyshee = kol, Photo = PhotoFileNeme };

                    context.tovar.Add(NewTovar);
                    context.SaveChanges();
                    MessageBox.Show("Запись добавлена", "Уведомление");

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

                        stackPanelTovar.Margin = new Thickness(10, 5, 5, 10);
                        Spisok.Children.Add(stackPanelTovar);
                    }
                };

            }   // товар
            if (boxVub.SelectedIndex == 1)  // категория
            {
                if (string.IsNullOrEmpty(NewCategInputBox.Text))
                {
                    MessageBox.Show("Поле пустое!");
                    return;
                }
                using (var context = new DbContact())
                {
                    var NotCateg = context.kategorTovaras.SingleOrDefault(x => x.Name == NewCategInputBox.Text);
                    if (NotCateg != null) { MessageBox.Show("Такая категория уже существут!", "Предупреждение"); return; }

                    var NewCateg = new KategorTovara { Name = NewCategInputBox.Text };

                    context.kategorTovaras.Add(NewCateg);
                    context.SaveChanges();
                    MessageBox.Show("Категория добавлена!");
                }

            }  // категория
            if (boxVub.SelectedIndex == 2)
            {
                MessageBox.Show("Элемент для добавления не выбран!", "Предупреждение");
                return;

            }   // ничего
        }

        private void boxVub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpisokTovarov.Children.Clear();
            SpisokTovarov.Visibility = Visibility.Collapsed;
            NewData.Visibility = Visibility.Visible;

            if (boxVub.SelectedIndex == 0)       // товар
            {
                using (var context = new DbContact())
                {
                    foreach (var item in context.kategorTovaras)
                    {
                        CategOutputBox.Items.Add(item.Name);
                    }
                }

                
                NewTovar.Visibility = Visibility.Visible;
                SpisTov.Visibility = Visibility.Collapsed;
                NewCategor.Visibility = Visibility.Collapsed;
            }   // товар

            if (boxVub.SelectedIndex == 1)  // категория
            {
                NewCategor.Visibility = Visibility.Visible;

                SpisTov.Visibility = Visibility.Collapsed;
                NewTovar.Visibility = Visibility.Collapsed;
                CategOutputBox.Items.Clear();
            }  // категория
            if (boxVub.SelectedIndex == 2)
            {
                NewCategor.Visibility = Visibility.Collapsed;
                NewData.Visibility = Visibility.Collapsed;
                NewTovar.Visibility = Visibility.Collapsed;

                CategOutputBox.Items.Clear();

                SpisTov.Visibility = Visibility.Visible;
                SpisokTovarov.Visibility= Visibility.Visible;

                using (var context = new DbContact())
                {
                    ScrollViewer scrollViewer = KatalogSpisok;
                    StackPanel stackPanel = new StackPanel();

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

                        stackPanelTovar.Margin = new Thickness(10, 5, 5, 10);
                        Spisok.Children.Add(stackPanelTovar);
                    }
                }
            }   // ничего
        }

        private void VubratPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображение(jpg, png)| *.jpg; *.png";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Photo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                PhotoFileNeme = openFileDialog.FileName;
            }
        }
    }
}
