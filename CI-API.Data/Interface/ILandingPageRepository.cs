using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Interface
{
    public interface ILandingPageRepository
    {
        #region Methods of LandingPageRepository
        public JsonResult LandingPage();
        #endregion
    }
}
