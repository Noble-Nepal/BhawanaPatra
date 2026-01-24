using BhawanaPatra.Database;
using BhawanaPatra.Models;

namespace BhawanaPatra.Service
{
    public class EntryService
    {
        private readonly DatabaseConfiguration _db;

        public EntryService(DatabaseConfiguration db)
        {
            _db = db;
        }

        private static string TodayKey() => DateTime.Now.ToString("yyyy-MM-dd");

        private static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            return text.Split(new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public EntryModel? GetTodayEntry(int userId)
        {
            return _db.GetEntryByDate(userId, TodayKey());
        }


        public EntryModel SaveTodayEntry(int userId, string? title, string content)
        {
            var key = TodayKey();
            var existing = _db.GetEntryByDate(userId, key);
            var now = DateTime.Now;

            if (existing == null)
            {
                var entry = new EntryModel
                {
                    UserId = userId,
                    EntryDateKey = key,
                    Title = title,
                    Content = content,
                    WordCount = CountWords(content),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _db.InsertEntry(entry);
                return entry;
            }

            existing.Title = title;
            existing.Content = content;
            existing.WordCount = CountWords(content);
            existing.UpdatedAt = now;

            _db.UpdateEntry(existing);
            return existing;
        }

        public void DeleteTodayEntry(int userId)
        {
            var entry = _db.GetEntryByDate(userId, TodayKey());
            if (entry != null)
                _db.DeleteEntry(entry);
        }
        public List<EntryModel> GetRecentEntries(int userId, int take = 5)
        => _db.GetEntriesByUser(userId).Take(take).ToList();

        public int GetTotalEntries(int userId)
            => _db.GetEntriesByUser(userId).Count;

        public int GetWordsToday(int userId)
            => GetTodayEntry(userId)?.WordCount ?? 0;

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
        public List<EntryModel> GetAllEntries(int userId)
        {
            return _db.GetEntriesByUser(userId);
        }


    }
}