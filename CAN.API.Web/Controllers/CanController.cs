using CAN.API.Core;
using CAN.API.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet("msg")]
        public IActionResult ReceiveCanMsg()
        {
            CanRx receivedMsg = new();
            var msg = receivedMsg.ReadRxBuffer();
            return Ok(msg);
        }

        [HttpPost("msg")]
        public IActionResult TransmitCanMsg(CanDto canDto)
        {
            CanTx transmitMsg = new(canDto.MsgLength);
            transmitMsg.TransmitMessage(canDto.Msg);
            return Ok();
        }

    }
}
