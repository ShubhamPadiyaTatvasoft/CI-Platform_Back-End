using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class MissionDataViewModel
    {
        public long missionId { get; set; }
        public string? title { get; set; }
        public string? shortDescription { get; set; }
        public string? description { get; set; }
        public string? organizationName { get; set; }
        public string? organizationDetails { get; set; }
        public long country { get; set; }
        public long city { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime deadlineDate { get; set; }
        public string? availability { get; set; }
        public long missionTheme { get; set; }
        public List<long>? missionSkills { get; set; }
        public string? missionType { get; set; }
        public long? totalSeats { get; set; }
        public string? goalText { get; set; }
        public string? status { get; set; }
        public long? goalValue { get; set; }
        public List<string>? missionImages { get; set; }
        public List<string>? missionDocuments { get; set; }
        public List<string>? missionDocumentsNameType { get; set; }
        public List<string>? missionImageNameType { get; set; }

    }
}
