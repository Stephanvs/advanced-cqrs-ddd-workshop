using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Models
{
    public class OrderDocument
    {
        public string Waiter { get; set; }

        public Guid OrderNumber { get; set; }

        public bool Paid { get; set; }

        public bool IsDodgyCustomer { get; set; }

        public List<LineItem> LineItems { get; set; } = new List<LineItem>();

        public List<string> Ingredients { get; set; } = new List<string>();

        public string CookName { get; set; }

        public double CookingMinutes { get; set; }

        public double TotalPrice => LineItems.Sum(c => c.Price * c.Quantity);
    }
}
