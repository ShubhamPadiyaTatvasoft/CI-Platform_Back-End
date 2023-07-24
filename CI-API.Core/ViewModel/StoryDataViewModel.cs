using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class StoryDataViewModel
    {
        [Key]
        public long? StoryId { get; set; }
        public string? Title { get; set; }
        public string StoryImage { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? UserName { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Theme { get; set; }
    }
}
