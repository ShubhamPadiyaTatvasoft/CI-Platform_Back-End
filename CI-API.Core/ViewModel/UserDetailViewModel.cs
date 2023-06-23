using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class UserDetailViewModel
    {
        public long userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public long countryId { get; set; }
        public long cityId { get; set; }
        public long phoneNumber { get; set; }
        public string role { get; set; }
        public bool? status { get; set; }
        public string manager { get; set; }

    }
}
