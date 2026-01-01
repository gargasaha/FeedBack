using System.ComponentModel.DataAnnotations;

namespace FeedBack{
    public class CountGroup
    {
        [StringLength(100)]
        public string ?EmojiValue {get;set;}
        public int ?Total {get;set;}
    }
}