using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.ViewModel
{
    public class RegisterModel
    {
     

        public  string Username { get; set; } = string.Empty;


        public  string Password { get; set; } = string.Empty;


        public  string Password2 { get; set; } = string.Empty;
    }
}
