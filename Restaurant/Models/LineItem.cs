namespace Restaurant.Models
{
    public class LineItem
    {
        public LineItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}