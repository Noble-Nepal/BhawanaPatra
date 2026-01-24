using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
   
      public class AppThemeService
    {
        public bool IsDarkMode { get; private set; }

        public void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }
    }
    
}
