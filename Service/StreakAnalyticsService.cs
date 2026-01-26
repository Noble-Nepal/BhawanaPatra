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
        public int GetCurrentStreak(int userId)
        {
            int streak = 0;
            var day = DateTime.Today;

            while (true)
            {
                var key = day.ToString("yyyy-MM-dd");
                if (_db.GetEntryByDate(userId, key) == null)
                    break;

                streak++;
                day = day.AddDays(-1);
            }

            return streak;
        }

        public int GetLongestStreak(int userId)
        {
            var entries = _db.GetEntriesByUser(userId)
                             .Select(e => e.EntryDateKey)
                             .ToHashSet();

            int best = 0, current = 0;

            for (int i = 0; i < 365; i++)
            {
                var key = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd");
                if (entries.Contains(key))
                {
                    current++;
                    best = Math.Max(best, current);
                }
                else current = 0;
            }

            return best;
        }
    }
}
