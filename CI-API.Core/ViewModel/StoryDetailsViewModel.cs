using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class StoryDetailsViewModel
    {
        public long StoryId { get; set; }
        public string? StoryTitle { get; set; }
        public string? VolunteerStoryDescription { get; set; }
        public string Avatar { get; set; }
        public string VolunteerName { get; set; }
        public string? WhyIVolunteer { get; set; }
        public long? StoryView { get; set; }
        public long MissionId { get; set; }
        public string StoryImage { get; set; }
        public string ToEmail { get; set; }
        //public List<StoryMedium> img { get; set; }
        public List<string> StoryImages { get; set; }
    }
}
