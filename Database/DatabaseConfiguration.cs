using BhawanaPatra.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace BhawanaPatra.Database
{
    public class DatabaseConfiguration
    {
       
        private readonly SQLiteConnection _db;

        public DatabaseConfiguration(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<UserModel>();
            _db.CreateTable<EntryModel>();

        }
        public UserModel? GetUser(string username)
        {
            return _db.Table<UserModel>().FirstOrDefault(u => u.Username == username);
        }

        public void RegisterUser(UserModel user)
        {
            _db.Insert(user);
        }

        public EntryModel? GetEntryByDate(int userId, string dateKey)
          => _db.Table<EntryModel>()
                .FirstOrDefault(e => e.UserId == userId && e.EntryDateKey == dateKey);

        public List<EntryModel> GetEntriesByUser(int userId)
            => _db.Table<EntryModel>()
                  .Where(e => e.UserId == userId)
                  .OrderByDescending(e => e.EntryDateKey)
                  .ToList();

        public void InsertEntry(EntryModel entry) => _db.Insert(entry);
        public void UpdateEntry(EntryModel entry) => _db.Update(entry);
        public void DeleteEntry(EntryModel entry) => _db.Delete(entry);


    }

        
    }



