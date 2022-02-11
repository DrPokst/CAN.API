using CAN.API.Core;
using Force.Crc32;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CAN.API.ConsoleTest
{
    class Program
    {
        public static string color { get; set; } = "66ff33";
        public static int brightness { get; set; } = 0x55;
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<ICanBus, CanBus>()
            .BuildServiceProvider();

            //configure console logging
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            

            logger.LogDebug("Starting application");
            var can = serviceProvider.GetService<ICanBus>();


            byte[] programForUpload = File.ReadAllBytes(@"project.bin");
            var CRC = Crc32Algorithm.Compute(programForUpload);
            var ilgis = programForUpload.Length;
            

            while (true)
            {
                Console.WriteLine("Pasirinkit norimus veiksmus: ");
                Console.WriteLine("1  - FLASH_START_WRITE         0x01");
                Console.WriteLine("2  - JUMP_TO_USER_APP          0x02");
                Console.WriteLine("3  - JUMP_TO_BOOTLOADER        0x03");
                Console.WriteLine("4  - RESET_SINGLE              0x04");
                Console.WriteLine("5  - RESET_ALL                 0x05");
                Console.WriteLine("6  - SET_CAN_BUS_SPEED         0x06");
                Console.WriteLine("7  - TURN_ON_SINGLE_LED        0x07");
                Console.WriteLine("8  - TURN_OFF_SINGLE_LED       0x08");
                Console.WriteLine("9  - TURN_ON_ALL_LED           0x09");
                Console.WriteLine("10 - TURN_OFF_ALL_LED          0x10");
                Console.WriteLine("11 - START_REELS_REGISTRATION  0x11");
                Console.WriteLine("12 - STOP_REELS_REGISTRATION   0x12");
                Console.WriteLine("13 - START_REELS_TAKING        0x13");
                Console.WriteLine("20 - CLEAR_EEPROM_MEMORY       0x20");
                Console.WriteLine("27 - BOOTLOADER_FLAG_SET       0x27");
                Console.WriteLine("28 - BOOTLOADER_FLAG_RESET     0x28");
                Console.WriteLine("29 - START_NODE_ID_NUMERATION  0x29");
                Console.WriteLine("ZZ - FLASH_ALL                 0xZZ");
                Console.WriteLine("XX - READ_CAN                  0xFF");

                var veiksmas = Console.ReadLine();


                Console.Write("Iveskite Node ID: ");
                var CanId = Int32.Parse(Console.ReadLine());
                

                switch (veiksmas)
                {
                    case "1":  //FLASH_START_WRITE
                        can.Flash_Start_Write(CanId, programForUpload.Length, CRC);

                        CanRx receive = new();
                        CanMessage msg = receive.ReadRxBuffer();

                        Console.WriteLine("CMD: {0}", msg.CMD);
                        Console.WriteLine("CAN ID: {0}", msg.CanId);
                        Console.WriteLine("CAN msg {0}: {1}", 1, BitConverter.ToString(msg.Data, 0, 2));

                        CanTx tx = new();


                        if (msg.Data[0] == 0x79 && msg.CanId == CanId && msg.CMD == 0x01)
                        {

                            Console.WriteLine("Flashing");
                            for (int i = 0; i < programForUpload.Length / 8 + 1; i++)
                            {

                                msg = receive.ReadRxBuffer();


                                Console.WriteLine("CMD: {0}", msg.CMD);
                                Console.WriteLine("CAN ID: {0}", msg.CanId);
                                Console.WriteLine("CAN msg {0}: {1}", 1, BitConverter.ToString(msg.Data, 0, 2));


                                if (msg.Data[0] == 0x79 && msg.CanId == CanId && msg.CMD == 0x03)
                                {

                                    if (i == (programForUpload.Length / 8 ))
                                    {
                                        var data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                                        tx.WriteToBuffer(data);
                                        data = programForUpload.Skip(8 * i).Take(8).ToArray();
                                        can.Send_Data(CanId, data);
                                        Console.WriteLine("Paskutinis");
                                       
                                    }
                                    else
                                    {
                                        var data = programForUpload.Skip(8 * i).Take(8).ToArray();
                                        can.Send_Data(CanId, data);
                                       Console.WriteLine("CAN msg {0}: {1}, {2}", i, BitConverter.ToString(data, 0, 4), BitConverter.ToString(data, 4, 4));
                                    }
                                    receive.ClearRxBuff();
                                }
                                Console.WriteLine("Done");
                            }
                        }

                        break;
                    case "2":  //JUMP_TO_USER_APP
                        can.Jump_To_User_App(CanId);
                        break;
                    case "3":  //JUMP_TO_BOOTLOADER
                        can.Jump_To_Bootloader(CanId);
                        break;
                    case "4":  //RESET_SINGLE 
                        can.Reset_Single(CanId);
                        break;
                    case "5":  //RESET_ALL
                        can.Reset_All();
                        break;
                    case "6":  //SET_CAN_BUS_SPEED
                        can.Set_Can_Bus_Speed(CanId, 500);
                        break;
                    case "7":  //TURN_ON_SINGLE_LED
                        Console.Write("Iveskite rites vieta: ");
                        var reelLocation = Int32.Parse(Console.ReadLine());
                        can.Turn_On_Single_Led(reelLocation, color, (byte)brightness);
                        break;
                    case "8":  //TURN_OFF_SINGLE_LED
                        Console.Write("Iveskite rites vieta: ");
                        var reelLocation2 = Int32.Parse(Console.ReadLine());
                        can.Turn_Off_Single_Led(reelLocation2);
                        break;
                    case "9":  //TURN_ON_ALL_LED
                        can.Turn_On_All_Led(color, (byte)brightness);
                        break;
                    case "10":  //TURN_OFF_ALL_LED
                        can.Turn_Off_All_Led();
                        break;
                    case "11":  //START_REELS_REGISTRATION
                        Console.Write("Iveskite rites ID: ");
                        var RitesId = Int32.Parse(Console.ReadLine());
                        can.Start_Reels_Registration(RitesId);
                        break;
                    case "12":  //STOP_REELS_REGISTRATION
                        can.Stop_Reels_Registration();
                        break;
                    case "13":  //START_REELS_TAKING
                        Console.Write("Iveskite rites ID: ");
                        var RitesId1 = Int32.Parse(Console.ReadLine());
                        Console.Write("Iveskite rites vieta: ");
                        var RitesLocation = Int32.Parse(Console.ReadLine());
                        can.Start_Reels_Taking(RitesLocation, RitesId1, color, (byte)brightness);
                        break;
                    case "20":  //CLEAR_EEPROM_MEMORY
                        can.Clear_EEPROM_Memory(CanId);
                        break;
                    case "XX":
                        CanRx receivedMsg = new();
                        while (true)
                        {
                            msg = receivedMsg.ReadRxBuffer();
                            if (msg is not null)
                            {
                                Console.WriteLine(msg.Data[0]);
                                Console.WriteLine("CMD: {0}", msg.CMD);
                                Console.WriteLine("CAN ID: {0}", msg.CanId);
                                Console.WriteLine("CAN msg {0}: {1}, {2}", 1, BitConverter.ToString(msg.Data, 0, 4), BitConverter.ToString(msg.Data, 4, 4));
                            }
                        }
                        break;
                    case "27":  //BOOTLOADER_FLAG_SET
                        can.Bootloader_Flag_Set(CanId);
                        break;
                    case "28":  //BOOTLOADER_FLAG_RESET
                        can.Bootloader_Flag_Reset(CanId);
                        break;
                    case "29":  //BOOTLOADER_FLAG_RESET
                        Console.Write("Iveskite nuo kurio ID numeruoti: ");
                        var startnum = Int32.Parse(Console.ReadLine());
                        can.Start_Node_ID_Numeration(startnum);
                        break;
                    case "ZZ":  //BOOTLOADER_FLAG_RESET
                        for (int i = 1; i <= 88; i++)
                        {
                            can.Bootloader_Flag_Set(i);
                            Thread.Sleep(5000);
                            can.Reset_Single(i);
                            Thread.Sleep(500);
                            can.Flash_Start_Write(i, programForUpload.Length, CRC);

                            CanRx receive1 = new();
                            CanMessage msg1 = receive1.ReadRxBuffer();

                            Console.WriteLine("CMD: {0}", msg1.CMD);
                            Console.WriteLine("CAN ID: {0}", msg1.CanId);
                            Console.WriteLine("CAN msg {0}: {1}", 1, BitConverter.ToString(msg1.Data, 0, 2));

                            CanTx tx1 = new();


                            if (msg1.Data[0] == 0x79 && msg1.CanId == i && msg1.CMD == 0x01)
                            {

                                Console.WriteLine("Flashing");
                                for (int a = 0; a < programForUpload.Length / 8 + 1; a++)
                                {

                                    msg1 = receive1.ReadRxBuffer();


                                    Console.WriteLine("CMD: {0}", msg1.CMD);
                                    Console.WriteLine("CAN ID: {0}", msg1.CanId);
                                    Console.WriteLine("CAN msg {0}: {1}", 1, BitConverter.ToString(msg1.Data, 0, 2));
                                    Console.WriteLine("Plokste:{0}", i);


                                    if (msg1.Data[0] == 0x79 && msg1.CanId == i && msg1.CMD == 0x03)
                                    {

                                        if (a == (programForUpload.Length / 8))
                                        {
                                            var data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                                            tx1.WriteToBuffer(data);
                                            data = programForUpload.Skip(8 * a).Take(8).ToArray();
                                            can.Send_Data(i, data);
                                            Console.WriteLine("Paskutinis");

                                        }
                                        else
                                        {
                                            var data = programForUpload.Skip(8 * a).Take(8).ToArray();
                                            can.Send_Data(i, data);
                                            Console.WriteLine("CAN msg {0}: {1}, {2}", a, BitConverter.ToString(data, 0, 4), BitConverter.ToString(data, 4, 4));
                                        }
                                        receive1.ClearRxBuff();
                                    }
                                    Console.WriteLine("Done");
                                }
                            }


                        }

                        Thread.Sleep(1000);
                        can.Bootloader_Flag_Reset(0);
                        Thread.Sleep(200);
                        can.Reset_All();



                        break;
                    default:
                        Console.WriteLine("Default");
                        break;
                }
            }
        }
    }
}
