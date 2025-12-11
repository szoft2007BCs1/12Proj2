using System;
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

        public MainWindow()
        {
            InitializeComponent();

            Orders = FileReader.Load("orders.txt");

            // Itt van nehany LINQ pelda
            var newOrders = Orders.Where(x => x.Status == "New").ToList();
            var today = Orders.Where(x => x.OrderDateTime.Date == DateTime.Today);
            var maxOrder = Orders.OrderByDescending(x => x.TotalPrice).First();
            var byRestaurant = Orders.GroupBy(x => x.RestaurantName);

            ComboBox_mindenes.Items.Clear();
            ComboBox_mindenes.Items.Add("New");
            foreach (var item in Orders)
                ComboBox_mindenes.Items.Add(item);

            btn_add.Visibility = Visibility.Hidden;
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
                btn_add.Visibility = Visibility.Visible;

                txtblock_id.Text = $"{Orders.Count + 1}";
                txtblock_name.Text = "Név";
                txtblock_phone.Text = "Telefonszám";
                txtblock_street.Text = "Utca";
                txtblock_restaurant.Text = "Étterem";
                txtblock_time.Text = "Idő";
                chckbox_status.IsChecked = false;
                ComboBox_method.SelectedIndex = 0;
                txtblock_price.Text = "Ár";
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if(ComboBox_mindenes.SelectedItem == "New")
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
                File.AppendAllText("orders.txt", $"\n{fileszoveg}");

                Orders = FileReader.Load("orders.txt");
                ComboBox_mindenes.Items.Clear();
                ComboBox_mindenes.Items.Add("New");
                foreach (var item in Orders)
                    ComboBox_mindenes.Items.Add(item);
            }
        }
    }
}