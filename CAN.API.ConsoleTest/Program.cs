using CAN.API.Core;
using CAN.API.Core.Helpers;
using Iot.Device.Mcp25xxx;
using Iot.Device.Mcp25xxx.Register;
using System;
using System.Collections;
using System.Device.Spi;
using System.Linq;
using System.Threading.Tasks;

namespace CAN.API.ConsoleTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ReadBinary binaryData = new(@"Detector_flash_program.bin");
            binaryData.SenBinaryFile();

            

            CanRx receive = new();

            receive.SetReelLocation();


            // var DLC = receive.ReceiveDLC();
            //var msg = receive.ReadRxBuffer();

            //Console.WriteLine("DLC: ", DLC );
            //Console.WriteLine("Pirmas msg: {0}, Antras msg: {1}", msg[0], msg[1]);

        }
    }
}
