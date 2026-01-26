using BhawanaPatra.Database;
using BhawanaPatra.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BhawanaPatra.Service
{
    public class EntryService
    {
        private readonly DatabaseConfiguration _db;

        public EntryService(DatabaseConfiguration db)
        {
            _db = db;
        }

        public static string TodayKey() => DateTime.Now.ToString("yyyy-MM-dd");

        public static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            return text.Split(new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public EntryModel? GetTodayEntry(int userId)
        {
            return _db.GetEntryByDate(userId, TodayKey());
        }

        public EntryModel? GetEntryByDate(int userId, string dateKey)
        {
            return _db.GetEntryByDate(userId, dateKey);
        }

        public void SaveEntry(EntryModel entry)
        {
            var existing = _db.GetEntryByDate(entry.UserId, entry.EntryDateKey);

            if (existing == null)
            {
                _db.InsertEntry(entry);
            }
            else
            {
                entry.EntryId = existing.EntryId;
                entry.CreatedAt = existing.CreatedAt;
                _db.UpdateEntry(entry);
            }
        }

        public void DeleteTodayEntry(int userId)
        {
            var entry = _db.GetEntryByDate(userId, TodayKey());
            if (entry != null)
                _db.DeleteEntry(entry);
        }

        public void DeleteEntry(int entryId)
        {
            var entry = _db.GetEntryById(entryId);
            if (entry != null)
                _db.DeleteEntry(entry);
        }

        public List<string> GetSecondaryMoodsList(EntryModel entry)
        {
            if (string.IsNullOrWhiteSpace(entry.SecondaryMoods))
                return new List<string>();

            return entry.SecondaryMoods.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(m => m.Trim())
                                        .ToList();
        }

        public List<EntryModel> GetRecentEntries(int userId, int take = 5)
            => _db.GetEntriesByUser(userId).Take(take).ToList();

        public int GetTotalEntries(int userId)
            => _db.GetEntriesByUser(userId).Count;

        public int GetWordsToday(int userId)
            => GetTodayEntry(userId)?.WordCount ?? 0;

        public List<EntryModel> GetAllEntries(int userId)
        {
            return _db.GetEntriesByUser(userId);
        }

        public List<EntryModel> SearchEntries(
            int userId,
            string? searchText = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? mood = null,
            string? tag = null)
        {
            var entries = _db.GetEntriesByUser(userId);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var search = searchText.ToLower();
                entries = entries.Where(e =>
                    (e.Title?.ToLower().Contains(search) ?? false) ||
                    (e.Content?.ToLower().Contains(search) ?? false)
                ).ToList();
            }

            if (startDate.HasValue || endDate.HasValue)
            {
                entries = entries.Where(e =>
                {
                    if (!DateTime.TryParse(e.EntryDateKey, out var date)) return false;
                    if (startDate.HasValue && date < startDate.Value) return false;
                    if (endDate.HasValue && date > endDate.Value) return false;
                    return true;
                }).ToList();
            }

            if (!string.IsNullOrWhiteSpace(mood))
            {
                entries = entries.Where(e =>
                    e.PrimaryMood?.Equals(mood, StringComparison.OrdinalIgnoreCase) ?? false ||
                    (e.SecondaryMoods?.Contains(mood, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                entries = entries.Where(e =>
                    e.Tags?.Contains(tag, StringComparison.OrdinalIgnoreCase) ?? false
                ).ToList();
            }

            return entries;
        }

        public List<string> GetAllMoods(int userId)
        {
            var entries = _db.GetEntriesByUser(userId);
            var moods = new HashSet<string>();

            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.PrimaryMood))
                    moods.Add(entry.PrimaryMood);

                if (!string.IsNullOrEmpty(entry.SecondaryMoods))
                {
                    foreach (var m in entry.SecondaryMoods.Split(','))
                        moods.Add(m.Trim());
                }
            }

            return moods.OrderBy(m => m).ToList();
        }

        public List<string> GetAllTags(int userId)
        {
            var entries = _db.GetEntriesByUser(userId);
            var tags = new HashSet<string>();

            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Tags))
                {
                    foreach (var t in entry.Tags.Split(','))
                        tags.Add(t.Trim());
                }
            }

            return tags.OrderBy(t => t).ToList();
        }
    }
}