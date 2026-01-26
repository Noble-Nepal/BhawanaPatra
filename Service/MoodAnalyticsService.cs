using BhawanaPatra.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Service
{
    public class MoodAnalyticsService
    {
        private readonly DatabaseConfiguration _db;

        public MoodAnalyticsService(DatabaseConfiguration db)
        {
            _db = db;
        }

        public Dictionary<string, int> GetMoodCategoryStatistics(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var entries = _db.GetEntriesByUser(userId);

            if (startDate.HasValue || endDate.HasValue)
            {
                entries = entries.Where(e =>
                {
                    if (!DateTime.TryParse(e.EntryDateKey, out var entryDate)) return false;
                    if (startDate.HasValue && entryDate < startDate.Value) return false;
                    if (endDate.HasValue && entryDate > endDate.Value) return false;
                    return true;
                }).ToList();
            }

            var stats = new Dictionary<string, int>
            {
                { "Positive", 0 },
                { "Neutral", 0 },
                { "Negative", 0 }
            };

            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.MoodCategory) && stats.ContainsKey(entry.MoodCategory))
                {
                    stats[entry.MoodCategory]++;
                }
            }

            return stats;
        }

        public Dictionary<string, double> GetMoodCategoryPercentages(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var stats = GetMoodCategoryStatistics(userId, startDate, endDate);
            var total = stats.Values.Sum();

            if (total == 0)
                return new Dictionary<string, double>
                {
                    { "Positive", 0 },
                    { "Neutral", 0 },
                    { "Negative", 0 }
                };

            return stats.ToDictionary(
                kvp => kvp.Key,
                kvp => Math.Round((kvp.Value / (double)total) * 100, 1)
            );
        }

        public Dictionary<string, int> GetMoodFrequency(int userId, int topN = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            var entries = _db.GetEntriesByUser(userId);

            if (startDate.HasValue || endDate.HasValue)
            {
                entries = entries.Where(e =>
                {
                    if (!DateTime.TryParse(e.EntryDateKey, out var entryDate)) return false;
                    if (startDate.HasValue && entryDate < startDate.Value) return false;
                    if (endDate.HasValue && entryDate > endDate.Value) return false;
                    return true;
                }).ToList();
            }

            var moodCounts = new Dictionary<string, int>();

            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.PrimaryMood))
                {
                    if (moodCounts.ContainsKey(entry.PrimaryMood))
                        moodCounts[entry.PrimaryMood]++;
                    else
                        moodCounts[entry.PrimaryMood] = 1;
                }

                if (!string.IsNullOrEmpty(entry.SecondaryMoods))
                {
                    var secondaryMoods = entry.SecondaryMoods.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(m => m.Trim());
                    foreach (var mood in secondaryMoods)
                    {
                        if (moodCounts.ContainsKey(mood))
                            moodCounts[mood]++;
                        else
                            moodCounts[mood] = 1;
                    }
                }
            }

            return moodCounts.OrderByDescending(kvp => kvp.Value)
                            .Take(topN)
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public string GetMostFrequentMood(int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var moods = GetMoodFrequency(userId, 1, startDate, endDate);
            return moods.FirstOrDefault().Key ?? "N/A";
        }

        public Dictionary<string, int> GetTagFrequency(int userId, int topN = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            var entries = _db.GetEntriesByUser(userId);

            if (startDate.HasValue || endDate.HasValue)
            {
                entries = entries.Where(e =>
                {
                    if (!DateTime.TryParse(e.EntryDateKey, out var entryDate)) return false;
                    if (startDate.HasValue && entryDate < startDate.Value) return false;
                    if (endDate.HasValue && entryDate > endDate.Value) return false;
                    return true;
                }).ToList();
            }

            var tagCounts = new Dictionary<string, int>();

            foreach (var entry in entries)
            {
                if (string.IsNullOrEmpty(entry.Tags)) continue;

                var tags = entry.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(t => t.Trim());

                foreach (var tag in tags)
                {
                    if (tagCounts.ContainsKey(tag))
                        tagCounts[tag]++;
                    else
                        tagCounts[tag] = 1;
                }
            }

            return tagCounts.OrderByDescending(kvp => kvp.Value)
                           .Take(topN)
                           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}