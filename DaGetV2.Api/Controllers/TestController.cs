using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaGetV2.Dal.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DaGetV2.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("index")]
        public IActionResult Index([FromServices] IContextFactory contextFactory)
        {
            using (var cx = contextFactory.CreateContext())
            {
                var userRepo = cx.GetUserRepository();
                var user = userRepo.GetById(1);
                return Ok(user);
            }
        }
    }
}