using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CAN.API.Core
{
    public class ReadBinary
    {
        public static byte[] Masyvas { get; set; }

        public ReadBinary(string path)
        {
            byte[] readText = File.ReadAllBytes(path);
            Masyvas = readText;
        }
        public void SendBinaryFile()
        {/*
            CanTx transmit = new();
            byte[] data = new byte[] { };

            for (int i = 0; i < (Masyvas.Length / 8) + 1; i++)
            {
                data = Masyvas.Skip(8 * i).Take(8).ToArray();
                transmit.TransmitMessage(data);
                Thread.Sleep(20);
            }*/
        }
        public void ZygioAlgortimas()
        {/*
            CanTx transmit = new();
            byte[] data = new byte[] { };
            CanRx receive = new();

            for (int i = 0; i < (Masyvas.Length / 8) + 1; i++)
            {
                var msg = receive.ReadRxBuffer();
                if (msg[0] == 0x4F && msg[1] == 0x4B)
                {

                    if (i == (Masyvas.Length / 8))
                    {
                        data = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        transmit.WriteToBuffer(data);
                        data = Masyvas.Skip(8 * i).Take(8).ToArray();
                        Console.WriteLine("CAN msg {0}: {1}", i, BitConverter.ToString(data, 0, 2));
                        transmit.TransmitMessage(data);
                    }
                    else
                    {
                        data = Masyvas.Skip(8 * i).Take(8).ToArray();
                        transmit.TransmitMessage(data);
                        Console.WriteLine("CAN msg {0}: {1}, {2}", i, BitConverter.ToString(data, 0, 4), BitConverter.ToString(data, 4, 4));
                    }
                    receive.ClearRxBuff();
                }
            }*/
        }

    }
}
