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
            Orders = FileReader.Load("orders.txt");

            // Itt van nehany LINQ pelda
            var newOrders = Orders.Where(x => x.Status == "New").ToList();
            var today = Orders.Where(x => x.OrderDateTime.Date == DateTime.Today);
            var maxOrder = Orders.OrderByDescending(x => x.TotalPrice).First();
            var byRestaurant = Orders.GroupBy(x => x.RestaurantName);
        }
    }
}