using CI_API.Application.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingPageController : ControllerBase
    {

        #region Dependancy Injection
        private readonly ILandingPageService landingPageService;

        public LandingPageController(ILandingPageService _landingPageService)
        {
            landingPageService = _landingPageService;
        }
        #endregion

        #region LandingPage
        
        [HttpGet("LandingPage")]
        public async Task<JsonResult> LandingPage()
        {
            return landingPageService.LandingPage();
        }
        #endregion

    }
}
