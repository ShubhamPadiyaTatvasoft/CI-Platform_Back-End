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
        public List<Mission> missions { get; set; }
        public List<MissionSkill> missionSkills { get; set; }
        public List<MissionTheme> missionThemes { get; set; }
    }
}
