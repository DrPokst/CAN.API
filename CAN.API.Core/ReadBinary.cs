using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public void SenBinaryFile()
        {

            CanInit canInit = new();
            CanTx transmit = new(canInit);
            byte[] data = new byte[] { };

            for (int i = 0; i < (Masyvas.Length / 8) + 1; i++)
            {
                data = Masyvas.Skip(8 * i).Take(8).ToArray();
                transmit.TransmitMessage(data);
                Thread.Sleep(20);
            }
        }

    }
}
