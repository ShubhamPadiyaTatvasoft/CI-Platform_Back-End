using CI_API.Common.CommonModels;
using CI_API.Core.CIDbContext;
using CI_API.Core.Models;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class CommonRepository:ICommonRepository
    {
        private readonly CiPlatformDbContext cIDbContext;
        public CommonRepository(CiPlatformDbContext _cIDbContext)
        {
            cIDbContext = _cIDbContext;
        }

        public async Task<JsonResult> GetCountriesList()
        {
            List<Country> countryList = await Task.FromResult(cIDbContext.Countries.ToList());
            return new JsonResult(new apiResponse<List<Country>> { Data = countryList, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }

        public async Task<JsonResult> GetCitiesList()
        {
            List<City> cityList = await Task.FromResult(cIDbContext.Cities.ToList());
            return new JsonResult(new apiResponse<List<City>> { Data = cityList, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }
        public async Task<JsonResult> GetCitiesList(long countryId)
        {
            List<City> cityList = await Task.FromResult(cIDbContext.Cities.Where(city => city.CountryId == countryId).ToList());
            return new JsonResult(new apiResponse<List<City>> { Data = cityList, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }

        public async Task<JsonResult> GetAvailability()
        {
            List<string> availability = new List<string> { };
            availability.Add("Daily");
            availability.Add("Weekly");
            availability.Add("Monthly");
            availability.Add("Yearly");
            return new JsonResult(new apiResponse<List<string>> { Data = availability, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }

        public async Task<JsonResult> GetSkillsList()
        {
            List<Skill> skillList = await Task.FromResult(cIDbContext.Skills.ToList());
            return new JsonResult(new apiResponse<List<Skill>> { Data = skillList, Message = ResponseMessages.Success, StatusCode = responseStatusCode.Success, Result = true });
        }
    }
}
