using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Order
{

    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string Street { get; set; }
    public string RestaurantName { get; set; }
    public DateTime OrderDateTime { get; set; }
    public string Status { get; set; }          // "New" vagy "Delivered"
    public string PaymentMethod { get; set; }   // "Online" vagy "Cash" vagy "Card"
    public int TotalPrice { get; set; }

    public override string ToString()
    {
        return $"Id: {Id} - Name: {CustomerName}";
    }
}
