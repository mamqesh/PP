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

        public mainPage()
        {
            InitializeComponent();
            LoadProductID();
            LoadUnits();
            LoadProductsInListView();
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
            //int countSymbol = textBlockProductNote.Text.Trim();
            //if (countSymbol.Length>10)
            //{
            //    textBlockProductNote.Text = +"\t";
            //}
        }
        void LoadProductID()
        {
            int productID = connection.Product.ToList().Count()+1;
            textBoxIDProduct.Text = productID.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)//ДОБАВИТЬ ПРОДУКЦИЮ
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
            }
            if (numberProduct.Length == 0 || nameProduct.Length == 0||countProduct.Length==0)
            {
                MessageBox.Show("Не все данные введенны");
                return;
            }
            else
            {
                Database.Product product = new Database.Product();
                product.ProductID = int.Parse(idProduct);
                product.ProductNumber = numberProduct;
                product.ProductName = nameProduct;
                product.Count = countProduct;
                product.Unit1 = comboBoxUnitName.SelectedItem as Unit;
                product.ProductNote = noteProduct;
                product.Image = 1;
                connection.Product.Add(product);
                int result = connection.SaveChanges();
                if (result == 1)
                {
                    ClearText();
                    MessageBox.Show("Данные добавлены");
                }
                else
                {
                    MessageBox.Show("Ошибка добавления");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//ДОБАВИТЬ ИЗОБРАЖНИЕ
        {
            f.OpenFileDialog openFileDialog = new f.OpenFileDialog();
            openFileDialog.Filter = "Формат изображения | *.png; *.jpg;";
            if (openFileDialog.ShowDialog()==f.DialogResult.OK)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(openFileDialog.FileName, UriKind.Relative);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.DecodePixelHeight = 256;
                bitmapImage.EndInit();
                GetImage(bitmapImage);
            }
        }
        void GetImage (BitmapImage bitmapImage)
        {
            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            StringBuilder stringBuilder = new StringBuilder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            pngBitmapEncoder.Save(memoryStream);
            byte[] imgByte = memoryStream.ToArray();
            foreach (byte _imgByte in imgByte)
                stringBuilder.Append(_imgByte).Append(";");
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            GetImageDatabase(stringBuilder.ToString());
          
        }
        void GetImageDatabase(string getImage)
        {
            Database.Image image = new Database.Image();
             image.ImageID = connection.Image.ToList().Count() + 1;
            image.Image1 = getImage;
            connection.Image.Add(image);
            int result = connection.SaveChanges();
            if (result!=0)
            {
                MessageBox.Show("Изображение добавлено");
            }
            else
            {
                MessageBox.Show("Ошибка добавления изображения");
            }
        }
        //void GetImageInWindow(string ID, string ByteGet)
        //{
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    image.UriSource = new Uri(stringBuilder, UriKind.Relative);
        //    image.EndInit();
        //    imageProductPhoto.Source = image;
        //    26:30
        //}
        private void Button_Click_2(object sender, RoutedEventArgs e) //ПОКАЗАТЬ ДАННЫЕ
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)//УДАЛИТЬ ИЗОБРАЖЕНИЕ
        {

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
                //LoadProductsInListView();
            }

        }
    }
}
