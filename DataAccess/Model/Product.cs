using SQLite;

namespace test.DataAccess.Model
{
    public class Product 
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int productId { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public Quantity unit { get; set; }
        public string expDate { get; set; }
        public int userId { get; set; }
        public bool inShoppingList { get; set; }

        public override string ToString()
        {
            return name + ", "+ count + " " + unit + ", Data ważności:" + expDate ;
        }
    }
 
    
}