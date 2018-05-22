namespace ORMExample.Models
{
    public class Product
    {
        public Product() { }

        public Product(int id, string name, int price)
        {
            ProductID = id;
            Name = name;
            Price = price;
        }

        public int ProductID { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public override string ToString()
        {
            return "ProductID: " + ProductID + " Name: " + Name + " Price " + Price;
        }
    }
}
