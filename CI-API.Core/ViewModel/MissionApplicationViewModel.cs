using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class MissionApplicationViewModel
    {
        public List<User>? Users { get; set; }
        public List<MissionApplication>? Applications { get; set; }
        public List<Mission>? Missions { get; set; }

        public long? missionApplicationId{ get; set; }
        public string? approvalStatus{ get; set; }
    }
}
