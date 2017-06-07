

namespace test.DataAccess.Model
{
    public class ProductOnline
    {
        public int productId { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public Quantity unit { get; set; }
        public string expDate { get; set; }
        public int userId { get; set; }
        public bool inShoppingList { get; set; }
    }

    public enum Quantity
    {
        kg,
        dek,
        g,
        l,
        ml,
        sztuk
    }

}