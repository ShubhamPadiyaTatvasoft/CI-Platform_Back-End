using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Core.ViewModel
{
    public class RegisterViewModel
    {
        public string firstName { get; set; }    
        public string lastName { get; set; }    
        public long? phoneNumber { get; set; }    
        public string? email { get; set; }
        public string? password { get; set; }
        public string? confirmPassword { get; set; }
    }
}
