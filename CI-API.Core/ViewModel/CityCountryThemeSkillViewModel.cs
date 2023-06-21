using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class CityCountryThemeSkillViewModel
    {
        public List<City> city { get; set; }
        public List<Country> country { get; set; }
        public List<MissionTheme> themes { get; set; }
        public List<Skill> skills { get; set; }
    }
}
