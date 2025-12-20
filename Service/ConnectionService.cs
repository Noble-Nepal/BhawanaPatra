using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
    internal class ConnectionService
    {
        public const string DatabaseFilename = "bhawanapatra.db3";
        public static string DatabasePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         DatabaseFilename);
    }
}
