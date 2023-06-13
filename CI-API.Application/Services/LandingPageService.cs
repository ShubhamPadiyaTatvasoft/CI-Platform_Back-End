using CI_API.Application.ServiceInterface;
using CI_API.Common.CommonModels;
using CI_API.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Application.Services
{
    public class LandingPageService:ILandingPageService
    {
        #region Dependency Injection of LandingPageRepository Interface

        private readonly ILandingPageRepository _LandingPageRepository;
        public LandingPageService(ILandingPageRepository LandingPageRepository)
        {
            _LandingPageRepository = LandingPageRepository;
        }
        #endregion

        #region LandingPage
        public JsonResult LandingPage()
        {
            return _LandingPageRepository.LandingPage();

            //return new JsonResult(new apiResponse<string> { Message = ResponseMessages.InternalServerError, StatusCode = responseStatusCode.BadRequest, Result = false });

        }
        #endregion
    }
}
