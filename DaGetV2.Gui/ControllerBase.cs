using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DaGetV2.Gui
{
    public abstract class ControllerBase : Controller
    {
        protected readonly AppConfiguration _appConfiguration;

        public ControllerBase(IConfiguration configuration) 
        {
            _appConfiguration = configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
        }
    }
}
