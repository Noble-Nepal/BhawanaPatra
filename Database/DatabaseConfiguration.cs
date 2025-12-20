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
            
        }
        public UserModel? GetUser(string username)
        {
            return _db.Table<UserModel>().FirstOrDefault(u => u.Username == username);
        }

        public void RegisterUser(UserModel user)
        {
            _db.Insert(user);
        }


    }

        
    }



