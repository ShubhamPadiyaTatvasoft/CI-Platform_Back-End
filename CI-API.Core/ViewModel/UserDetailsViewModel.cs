using CI_API.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class UserDetailsViewModel
    {
        public long UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string ?EmployeeId { get; set; }
        public string? Manager { get; set; }
        public string? Title { get; set; }
        public string? Department { get; set; }
        public string? ProfileText { get; set; }
        public string? WhyVolunteer { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public string? Availability { get; set; }
        public string ?LinkedIn { get; set; }
        public string ?Skills { get; set; }
        public string? Avatar { get; set; }
    }
}
