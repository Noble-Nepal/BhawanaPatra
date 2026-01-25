using SQLite;

namespace BhawanaPatra.Models
{
    [Table("entries")]
    public class EntryModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("entry_id")]
        public int EntryId { get; set; }

        [Indexed]
        [Column("user_id")]
        public int UserId { get; set; }

        [Indexed]
        [Column("entry_date_key")]
        public string EntryDateKey { get; set; } = string.Empty;

        [Column("title")]
        public string? Title { get; set; }

        [NotNull]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("word_count")]
        public int WordCount { get; set; }

        [Column("primary_mood")]
        public string? PrimaryMood { get; set; }

        [Column("secondary_moods")]
        public string? SecondaryMoods { get; set; }
        [Column("tags")]
        public string? Tags { get; set; }  

        [Column("mood_category")]
        public string? MoodCategory { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}
