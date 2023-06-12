using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Common.CommonModels
{
    public class responseStatusCode
    {
        #region Status codes
        public static int Success = 200;
        public static int NotFound = 404;
        public static int InvalidData = 401;
        public static int AlreadyExist = 409;
        public static int BadRequest = 500;
        #endregion
    }
}
