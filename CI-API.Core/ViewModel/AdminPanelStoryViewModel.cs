using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class AdminPanelStoryViewModel
    {
        public List<Story>? Stories { get; set; }
        public List<Mission>? Missions { get; set; }
        public List<User>? Users { get; set; }
        public long? storyId { get; set; }
        public string? storyStatus { get; set; }
    }
}
