using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class BannerDataViewModel
    {
        public string? bannerText { get; set; }
        public long? bannerId { get; set; }
        public long? sortOrder { get; set; }
        public List<string>? images { get; set; }
    }
}
