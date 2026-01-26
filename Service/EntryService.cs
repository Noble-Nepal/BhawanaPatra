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

        
        public EntryModel SaveTodayEntry(
           int userId,
           string? title,
           string content,
           string? primaryMood = null,
           string? moodCategory = null,
           List<string>? secondaryMoods = null,
           string? tags = null)
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
                    PrimaryMood = primaryMood,
                    MoodCategory = moodCategory,
                    SecondaryMoods = secondaryMoods != null && secondaryMoods.Any()
                        ? string.Join(",", secondaryMoods)
                        : null,
                    Tags = tags,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _db.InsertEntry(entry);
                return entry;
            }
            existing.Title = title;
            existing.Content = content;
            existing.WordCount = CountWords(content);
            existing.PrimaryMood = primaryMood;
            existing.MoodCategory = moodCategory;
            existing.SecondaryMoods = secondaryMoods != null && secondaryMoods.Any()
                ? string.Join(",", secondaryMoods)
                : null;
            existing.Tags = tags;
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
    }
}