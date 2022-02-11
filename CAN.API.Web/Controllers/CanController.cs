using CAN.API.Core;
using CAN.API.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CAN.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanController : ControllerBase
    {
        private readonly ICanBus _can;
        public CanController(ICanBus can)
        {
            _can = can;
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
           // CanTx transmitMsg = new(canDto.MsgLength);
           // transmitMsg.TransmitMessage(canDto.Msg);
            return Ok();
        }

        [HttpGet("setlocation")]
        public IActionResult SetReelLocation(int id)
        {
            _can.Start_Reels_Registration(id);
            CanRx receivedMsg = new();
            var msg = receivedMsg.ReadRxBuffer();
            CanMessageDTO msgForReturn = new CanMessageDTO
            {
                CanId = msg.CanId,  //nenaudojama
                CMD = msg.CMD,
                Data = msg.Data.ToList()
            };

            return Ok(msgForReturn);
        }

        [HttpGet("takeout")]
        public IActionResult TakeOut(int location, int reelID, string color)
        {
            _can.Start_Reels_Taking(location, reelID, color, 0xFF);
            return Ok();
        }
    }
}
