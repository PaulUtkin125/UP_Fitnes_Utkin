using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MessageBox = System.Windows.Forms.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
using Panel = System.Windows.Forms.Panel;
using RadioButton = System.Windows.Controls.RadioButton;
using TextBox = System.Windows.Controls.TextBox;

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
                    TextBlock blocIdtov = new TextBlock();
                    blocIdtov.Text = item.Id.ToString();
                    blocIdtov.Visibility = Visibility.Collapsed;
                    blocIdtov.Name = "Id";
                    stackPanelTovar.Children.Add(blocIdtov);


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
            KatalogVesi.Visibility = Visibility.Collapsed;
            KorzonaVsiy.Visibility = Visibility.Visible;

            int counter = 0;
            foreach (var item in KorzinaSpisok.Children)
            {
                if (item is StackPanel) counter++;
            }
            HintAssist.SetHint(KolTovObsh, $"Товары, {counter} шт.");
        }

        private void KoshelPopoln_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KoshelInputBox.Text)) return;
            if (double.Parse(KoshelInputBox.Text.Replace('.', ',')) < 0)
            {
                KoshelInputBox.Text = "";
                MessageBox.Show("Поле не может быть отрицательным!");
                return;
            }

            var UserId = _user.ID;

            using (var context = new DbContact())
            {
                var user = context.users.Find(UserId);
                user.Money += double.Parse(KoshelInputBox.Text.Replace('.',','));

                context.SaveChanges();

                Koshelek.Text = user.Money.ToString() + " ₽";
                KoshelInputBox.Text = "";
            }
        }
        private void Vubor_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null) 
            { 
                StackPanel block = button.Parent as StackPanel;
                if (block != null) 
                {
                    foreach (var children in block.Children)
                    {
                        if (children is TextBlock blocIdtov) 
                        {
                            if (children is TextBlock text)
                            {
                                if (text.Name == "Id")
                                {
                                    int IdTov = int.Parse(text.Text);
                                    TextBox textBoxKolTovara = new TextBox();
                                    TextBlock textPrise = new TextBlock();
                                    
                                    using (var context = new DbContact())
                                    {
                                        var DbTovar = context.tovar.Find(IdTov);
                                        StackPanel panelKorzonaStroka = new StackPanel();
                                        panelKorzonaStroka.Name = "Id" + DbTovar.Id.ToString();
                                        StackPanel panelKolRed = new StackPanel();
                                        StackPanel PrisePanel = new StackPanel();
                                        panelKorzonaStroka.Orientation = Orientation.Horizontal;
                                        panelKolRed.Orientation = Orientation.Horizontal;

                                        BitmapImage bitmapImage = new BitmapImage(new Uri(DbTovar.Photo));
                                        Image image1 = new Image();
                                        image1.Source = bitmapImage;
                                        image1.Width = 100;
                                        image1.Height = 100;
                                        panelKorzonaStroka.Children.Add(image1);

                                        TextBlock textBlock1 = new TextBlock();
                                        textBlock1.Text = DbTovar.Name_tovar;
                                        textBlock1.Width = 150;
                                        textBlock1.VerticalAlignment = VerticalAlignment.Top;
                                        textBlock1.FontSize = 18;
                                        textBlock1.TextWrapping = TextWrapping.Wrap;
                                        panelKorzonaStroka.Children.Add(textBlock1);


                                        Button buttonYmensh = new Button();
                                        buttonYmensh.Width = 45;
                                        buttonYmensh.Height = 28;
                                        buttonYmensh.Content = "-";
                                        buttonYmensh.VerticalContentAlignment = VerticalAlignment.Center;
                                        buttonYmensh.FontSize = 18;
                                        buttonYmensh.VerticalAlignment = VerticalAlignment.Center;
                                        buttonYmensh.Click += (sender, e) =>
                                        {
                                            textBoxKolTovara.Text = (int.Parse(textBoxKolTovara.Text) - 1).ToString();
                                            textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                            if (int.Parse(textBoxKolTovara.Text) == 0) KorzinaSpisok.Children.Remove(panelKorzonaStroka);
                                        };
                                        panelKolRed.Children.Add(buttonYmensh);


                                        textBoxKolTovara.Width = 50;
                                        textBoxKolTovara.FontSize = 20;
                                        textBoxKolTovara.Text = "1";
                                        textBoxKolTovara.VerticalAlignment = VerticalAlignment.Center;
                                        textBoxKolTovara.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                                        textBoxKolTovara.Name = "KolTov";
                                        textBoxKolTovara.PreviewKeyDown += (sender, e) =>
                                        {
                                            if (e.Key == Key.Enter)
                                            {
                                                textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                                if (int.Parse(textBoxKolTovara.Text) == 0) KorzinaSpisok.Children.Remove(panelKorzonaStroka);
                                            }
                                        };
                                        panelKolRed.Children.Add(textBoxKolTovara);


                                        Button buttonPribav = new Button();
                                        buttonPribav.Width = 45;
                                        buttonPribav.Height = 28;
                                        buttonPribav.Content = "+";
                                        buttonPribav.FontSize = 15;
                                        buttonPribav.VerticalAlignment = VerticalAlignment.Center;
                                        buttonPribav.Click += (sender, e) =>
                                        {
                                            textBoxKolTovara.Text = (int.Parse(textBoxKolTovara.Text) + 1).ToString();
                                            textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                            buttonYmensh.IsEnabled = true;
                                        };
                                        panelKolRed.Children.Add(buttonPribav);
                                        panelKorzonaStroka.Children.Add(panelKolRed);

                                        textPrise.Width = 100;
                                        textPrise.Name = "Price";
                                        textPrise.FontSize = 20;
                                        textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                        PrisePanel.Children.Add(textPrise);
                                        PrisePanel.VerticalAlignment = VerticalAlignment.Center;
                                        PrisePanel.Margin = new Thickness(20, 0, 0, 0);
                                        panelKorzonaStroka.Children.Add(PrisePanel);

                                        KorzinaSpisok.Children.Add(panelKorzonaStroka);

                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                                        TextBlock blocIdtov = new TextBlock();
                                        blocIdtov.Text = item.Category.Name;
                                        blocIdtov.Name = "Id";
                                        blocIdtov.Visibility = Visibility.Collapsed;
                                        stackPanelTovar.Children.Add(blocIdtov);


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
                                        TextBlock blocIdtov = new TextBlock();
                                        blocIdtov.Text = item.Id.ToString();
                                        blocIdtov.Name = "Id";
                                        blocIdtov.Visibility = Visibility.Collapsed;
                                        stackPanelTovar.Children.Add(blocIdtov);


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
                        TextBlock blocIdtov = new TextBlock();
                        blocIdtov.Text = item.Id.ToString();
                        blocIdtov.Name = "Id";
                        blocIdtov.Visibility = Visibility.Collapsed;
                        stackPanelTovar.Children.Add(blocIdtov);

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
                        TextBlock blocIdtov = new TextBlock();
                        blocIdtov.Text = item.Id.ToString();
                        blocIdtov.Visibility = Visibility.Collapsed;
                        stackPanelTovar.Children.Add(blocIdtov);

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

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            KatalogVesi.Visibility = Visibility.Visible;
            KorzonaVsiy.Visibility = Visibility.Collapsed;
        }

        private void Vuhod_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Kupit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
