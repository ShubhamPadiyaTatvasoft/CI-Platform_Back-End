using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class LandingPageViewModel
    {
        public Mission? mission { get; set; }
        public List<MissionSkill>? missionSkills { get; set; }
        public List<MissionDocument>? missionDocuments{ get; set; }
        public List<MissionTheme>? missionThemes { get; set; }
        public List<Mission>? missions { get; set; }
        public List<MissionMedium>? missionMedia { get; set; }
        public GoalMission? goalMissions{ get; set; }
    }
}
