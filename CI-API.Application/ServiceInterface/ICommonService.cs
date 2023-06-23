using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.ServiceInterface
{
    public interface ICommonService
    {
        public Task<JsonResult> GetCountriesList();
        public Task<JsonResult> GetCitiesList();
        public Task<JsonResult> GetCitiesList(long countryId);
        public Task<JsonResult> GetAvailability();
        public Task<JsonResult> GetSkillsList();
    }
}
