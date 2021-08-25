using CAN.API.Core;
using CAN.API.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CAN.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedController : ControllerBase
    {
        [HttpGet("on")]
        public IActionResult TurnOnLed(int id, String color)
        {
            CanTx canTx = new(8);

            var calculateIDandSlotNr = IdCalculation(id);

            byte[] bytes = Helpers.GetBytesFromByteString(color).ToArray();
            byte brightness = 0xFF;
            int check = bytes.Length;

            if (check > 3) brightness = bytes[3];

            byte[] data = new byte[] { calculateIDandSlotNr.Item2, (byte)calculateIDandSlotNr.Item1, 0x00, 0xFF, bytes[0], bytes[1], bytes[2], brightness };
            canTx.TransmitMessage(data);
            return Ok();
        }

        [HttpGet("off")]
        public IActionResult TurnOffLed(int id)
        {
            CanTx canTx = new(8);

            var calculateIDandSlotNr = IdCalculation(id);
            byte[] data = new byte[] { calculateIDandSlotNr.Item2, (byte)calculateIDandSlotNr.Item1, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF };
            canTx.TransmitMessage(data);
            return Ok();
        }

        [HttpGet("on/all")]
        public IActionResult TurnOnAll(String color)
        {
            CanTx canTx = new(8);

            byte[] bytes = Helpers.GetBytesFromByteString(color).ToArray();
            byte brightness = 0xFF;
            int check = bytes.Length;

            if (check > 3) brightness = bytes[3];


            byte[] data = new byte[] { 0, 0, 0xFF, 0xFF, bytes[0], bytes[1], bytes[2], brightness };
            canTx.TransmitMessage(data);

            return Ok();
        }
        [HttpGet("off/all")]
        public IActionResult TurnOffAll()
        {
            CanTx canTx = new(8);
            byte[] data = new byte[] { 0, 0, 0x00, 0x00, 0x00, 0xFF, 0x00, 0xFF };
            canTx.TransmitMessage(data);

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
