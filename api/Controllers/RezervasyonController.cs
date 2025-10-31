using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RezervasyonController : ControllerBase
    {
        private readonly IRezervasyonService _rezervasyonService;

        public RezervasyonController(IRezervasyonService rezervasyonService)
        {
            _rezervasyonService = rezervasyonService;
        }

        [HttpPost]
        public ActionResult<RezervasyonRes> RezervasyonYap([FromBody] RezervasyonReq request)
        {
            if (request == null || request.Tren == null)
            {
                return BadRequest("Geçersiz istek");
            }

            var response = _rezervasyonService.RezervasyonYap(request);
            return Ok(response);
        }
    }
}