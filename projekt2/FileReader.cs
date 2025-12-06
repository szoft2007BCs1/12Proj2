using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class FileReader
{

    public static List<Order> Load(string filePath)
    {
        return File.ReadAllLines(filePath)
            .Skip(1)
            .Select(line => line.Split(';'))
            .Select(parts => new Order
            {
                Id = int.Parse(parts[0]),
                CustomerName = parts[1],
                CustomerPhone = parts[2],
                Street = parts[3],
                RestaurantName = parts[4],
                OrderDateTime = DateTime.ParseExact(parts[5], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Status = parts[6],
                PaymentMethod = parts[7],
                TotalPrice = int.Parse(parts[8])

            })
            .ToList();
    }

}
