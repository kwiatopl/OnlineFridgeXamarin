﻿
namespace OnlineFridge.DataAccess.Model
{
    public class User
    {
        public int userId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string passwordHash { get; set; }
        public string salt { get; set; }
        public string username { get; set; }
    }
}