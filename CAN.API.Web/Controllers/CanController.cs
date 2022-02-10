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
        public IActionResult SetReelLocation()
        {
          //  CanTx transmitMsg = new(4);
            byte[] data = new byte[] { 0x00, 0x00, 0x0F, 0xF0 };
            //transmitMsg.TransmitMessage(data);
            CanRx receivedMsg = new();
            var msg = receivedMsg.ReadRxBuffer();
            return Ok(msg);
        }
        [HttpGet("takeout")]
        public IActionResult TakeOut(int id)
        {
           // CanTx canTx = new(8);

            var calculatedIdAndSlotNr = IdCalculation(id);
            string color = "00897b";
            byte[] bytes = Helpers.GetBytesFromByteString(color).ToArray();
            byte brightness = 0xFF;
            int check = bytes.Length;

            if (check > 3) brightness = bytes[3];

            byte[] data = new byte[] { calculatedIdAndSlotNr.Item2, (byte)calculatedIdAndSlotNr.Item1, 0xF0, 0x0F, bytes[0], bytes[1], bytes[2], brightness };
           // canTx.TransmitMessage(data);
            return Ok();
        }
        private Tuple<int, byte> IdCalculation(int id)
        {
            int tarpinis = (id - 1) / 10;
            int slotNr = id - (tarpinis * 10);
            byte ID = Convert.ToByte(tarpinis);

            return Tuple.Create(slotNr, ID);
        }
    }
}
