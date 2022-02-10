using CAN.API.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CAN.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedController : ControllerBase
    {
        private readonly ICanBus _can;
        public LedController(ICanBus can)
        {
            _can = can;
        }

        [HttpGet("on")]
        public IActionResult TurnOnLed(int location, string color)
        {
            _can.Turn_On_Single_Led(location, color, 0xFF);
            return Ok();
        }

        [HttpGet("off")]
        public IActionResult TurnOffLed(int location)
        {
            _can.Turn_Off_Single_Led(location);
            return Ok();
        }

        [HttpGet("on/all")]
        public IActionResult TurnOnAll(string color)
        {
            Console.WriteLine("!!!!!!!!!!!!komanda gauta!!!!!!!!!!!!!!!!!!!");
            _can.Turn_On_All_Led(color, 0xFF);
            return Ok();
        }

        [HttpGet("off/all")]
        public IActionResult TurnOffAll()
        {
            _can.Turn_Off_All_Led();
            return Ok();
        }
    }
}
