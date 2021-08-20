using CAN.API.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAN.API.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CanController : ControllerBase
    {
        private readonly ILogger<CanController> _logger;

        public CanController(ILogger<CanController> logger)
        {
            _logger = logger;
        }

        [HttpPost("test")]
        public async Task<IActionResult> CanMsg()
        {
            CanInit mcp2515 = new();
            byte[] data = new byte[] { 0xFF, 0xFF, 0xF0, 0x0F, 0x00, 0x00, 0xFF, 0xFF };
            mcp2515.TransmitMessage(data);

            return Ok();
        }
    }
}
