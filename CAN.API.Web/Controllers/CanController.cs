using CAN.API.Core;
using CAN.API.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;

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
        [HttpGet("setlocation")]
        public IActionResult SetReelLocation()
        {
            CanTx transmitMsg = new(4);
            byte[] data = new byte[] { 0x00, 0x00, 0x0F, 0xF0 };
            transmitMsg.TransmitMessage(data);
            CanRx receivedMsg = new();
            var msg = receivedMsg.ReadRxBuffer();
            return Ok(msg);
        }
        [HttpGet("takeout")]
        public IActionResult TakeOut(int id)
        {
            int tarpinis = (id / 10) + 1;
            int slotNr = id - ((tarpinis - 1) * 10);
            byte ID = Convert.ToByte(tarpinis);

            CanTx transmitMsg = new(8);
            byte[] data = new byte[] { ID, (byte)slotNr, 0xF0, 0x0F, 0x00, 0x00, 0xFF, 0xFF };
            transmitMsg.TransmitMessage(data);
            return Ok();
        }

    }
}
