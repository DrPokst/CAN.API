using CAN.API.Core;
using CAN.API.Web.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CAN.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanController : ControllerBase
    {

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
