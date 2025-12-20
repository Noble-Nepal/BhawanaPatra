using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System;

namespace BhawanaPatra.Models
{
    [Table("users")]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("user_id")]
        public int Id { get; set; }

        [Column("username")]
       
        [Unique]
        [MaxLength(50)]
        [NotNull]
        public  string Username { get; set; } = string.Empty;

        [Column("password_hash")]
        [NotNull]
        public  string PasswordHash { get; set; } = string.Empty;

        [Column("created_at")]
        public  DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}