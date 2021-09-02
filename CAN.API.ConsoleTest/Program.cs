using CAN.API.Core;
using System;

namespace CAN.API.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ReadBinary binaryData = new(@"../../../Detector_flash_program.bin");
            CanTx transmit = new(8);
            byte[] data = new byte[] { };







            Console.WriteLine("Pasirinkit norimus veiksmus: ");
            Console.WriteLine("1 - FLASH_START_ERASE     0xB0");
            Console.WriteLine("2 - FLASH_START_WRITE     0xB1");
            Console.WriteLine("3 - FLASH_ERASE_ERROR     0xB2");
            Console.WriteLine("4 - FLASH_WRITE_ERROR     0xB3");
            Console.WriteLine("5 - JUMP_TO_USER_APP      0xB4");
            Console.WriteLine("6 - READ_CAN              0xB5");

            var veiksmas = Console.ReadLine();

            switch (veiksmas)
            {
                case "1":
                    data = new byte[] { 0x00, 0xB0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    transmit.TransmitMessage(data);
                    break;
                case "2":
                    data = new byte[] { 0x00, 0xB1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    transmit.TransmitMessage(data);
                    binaryData.ZygioAlgortimas();
                    break;
                case "3":
                    data = new byte[] { 0x00, 0xB2, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    transmit.TransmitMessage(data);
                    break;
                case "4":
                    data = new byte[] { 0x00, 0xB3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    break;
                case "5":
                    data = new byte[] { 0x00, 0xB4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    transmit.TransmitMessage(data);
                    break;
                case "6":
                    CanRx receivedMsg = new();
                    while (true)
                    {
                        var msg = receivedMsg.ReadRxBuffer();
                        if (msg is not null)
                        {
                            Console.WriteLine(msg[0]);
                            Console.WriteLine("CAN msg {0}: {1}, {2}", 1, BitConverter.ToString(msg, 0, 4), BitConverter.ToString(msg, 4, 4));
                        }

                    }
                    break;

                default:
                    Console.WriteLine("Default");
                    break;
            }

        }
    }
}
