using System;
using System.IO;
using SQLite;
using test.DataAccess.Model;
using Environment = System.Environment;

namespace test.DataAccess
{
    public class FridgeDb : IDisposable
    {
        private string _dbPath;
        private string DbPath {
            get { return _dbPath ?? (_dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "fridgeDb.db3")); }
        }

        private SQLiteConnection Db { get; set; }

        public FridgeDb()
        {
            Db = new SQLiteConnection(DbPath);
            Db.CreateTable<Product>();
        }

        public TableQuery<T> Table<T>() where T : new()
        {
            return Db.Table<T>();
        }

        public int Insert(object objectToInsert)
        {
            return Db.Insert(objectToInsert);
        }

        public int Update(object objectToUpdate)
        {
            return Db.Update(objectToUpdate);
        }

        public int Delete(object objectToDelete)
        {
            return Db.Delete(objectToDelete);
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}