using CI_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class VolunteerMissionViewModel
    {
        public long missionId { get; set; }
        public string title { get; set; }
        public string? shortDescription { get; set; }
        public string? description { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string countryName { get; set; }
        public string cityName { get; set; }
        public string themeName { get; set; }
        public string missionType { get; set; } 
        public string? organizationName { get; set; }
        public string? organizationDetail { get; set; }
        public long? leftSeats { get; set; }
        public DateTime? deadline { get; set; }
        public long missionApplicationId { get; set; }
        public string? documentPath { get; set; }
        public string? documentName { get; set; }
        public string? documentType { get; set; }
        public string? mediaPath { get; set; }
        public int? ratings { get; set; }
        public string skillName { get; set; }
        public string goalObjectiveText { get; set; } 
        public string goalValue { get; set; }
        public long applymissionuser { get; set; }
        public double avgRating { get; set; }
        public DateTime createdAtcomment { get; set; }
        public string? avatar { get; set; }
        public long? userId { get; set; }
        public string? commentText { get; set; }
        public string? approvalStatus { get; set; }
        public bool? IsFavMission { get; set; }
        public List<DocDetail> docsPath { get; set; }  
        public string? imgPath { get; set; }        
        public List<Comment> comments { get; set; }
    }

    public class DocDetail
    {
       
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
       
    }
}
