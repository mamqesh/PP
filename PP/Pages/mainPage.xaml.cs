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
using System.IO;
using System.Drawing;
using System.Data.Entity;
using Microsoft.Win32;
using PP.Database;
using f = System.Windows.Forms;

namespace PP.Pages
{
    /// <summary>
    /// Логика взаимодействия для mainPage.xaml
    /// </summary>

    public partial class mainPage : Page
    {
        danil3Entities connection = new danil3Entities();
        public List<Database.Unit> units { get; set; }
        public Database.Product product { get; set; }
        public List<Database.Product> products { get; set; }
        string imageAdress = "";
        int productID = 0;
        public mainPage()
        {
            InitializeComponent();
            LoadProductID();
            LoadUnits();
            LoadProductsInListView();
            products = connection.Product.ToList();
            DataContext = this;
        }
        void LoadUnits()
        {
            units = connection.Unit.ToList();
        }
        void LoadProductsInListView()
        {
            var currentProduct = connection.Product.ToList();
            listViewProducts.ItemsSource = currentProduct;
        }
        void LoadProductID()
        {
            productID = connection.Product.ToList().Count() + 1;
            var proudctIDS = connection.Product.ToList();
            foreach (var _productsIDS in proudctIDS)
            {
                if (_productsIDS.ProductID == productID)
                {
                    productID++;
                }
            }
            textBoxIDProduct.Text = productID.ToString();
        }
        void LoadProducts()
        {
            textBoxViewIDProduct.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            textBoxViewNumberProduct.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            textBoxViewNameProduct.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            textBoxViewCountProduct.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            textBoxViewNoteProduct.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            comboBoxViewUnitProduct.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateTarget();
            listBoxViewProducts.GetBindingExpression(ListBox.ItemsSourceProperty)?.UpdateTarget();
        }
        void DeleteImage()
        {
            imageAdress = "";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("/Resources/picture.png", UriKind.Relative);
            image.EndInit();
            imageProductPhoto.Source = image;
        }
        private void Button_Click(object sender, RoutedEventArgs e)//ДОБАВИТЬ ПРОДУКЦИЮ
        {
            try
            {
                if (textBoxCountProduct.Text.Length > 0 && textBoxIDProduct.Text.Length > 0 && textBoxNameProduct.Text.Length > 0)
                {
                    if (comboBoxUnitName.SelectedIndex != -1)
                    {
                        string idProduct = textBoxIDProduct.Text.Trim();
                        string numberProduct = textBoxNumberProduct.Text.Trim();
                        string nameProduct = textBoxNameProduct.Text.Trim();
                        string countProduct = textBoxCountProduct.Text.Trim();
                        string noteProduct = textBoxNoteProduct.Text.Trim();
                        void ClearText()
                        {
                            numberProduct = "";
                            nameProduct = "";
                            countProduct = "";
                            noteProduct = "";
                            textBoxNumberProduct.Clear();
                            textBoxNameProduct.Clear();
                            textBoxCountProduct.Clear();
                            textBoxNoteProduct.Clear();
                            comboBoxUnitName.SelectedIndex = -1;
                        }
                        if (nameProduct.Length == 0 || countProduct.Length == 0)
                        {
                            MessageBox.Show("Не все данные введенны", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            int imageID = connection.Image.ToList().Count() + 1;
                            Database.Image image = new Database.Image();
                            image.ImageID = imageID;
                            image.Image1 = imageAdress;
                            connection.Image.Add(image);
                            connection.SaveChanges();
                            Database.Product product = new Database.Product();
                            product.ProductID = int.Parse(idProduct);
                            product.ProductNumber = numberProduct;
                            product.ProductName = nameProduct;
                            product.Count = countProduct;
                            product.Unit1 = comboBoxUnitName.SelectedItem as Unit;
                            product.ProductNote = noteProduct;
                            product.Image = imageID;
                            connection.Product.Add(product);
                            int result = connection.SaveChanges();
                            if (result == 1)
                            {
                                ClearText();
                                LoadProductID();
                                LoadProductsInListView();
                                products = connection.Product.ToList();
                                LoadProducts();
                                DeleteImage();
                                MessageBox.Show("Данные добавлены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Единица измерения не выбрана", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                if (productID==product.ProductID)
                {
                    LoadProductID();
                }
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)//ДОБАВИТЬ ИЗОБРАЖНИЕ
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                if ((bool)openFile.ShowDialog())
                {
                    imageAdress = openFile.FileName;
                    imageProductPhoto.Source = new BitmapImage(new Uri(openFile.FileName, UriKind.Absolute))
                    {
                        CreateOptions = BitmapCreateOptions.IgnoreImageCache
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //f.OpenFileDialog openFileDialog = new f.OpenFileDialog();
            //openFileDialog.Filter = "Формат изображения | *.png; *.jpg;";
            //if (openFileDialog.ShowDialog() == f.DialogResult.OK)
            //{
            //    BitmapImage bitmapImage = new BitmapImage();
            //    bitmapImage.BeginInit();
            //    bitmapImage.UriSource = new Uri(openFileDialog.FileName, UriKind.Relative);
            //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapImage.DecodePixelHeight = 256;
            //    bitmapImage.EndInit();
            //    GetImage(bitmapImage);
            //}
        }
        //void GetImage(BitmapImage bitmapImage)
        //{
        //    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
        //    MemoryStream memoryStream = new MemoryStream();
        //    StringBuilder stringBuilder = new StringBuilder();
        //    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        //    pngBitmapEncoder.Save(memoryStream);
        //    byte[] imgByte = memoryStream.ToArray();
        //    foreach (byte _imgByte in imgByte)
        //    {
        //        stringBuilder.Append(_imgByte).Append(";");
        //    }
        //    stringBuilder.Remove(stringBuilder.Length - 1, 1);
        //    GetImageDatabase(stringBuilder.ToString());

        //}
        //void GetImageDatabase(string getImage)
        //{
        //    try
        //    {
        //        Database.Image image = new Database.Image();
        //        image.ImageID = connection.Image.ToList().Count() + 1;
        //        image.Image1 = getImage;
        //        connection.Image.Add(image);
        //        int result = connection.SaveChanges();
        //        if (result != 0)
        //        {
        //            MessageBox.Show("Изображение добавлено");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}
        //void GetImageInWindow(string ID, string ByteGet)
        //{
        //    byte[] imageByte = ByteGet.Split(';').Select(a => byte.Parse(a)).ToArray();
        //    MemoryStream memoryStream = new MemoryStream(imageByte);
        //    imageProductPhoto.Source = BitmapFrame.Create(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        //    imageProductPhoto.Style = FindResource("Image") as Style;
        //    imageProductPhoto.DataContext = ID;
        //}
        private void Button_Click_3(object sender, RoutedEventArgs e)//УДАЛИТЬ ИЗОБРАЖЕНИЕ
        {
            DeleteImage();
        }
        private void ImageSource_BadgeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
        private void Button_Click_4(object sender, RoutedEventArgs e)//ОБНОВИТЬ
        {
            LoadProductsInListView();
        }
        private void textBoxSearchProduct_TextChanged(object sender, TextChangedEventArgs e)//ПОИСК 
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                var textForSearch = textBoxSearchProduct.Text.Trim();
                products = connection.Product.Where(p => DbFunctions.Like(p.ProductName, "%" + textForSearch + "%") || DbFunctions.Like(p.ProductNumber, "%" + textForSearch + "%")).ToList();
                listViewProducts.ItemsSource = products;
            }

        }
        private void listBoxViewProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                product = listBoxViewProducts.SelectedItem as Database.Product;
                LoadProducts();
                if (listBoxViewProducts.SelectedItem!=null)
                {
                    string imageView = product.Image1.Image1;
                    if (imageView == "")
                    {
                        imageProductView.Source = new BitmapImage(new Uri("../Resources/picture.png", UriKind.Relative));
                    }
                    else
                    {
                        imageProductView.Source = new BitmapImage(new Uri(imageView, UriKind.Absolute));
                    }
                }
                else
                {
                    imageProductView.Source = new BitmapImage(new Uri("../Resources/picture.png", UriKind.Relative));
                }
              
            }
            catch (Exception ex)
            {
                imageProductView.Source = new BitmapImage(new Uri("../Resources/picture.png", UriKind.Relative));
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                int result = connection.SaveChanges();
                if (result == 1)
                {
                    MessageBox.Show("Данные успешно отредактированы", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(), "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)//УДАЛИТЬ ДАННЫЕ
        {
            var productDelete = listBoxViewProducts.SelectedItem as Database.Product;

            if (MessageBox.Show("Вы действительно хотите безвозвратно удалить данные?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    connection.Product.Remove(productDelete);
                    int result = connection.SaveChanges();
                    if (result > 0)
                    {
                        LoadProductsInListView();
                        products = connection.Product.ToList();
                        LoadProducts();
                        listBoxViewProducts.SelectedItem = null;
                        imageProductView.Source = new BitmapImage(new Uri("../Resources/picture.png", UriKind.Relative));
                        MessageBox.Show("Данные безвозвратно удалены!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void textBoxViewSearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                var text = textBox.Text.Trim();
                products = connection.Product.Where(p => DbFunctions.Like(p.ProductName, "%" + text + "%")).ToList();
                LoadProducts();
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string addUnit = textBoxAddUnit.Text.Trim();
                if (addUnit.Length > 0)
                {
                    Database.Unit unit = new Database.Unit();
                    unit.UnitID = connection.Unit.ToList().Count() + 1;
                    unit.UnitName = addUnit;
                    connection.Unit.Add(unit);
                    int result = connection.SaveChanges();
                    if (result > 0)
                    {
                        textBoxAddUnit.Clear();
                        MessageBox.Show("Данные добавлены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Введите данные", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
