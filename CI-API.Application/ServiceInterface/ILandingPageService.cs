using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface ILandingPageService
    {
        #region Method of LandingPageService
        public JsonResult LandingPage();
        #endregion
    }
}
