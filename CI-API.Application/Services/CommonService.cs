using CI_API.Application.ServiceInterface;
using CI_API.Data.Interface;
using CI_API.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.Services
{
    public class CommonService:ICommonService
    {
        private readonly ICommonRepository commonRepository;

        public CommonService(ICommonRepository _commonRepository)
        {
            commonRepository = _commonRepository;
        }
        public async Task<JsonResult> GetCountriesList()
        {
            return await commonRepository.GetCountriesList();
        }

        public async Task<JsonResult> GetCitiesList()
        {
            return await commonRepository.GetCitiesList();
        }

        public async Task<JsonResult> GetCitiesList(long countryId)
        {
            return await commonRepository.GetCitiesList(countryId);
        }

        public async Task<JsonResult> GetAvailability()
        {
            return await commonRepository.GetAvailability();
        }

        public async Task<JsonResult> GetSkillsList()
        {
            return await commonRepository.GetSkillsList();
        }
    }
}
