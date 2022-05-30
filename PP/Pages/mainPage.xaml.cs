﻿using System;
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
using PP.Database;

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

        public mainPage()
        {
            InitializeComponent();
            LoadProductID();
            units = connection.Unit.ToList();
            DataContext = this;
            
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
          
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //ПОКАЗАТЬ ДАННЫЕ
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)//УДАЛИТЬ ИЗОБРАЖЕНИЕ
        {

        }
    }
}
