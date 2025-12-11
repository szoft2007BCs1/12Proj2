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

            foreach (var item in Orders)
                ComboBox_mindenes.Items.Add(item);

        }

        private void ComboBox_mindenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtblock_id.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].Id}";
            txtblock_name.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].CustomerName}";
            txtblock_phone.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].CustomerPhone}";
            txtblock_street.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].Street}";
            txtblock_restaurant.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].RestaurantName}";
            txtblock_time.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].OrderDateTime}";
            if (Orders[ComboBox_mindenes.SelectedIndex].Status == "New")
                chckbox_status.IsChecked = true;
            else
                chckbox_status.IsChecked = false;

            foreach (ComboBoxItem item in ComboBox_method.Items)
            {
                if (item.Content.ToString() == Orders[ComboBox_mindenes.SelectedIndex].PaymentMethod)
                {
                    ComboBox_method.SelectedItem = item;
                    break;
                }
            }
            txtblock_price.Text = $"{Orders[ComboBox_mindenes.SelectedIndex].TotalPrice}";
        }
    }
}