using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Order> Orders { get; set; }

        static bool isInt(string bemenet)
        {
            int kimenet = 0;
            bool tmp = int.TryParse(bemenet, out kimenet);
            return tmp;
        }

        List<string> filekimenet = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            Orders = FileReader.Load("orders.txt");

            foreach (var item in File.ReadAllLines("orders.txt"))
                filekimenet.Add(item);

            // Itt van nehany LINQ pelda
            var newOrders = Orders.Where(x => x.Status == "New").ToList();
            var today = Orders.Where(x => x.OrderDateTime.Date == DateTime.Today);
            if(today.Count() > 0)
            {
                lbl_mai.Content = $"{today.First().ToString()} - Dátum: {today.First().OrderDateTime}";
            }
            var maxOrder = Orders.OrderByDescending(x => x.TotalPrice).First();
            var minOrder = Orders.OrderBy(x => x.TotalPrice).First();
            var byRestaurant = Orders.GroupBy(x => x.RestaurantName);

            ComboBox_mindenes.Items.Clear();
            ComboBox_torles.Items.Clear();

            ComboBox_mindenes.Items.Add("New");
            foreach (var item in Orders)
            {
                ComboBox_mindenes.Items.Add(item);
                ComboBox_torles.Items.Add(item);
            }

            btn_add.Visibility = Visibility.Hidden;

            ComboBox_mindenes.SelectedIndex = 1;
            ComboBox_torles.SelectedIndex = 0;

            lbl_ossz.Content = $"Össz rendelések száma: {Orders.Count}";
            lbl_maxar.Content = $"{maxOrder.ToString()} - Ár: {maxOrder.TotalPrice}";
            lbl_minar.Content = $"{minOrder.ToString()} - Ár: {minOrder.TotalPrice}";

        }

        private void ComboBox_mindenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_mindenes.SelectedItem is Order selectedOrder)
            {
                btn_add.Visibility = Visibility.Hidden;

                txtblock_id.Text = $"{selectedOrder.Id}";
                txtblock_name.Text = $"{selectedOrder.CustomerName}";
                txtblock_phone.Text = $"{selectedOrder.CustomerPhone}";
                txtblock_street.Text = $"{selectedOrder.Street}";
                txtblock_restaurant.Text = $"{selectedOrder.RestaurantName}";
                txtblock_time.Text = $"{selectedOrder.OrderDateTime}";
                txtblock_time.IsEnabled = true;

                if (selectedOrder.Status == "New")
                    chckbox_status.IsChecked = true;
                else
                    chckbox_status.IsChecked = false;

                foreach (ComboBoxItem item in ComboBox_method.Items)
                {
                    if (item.Content.ToString() == selectedOrder.PaymentMethod)
                    {
                        ComboBox_method.SelectedItem = item;
                        break;
                    }
                }
                txtblock_price.Text = $"{selectedOrder.TotalPrice}";
            }
            else
            {
                string Mai = DateTime.Now.ToString("u");
                btn_add.Visibility = Visibility.Visible;

                txtblock_id.Text = $"{Orders.Count + 1}";
                txtblock_name.Text = "Név";
                txtblock_phone.Text = "Telefonszám";
                txtblock_street.Text = "Utca";
                txtblock_restaurant.Text = "Étterem";
                txtblock_time.Text = Mai.Remove(Mai.Length - 4);
                txtblock_time.IsEnabled = false;

                chckbox_status.IsChecked = false;
                ComboBox_method.SelectedIndex = 0;
                txtblock_price.Text = "Ár";
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if(ComboBox_mindenes.SelectedItem == "New")
            {
                if (isInt(txtblock_price.Text))
                {
                    string tmp = "";
                    if (ComboBox_method.SelectedItem is ComboBoxItem selectedItem)
                    {
                        tmp = selectedItem.Content.ToString();
                    }
                    string status = "";
                    if (chckbox_status.IsChecked == true)
                        status = "New";
                    else
                        status = "Delivered";
                    string fileszoveg = $"{txtblock_id.Text};{txtblock_name.Text};{txtblock_phone.Text};{txtblock_street.Text};{txtblock_restaurant.Text};{txtblock_time.Text};{status};{tmp};{txtblock_price.Text}";

                    //Ez a nem túl jó megoldás
                    //File.AppendAllText("orders.txt", $"\n{fileszoveg}");

                    //Ez a javított fájl írás. Itt teljesen újra írja a "orders-txt" file-t
                    filekimenet.Add(fileszoveg);
                    File.WriteAllLines("orders.txt", filekimenet);

                    Orders = FileReader.Load("orders.txt");
                    ComboBox_mindenes.Items.Clear();
                    ComboBox_torles.Items.Clear();

                    ComboBox_mindenes.Items.Add("New");
                    foreach (var item in Orders)
                    {
                        ComboBox_mindenes.Items.Add(item);
                        ComboBox_torles.Items.Add(item);
                    }

                    ComboBox_mindenes.SelectedIndex = filekimenet.Count-1;

                    var maxOrder = Orders.OrderByDescending(x => x.TotalPrice).First();
                    var minOrder = Orders.OrderBy(x => x.TotalPrice).First();

                    lbl_ossz.Content = $"Össz rendelések száma: {Orders.Count}";
                    lbl_maxar.Content = $"{maxOrder.ToString()} - Ár: {maxOrder.TotalPrice}";
                    lbl_minar.Content = $"{minOrder.ToString()} - Ár: {minOrder.TotalPrice}";

                    var today = Orders.Where(x => x.OrderDateTime.Date == DateTime.Today);
                    if (today.Count() > 0)
                    {
                        lbl_mai.Content = $"{today.First().ToString()} - Dátum: {today.First().OrderDateTime}";
                    }
                }
                else
                    MessageBox.Show("Nem jól van megadva az Ár");
            }
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_mindenes.SelectedItem is Order selectedOrder)
            {
                filekimenet.RemoveAt(ComboBox_torles.SelectedIndex+1);
                File.WriteAllLines("orders.txt", filekimenet);

                Orders = FileReader.Load("orders.txt");
                ComboBox_mindenes.Items.Clear();
                ComboBox_torles.Items.Clear();

                ComboBox_mindenes.Items.Add("New");
                foreach (var item in Orders)
                {
                    ComboBox_mindenes.Items.Add(item);
                    ComboBox_torles.Items.Add(item);
                }

                ComboBox_mindenes.SelectedIndex = 1;
                ComboBox_torles.SelectedIndex = 0;

                var maxOrder = Orders.OrderByDescending(x => x.TotalPrice).First();
                var minOrder = Orders.OrderBy(x => x.TotalPrice).First();

                lbl_ossz.Content = $"Össz rendelések száma: {Orders.Count}";
                lbl_maxar.Content = $"{maxOrder.ToString()} - Ár: {maxOrder.TotalPrice}";
                lbl_minar.Content = $"{minOrder.ToString()} - Ár: {minOrder.TotalPrice}";

                var today = Orders.Where(x => x.OrderDateTime.Date == DateTime.Today);
                if (today.Count() > 0)
                {
                    lbl_mai.Content = $"{today.First().ToString()} - Dátum: {today.First().OrderDateTime}";
                }
            }
        }
    }
}