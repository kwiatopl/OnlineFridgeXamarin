using SQLite;

namespace test.DataAccess.Model
{
    public class User
    {
        [PrimaryKey, Unique, AutoIncrement]
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}