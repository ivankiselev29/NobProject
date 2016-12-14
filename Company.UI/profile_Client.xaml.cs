﻿using Company.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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

namespace Company.UI
{
    /// <summary>
    /// Логика взаимодействия для profile.xaml
    /// </summary>
    public partial class profile_Client : Window
    {



        public profile_Client()
        {
            InitializeComponent();
        }
        Dictionary<string, string> dictClientNames = new Dictionary<string, string>();

        Client client = new Client();
        Admin admin = new Admin();
        Repository repo = new Repository();
        List<Catalogue> ListCatalogue = new List<Catalogue>();
        List<string> ListBasket = new List<string>();
        List<string> ListMyOrder = new List<string>();

        private void button_show_catalogue_Click(object sender, RoutedEventArgs e)
        {
            using (var c = new Context())
            {
                //скачивание данных из базы и показ в листвью каталога
                listView_myCatalogue.Items.Clear();
                listView_myCatalogue.Items.Refresh();

                ListCatalogue = c.Catalogue.ToList();

                foreach (Catalogue item in ListCatalogue)
                {
                    //ListViewItem boxitem = new ListViewItem();
                    listView_myCatalogue.Items.Add(item.ItemName + " " + item.Price);
                }
            }
        }

        private void listView_myCatalogue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //добавление содержимого из списка каталога в корзину

            foreach (var item in listView_myCatalogue.SelectedItems)
            {
                //добавляю данные в листбокс заказов
                list_myBasket.Items.Add(item);

                //все данные в строку и отправляю их в лист
                var a = item.ToString();
                ListBasket.Add(a);

                MessageBox.Show("was added");
            }
        }


        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            //удалить заказ (удаляет весь список, позже можно настроить, чтобы поштучно удалял)
            list_myBasket.Items.Clear();
            ListBasket.Clear();
        }

        private void order_botton_Click(object sender, RoutedEventArgs e)
        {

            Catalogue itemname = new Catalogue();
            //оформить заказ (отправляю новые данные в таблицу бд Orders)
            int purchase = 0;
            //Разделяю данные в ListOrders
            using (var c = new Context())
            {
                foreach (string item in ListBasket)
                {
                    //отрываю название от цены)
                    string[] a = item.Split(' ');
                    int price = Convert.ToInt32(a[1]);
                    itemname.ItemName = a[0];
                    ListMyOrder.Add(item);
                    listBox_orders.Items.Add(a[0] + " " + price.ToString() + "$");


                    //ИЗБЕЖАТЬ ДУБЛИКАТА

                    c.Orders.Add(new Orders
                    {
                        ItemName = itemname,
                        Cost = price
                    });
                    c.SaveChanges();
                    purchase += price;
                }
                totalCost.Content = purchase.ToString() + "$";
                MessageBox.Show("Заказано");
                list_myBasket.Items.Clear();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in repo.DeletedItems)
            {
                var a = item.ToString();
                MessageBox.Show(a);
            }
        }

        //экспорт в текстовый файл
        private void btn_exl_Click(object sender, RoutedEventArgs e)
        {
            using (TextWriter tw = new StreamWriter("C:\\Users\\Ivan\\Desktop\\list.txt"))
            {
                foreach (var item in ListMyOrder)
                {
                    string[] a = item.Split(' ');
                    string str1 = a[0];
                    string str2 = a[1];
                    tw.WriteLine(str1 + " " + str2 + "$");
                }
                MessageBox.Show("Done");
            }

        }
    }
}

