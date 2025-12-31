using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FeedBack.Models
{
    public class FB
    {
        [MaxLength(100)]
        public string ?Name { get; set; }
        
        [MaxLength(150)]
        public string ?Email { get; set; }
        
        [MaxLength(500)]
        public string ?Fb { get; set; }
 
        [NotNull]
        public string ?Emojivalue { get; set; }

    }
}