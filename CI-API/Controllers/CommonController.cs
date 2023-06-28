using CI_API.Application.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        ICommonService commonService;
        public CommonController(ICommonService _commonService)
        {
            commonService = _commonService;
        }

        [HttpGet("GetCountryList")]
        public Task<JsonResult> GetCountriesList()
        {
            return commonService.GetCountriesList();
        }

        [HttpGet("GetCitiesList")]
        public Task<JsonResult> GetCitiesList()
        {
            return commonService.GetCitiesList();
        }

        [HttpGet("GetCitiesListByCountryId")]
        public Task<JsonResult> GetCitiesList(long countryId)
        {
            return commonService.GetCitiesList(countryId);
        }

        [HttpGet("GetAvailability")]
        public Task<JsonResult> GetAvailability()
        {
            return commonService.GetAvailability();
        }

        [HttpGet("GetSkillsList")]
        public Task<JsonResult> GetSkillsList()
        {
            return commonService.GetSkillsList();
        }
    }
}
