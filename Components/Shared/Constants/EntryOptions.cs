using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhawanaPatra.Components.Shared.Constants
{
    public class EntryOptions
    {
        
        public static readonly List<string> PositiveMoods = new()
        {
            "Happy", "Excited", "Relaxed", "Grateful", "Confident"
        };

        public static readonly List<string> NeutralMoods = new()
        {
            "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored"
        };

        public static readonly List<string> NegativeMoods = new()
        {
            "Sad", "Angry", "Stressed", "Lonely", "Anxious"
        };

        
        public static readonly List<string> AllMoods = new List<string>()
            .Concat(PositiveMoods)
            .Concat(NeutralMoods)
            .Concat(NegativeMoods)
            .ToList();

        
        public static readonly List<string> PreDefinedTags = new()
        {
            "Work", "Career", "Studies", "Family", "Friends", "Relationships",
            "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies",
            "Travel", "Nature", "Finance", "Spirituality", "Birthday", "Holiday",
            "Vacation", "Celebration", "Exercise", "Reading", "Writing", "Cooking",
            "Meditation", "Yoga", "Music", "Shopping", "Parenting", "Projects",
            "Planning", "Reflection"
        };
    }
}
