using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UP_Fitnes_Utkin.Data;
using UP_Fitnes_Utkin.Model;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.Forms.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
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
        public int counter = 0;
        public double SumKOplate = 0.0;


        List<int[]> List1 = new List<int[]>();

        public KabinetA(User user)
        {
            InitializeComponent();

            _user = user;

            Koshelek.Text = _user.Money.ToString() + " ₽";


            VuvodRadioButton();
            VuvodTovarov();
        }


        public int[] ints1 = new int[2]; // Id, кол-во //

        public void VuvodRadioButton() 
        {
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

            }
        }

        public void VuvodTovarov()
        {
            using (var context = new DbContact())
            {
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
                    textBlockNameTovar.Width = 100;//
                    textBlockNameTovar.Height = 50;//
                    textBlockNameTovar.TextWrapping = TextWrapping.Wrap;//
                    stackPanelTovar.Children.Add(textBlockNameTovar);

                    TextBlock textBlockKolTovara = new TextBlock();
                    textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                    textBlockKolTovara.FontSize = 15;
                    textBlockKolTovara.Visibility = Visibility.Collapsed;//
                    stackPanelTovar.Children.Add(textBlockKolTovara);

                    TextBlock blockPriseSht = new TextBlock();//
                    blockPriseSht.Text = item.Price_sht.ToString() + " ₽";//
                    blockPriseSht.FontSize = 15;//
                    stackPanelTovar.Children.Add(blockPriseSht);//

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
        }


        private void Korzina_Click(object sender, RoutedEventArgs e)
        {
            KatalogVesi.Visibility = Visibility.Collapsed;
            KorzonaVsiy.Visibility = Visibility.Visible;

            SpisokTovarov.Children.Clear();

            counter = 0;
            foreach (var item in KorzinaSpisok.Children)
            {
                if (item is StackPanel ty) counter += 1;
            }
            HintAssist.SetHint(KolTovObsh, $"Товары, {counter} шт.");
            ItogoOutBlock.Text = SumKOplate.ToString() + " ₽";
        }


        private void KoshelPopoln_Click(object sender, RoutedEventArgs e)
        {
            foreach (char letter in KoshelInputBox.Text.Replace("-", ""))
            {
                if (!char.IsDigit(letter))
                {
                    MessageBox.Show("Поле поддерживает только числовые значения", "Ошибка пополнения кошелька");
                    KoshelInputBox.Text = "";
                    return;
                }
            }

            if (string.IsNullOrEmpty(KoshelInputBox.Text)) return;
            if (double.Parse(KoshelInputBox.Text.Replace('.', ',')) < 0)
            {
                KoshelInputBox.Text = "";
                MessageBox.Show("Поле не может быть отрицательным!", "Ошибка пополнения кошелька");
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
                                    ints1[0] = IdTov;

                                    using (var context = new DbContact())
                                    {
                                        var DbTovar = context.tovar.Find(IdTov);
                                        StackPanel panelKorzonaStroka = new StackPanel();
                                        panelKorzonaStroka.Name = "Id" + DbTovar.Id.ToString();
                                        StackPanel panelKolRed = new StackPanel();
                                        StackPanel PrisePanel = new StackPanel();
                                        panelKorzonaStroka.Orientation = Orientation.Horizontal;
                                        panelKolRed.Orientation = Orientation.Horizontal;

                                        TextBlock textId = new TextBlock();
                                        textId.Name = "Id";
                                        textId.Text = IdTov.ToString();
                                        textId.Visibility = Visibility.Collapsed;
                                        panelKorzonaStroka.Children.Add(textId);

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
                                            SumKOplate -= DbTovar.Price_sht;
                                            ItogoOutBlock.Text = SumKOplate.ToString() + " ₽";
                                            textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                            foreach (var mas in ints1)
                                                if (int.Parse(textBoxKolTovara.Text) == 0) 
                                                { 
                                                    KorzinaSpisok.Children.Remove(panelKorzonaStroka);
                                                    List1.Remove(ints1);
                                                }
                                            
                                            counter -= 1;
                                            HintAssist.SetHint(KolTovObsh, $"Товары, {counter} шт.");

                                            
                                            foreach (var item in List1)
                                            {
                                                if (item[0] == int.Parse(textId.Text))
                                                {
                                                    if ((item[1] - 1) <= 0)
                                                    {
                                                        List1.Remove(item);
                                                    }
                                                    else item[1] -= 1;
                                                }
                                            }
                                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        };
                                        panelKolRed.Children.Add(buttonYmensh);


                                        textBoxKolTovara.Width = 50;
                                        textBoxKolTovara.FontSize = 20;
                                        textBoxKolTovara.Text = "1";
                                        textBoxKolTovara.VerticalAlignment = VerticalAlignment.Center;
                                        textBoxKolTovara.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                                        textBoxKolTovara.Name = "KolTov";
                                        textBoxKolTovara.IsReadOnly = true;
                                        textBoxKolTovara.PreviewKeyDown += (sender, e) =>
                                        {
                                            double kol = 0;
                                            if (e.Key == Key.Enter)
                                            {
                                                

                                                int kolTov = int.Parse(textBoxKolTovara.Text);
                                                kol += kolTov;
                                                textPrise.Text = (DbTovar.Price_sht * kolTov).ToString() + " ₽";
                                                if (kolTov == 0) KorzinaSpisok.Children.Remove(panelKorzonaStroka);

                                                foreach (var item in List1)
                                                {
                                                    if (item[0] == int.Parse(textId.Text))
                                                    {
                                                        if ((item[1] + kolTov) <= 0)
                                                        {
                                                            
                                                            List1.Remove(item);
                                                        }
                                                        else
                                                        {
                                                            item[1] = kolTov;
                                                        }
                                                    }
                                                }
                                            }
                                        };
                                        ints1[1] = 1;
                                        List1.Add(ints1);
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
                                            SumKOplate += DbTovar.Price_sht;
                                            ItogoOutBlock.Text = SumKOplate.ToString() + " ₽";
                                            textPrise.Text = (DbTovar.Price_sht * int.Parse(textBoxKolTovara.Text.Replace(" ₽", ""))).ToString() + " ₽";
                                            counter += 1;
                                            HintAssist.SetHint(KolTovObsh, $"Товары, {counter} шт.");

                                            foreach (var item in List1)
                                            {
                                                if (item[0] == int.Parse(textId.Text)) 
                                                {
                                                    item[1] += 1; 
                                                }
                                               
                                            }

                                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

                                        SumKOplate += DbTovar.Price_sht;
                                        ItogoOutBlock.Text = SumKOplate.ToString() + " ₽";

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
                                      .Where(x => x.Category.Name == content)
                                      .Include(x => x.Category);

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
                                        textBlockNameTovar.Width = 100;//
                                        textBlockNameTovar.Height = 50;//
                                        textBlockNameTovar.TextWrapping = TextWrapping.Wrap;//
                                        stackPanelTovar.Children.Add(textBlockNameTovar);

                                        TextBlock textBlockKolTovara = new TextBlock();
                                        textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                                        textBlockKolTovara.FontSize = 15;
                                        stackPanelTovar.Children.Add(textBlockKolTovara);

                                        TextBlock blockPriseSht = new TextBlock();//
                                        blockPriseSht.Text = item.Price_sht.ToString() + " ₽";//
                                        blockPriseSht.FontSize = 15;//
                                        stackPanelTovar.Children.Add(blockPriseSht);//

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
                                        textBlockNameTovar.Width = 100;//
                                        textBlockNameTovar.Height = 50;//
                                        textBlockNameTovar.TextWrapping = TextWrapping.Wrap;//
                                        stackPanelTovar.Children.Add(textBlockNameTovar);
                                        TextBlock textBlockKolTovara = new TextBlock();
                                        textBlockKolTovara.Text = item.Count_tekyshee.ToString() + " шт.";
                                        textBlockKolTovara.FontSize = 15;
                                        blocIdtov.Visibility = Visibility.Collapsed;
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
                    MessageBox.Show("Поле 'Категория' должно быть заполнено!", "Ошибка");
                    return;
                }
                if (string.IsNullOrEmpty(CenaNewTovara.Text))
                {
                    MessageBox.Show("Поле 'Цена' должно быть заполнено!", "Ошибка");
                    return;
                }
                if (string.IsNullOrEmpty(NameNewTovara.Text))
                {
                    MessageBox.Show("Поле 'Название' должно быть заполнено!", "Ошибка");
                    return;
                }
                if (string.IsNullOrEmpty(KolNewInputBox.Text))
                {
                    MessageBox.Show("Поле 'Кол-во' должно быть заполнено!", "Ошибка");
                    return;
                }
                if (PhotoFileNeme is null)
                {
                    MessageBox.Show("Фотографии товара не выбрана!", "Ошибка");
                    return;
                }
                foreach (char s in CenaNewTovara.Text.Replace("-",""))
                {
                    if (!char.IsDigit(s)) 
                    {
                        MessageBox.Show("Поле поддерживает только числовые значения", "Ошибка установки цены товара");
                        CenaNewTovara.Text = "";
                        return;
                    }  
                }
                foreach (char s in KolNewInputBox.Text.Replace("-", ""))
                {
                    if (!char.IsDigit(s))
                    {
                        MessageBox.Show("Поле поддерживает только числовые значения", "Ошибка установки количества товара");
                        KolNewInputBox.Text = "";
                        return;
                    }
                }

                string neme = NameNewTovara.Text;
                string categor = CategOutputBox.Text;
                int kol = int.Parse(KolNewInputBox.Text);
                double cena = double.Parse(CenaNewTovara.Text);

                if (kol < 1)
                {
                    MessageBox.Show("Недопустимое значение поля 'Кол-во'!", "Ошибка");
                    return;
                }
                if (cena < 0.01)
                {
                    MessageBox.Show("Недопустимое значение поля 'Цена'!", "Ошибка");
                    return;
                }

                using (var context = new DbContact())
                {
                    var NotTovar = context.tovar.SingleOrDefault(x => x.Name_tovar == neme && x.Category.Name == categor);
                    if (NotTovar != null) { MessageBox.Show("Такой товар уже существут!", "Ошибка"); return; }

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

                    KatalogSpisok.Content = "";
                    VuvodRadioButton();

                    MessageBox.Show("Категория добавлена!");
                }

            }  // категория
            
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

                VuvodTovarov();
                
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
            VuvodTovarov();

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
            double kOplate = double.Parse(ItogoOutBlock.Text.Replace(" ₽", ""));
            double koshelok_ = double.Parse(Koshelek.Text.Replace(" ₽", ""));
            if (kOplate >= koshelok_) 
            {
                MessageBox.Show("Недостаточно средств", "Ошибка");
                return;
            }

            using (var context = new DbContact())
            {
                foreach (var tovarMas in List1)
                {
                    foreach (var tovar_Sklad in context.tovar)
                    {
                        if (tovarMas[0] == tovar_Sklad.Id)
                        {
                            var tovarDb = context.tovar.Find(tovarMas[0]);
                            tovarDb.Count_tekyshee += tovarMas[1];
                        }
                    }
                }
                var user_ = context.users.Find(_user.ID);
                user_.Money -= kOplate;
                context.SaveChanges();
            }
            List1.Clear();
            KorzinaSpisok.Children.Clear();
            koshelok_ -= kOplate;
            Koshelek.Text =koshelok_ + " ₽";

            ItogoOutBlock.Text = "0 ₽";
            HintAssist.SetHint(KolTovObsh, "Товары, 0 шт.");


            TextBlock blockKorzina = new TextBlock();
            blockKorzina.Text = "Корзина";
            blockKorzina.FontSize = 20;
            blockKorzina.Margin = new Thickness(15, 5, 0, 20);
            WrapPanel wrapPanel = FindName("KorzinaSpisok") as WrapPanel;
            wrapPanel.Children.Add(blockKorzina);
        }
    }
}
