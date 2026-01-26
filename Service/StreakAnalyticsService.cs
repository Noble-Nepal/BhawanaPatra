using BhawanaPatra.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
    public class StreakAnalyticsService
    {
        private readonly DatabaseConfiguration _db;

        public StreakAnalyticsService(DatabaseConfiguration db)
        {
            _db = db;
        }
    }
}
