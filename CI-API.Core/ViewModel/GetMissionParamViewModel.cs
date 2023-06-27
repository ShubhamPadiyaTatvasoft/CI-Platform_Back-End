﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class GetMissionParamViewModel
    {
        public List<long> CountryIds { get; set; } = new List<long>();

        public List<long> CityIds { get; set; } = new List<long>();

        public List<long> ThemeIds { get; set; } = new List<long>();

        public List<long> SkillIds { get; set; } = new List<long>();

        public string Search { get; set; } = string.Empty;

        public string ShortBy { get; set; } = string.Empty;

        public long LoginUserId { get; set; }
    }
}
