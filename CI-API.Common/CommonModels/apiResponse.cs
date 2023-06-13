using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Common.CommonModels
{
    public class apiResponse<T>
    {
        public T? Data { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
