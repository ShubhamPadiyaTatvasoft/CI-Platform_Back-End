using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class MissionCardViewModel
    {
        public Mission Mission { get; set; } = new Mission();

        public string CityName { get; set; } = string.Empty;

        public string ThemeName { get; set; } = string.Empty;

        public long? Seatleft { get; set; }

        public long? AlreadyVolunteer { get; set; }

        public bool? IsfavMission { get; set; }

        public float? AvgRating { get; set; }

        public bool? IsAppliedMission { get; set; }

        public bool? IsApproveMission { get; set; }

        public string MissionImagePath { get; set; } = string.Empty;

        public int? TargetGoalValue { get; set; }

        public int? AchieveGoalValue { get; set; }
    }
}
