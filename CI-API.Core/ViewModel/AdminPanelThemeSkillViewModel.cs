using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class AdminPanelThemeSkillViewModel
    {
        public long? themeId { get; set; }
        public string? themeTitle { get; set; }
        public string? themeStatus { get; set; }
        public long? skillId { get; set; }
        public string? skillName { get; set; }
        public string? skillStatus { get; set; }
    }
}
