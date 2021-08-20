using Iot.Device.Mcp25xxx;
using Iot.Device.Mcp25xxx.Register;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Spi;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public class CanRx 
    {
        public CanInit canRx { get; set; }
        public object CANINTF { get; private set; }

        public CanRx()
        {
            CanInit canInit = new();
            canRx = canInit;
            SetRxParameters();
            ClearRxBuff();
        }

        private void SetRxParameters()
        {
            canRx.mcp2515.Write(Address.RxB0Ctrl, new byte[] { 0b0110_0000 });
            canRx.mcp2515.Write(Address.RxB1Ctrl, new byte[] { 0b0110_0000 });
        }
        public void ClearRxBuff()
        {
            canRx.mcp2515.Write(Address.CanIntF, new byte[] { 0b0000_0000 });
        }
        public int ReceiveDLC()
        {
            var RxB0Dlc = new BitArray(BitConverter.GetBytes(canRx.mcp2515.Read(Address.RxB0Dlc)).ToArray());
            RxB0Dlc[6] = false;
            RxB0Dlc[5] = false;
            RxB0Dlc[4] = false;

            return (Helpers.Helpers.getIntFromBitArray(RxB0Dlc));
        }
        public byte[] ReadRxBuffer()
        {
            BitArray CANINTF;
            byte[] bufferData;

            do
            {
                CANINTF = new BitArray(BitConverter.GetBytes(canRx.mcp2515.Read(Address.CanIntF)).ToArray());
            } while (CANINTF[0] == false);
            
            if (CANINTF[0] == true)
            {
                bufferData = canRx.mcp2515.ReadRxBuffer(RxBufferAddressPointer.RxB0D0, 8);
            }
            else
            {
                bufferData = canRx.mcp2515.ReadRxBuffer(RxBufferAddressPointer.RxB0D0, 8);
            }

            return (bufferData);

        }


        public static Mcp25xxx GetMcp25xxxDevice()
        {
            var settings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 10_000_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };
            var spiDevice = SpiDevice.Create(settings);
            return new Mcp25625(spiDevice);
        }
        public void SetReelLocation()
        {

            using (Mcp25xxx mcp25xxx = GetMcp25xxxDevice())
            {


                // Reset(mcp25xxx);
                mcp25xxx.Write(Address.Cnf1, new byte[] { 0b0000_0000 });
                mcp25xxx.Write(Address.Cnf2, new byte[] { 0b1001_0001 });
                mcp25xxx.Write(Address.Cnf3, new byte[] { 0b0000_0001 });

                //Rx buferio parametrai 
                mcp25xxx.Write(Address.RxB0Ctrl, new byte[] { 0b0110_0000 });
                mcp25xxx.Write(Address.RxB1Ctrl, new byte[] { 0b0110_0000 });

                //isvalau rx bufferius 
                mcp25xxx.Write(Address.CanIntF, new byte[] { 0b0000_0000 });


                var CANINTF = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.CanIntF)).ToArray());


                while (CANINTF[0] == false)
                {
                    CANINTF = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.CanIntF)).ToArray());
                }



                var CANINTE = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.CanIntE)).ToArray());

                byte[] data1 = mcp25xxx.ReadRxBuffer(RxBufferAddressPointer.RxB0D0, 8);
                byte[] data2 = mcp25xxx.ReadRxBuffer(RxBufferAddressPointer.RxB1D0, 8);
                var bufferData = data1;



                byte STID0 = mcp25xxx.Read(Address.RxB0Sidh);
                byte STID1 = mcp25xxx.Read(Address.RxB0Sidl);



                //Nuskaito registrus ID paieskai ir konvertuoja i bitu masyva. 
                var bits1 = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.RxB0Sidh)).ToArray());
                var bits2 = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.RxB0Sidl)).ToArray());


                var RxB0Dlc = new BitArray(BitConverter.GetBytes(mcp25xxx.Read(Address.RxB0Dlc)).ToArray());
                RxB0Dlc[6] = false;
                RxB0Dlc[5] = false;
                RxB0Dlc[4] = false;

                int DLC = Helpers.Helpers.getIntFromBitArray(RxB0Dlc);


                //surasau bitus is dvieju skirtingu adresu i viena masyva
                bool[] bits3 = new bool[11] { bits2[5], bits2[6], bits2[7], bits1[0], bits1[1], bits1[2], bits1[3], bits1[4], bits1[5], bits1[6], bits1[7] };

                BitArray myBA2 = new BitArray(bits3);

                //bitu masyva pakeiciu i integer skaiciu kuris parodo atejusio CAN paketo ID 
                int ID = Helpers.Helpers.getIntFromBitArray(myBA2);

                Console.WriteLine(DLC);
                Console.WriteLine(ID);
                if (data1[2] == 0x00)
                {
                    bufferData = data2;
                }



            }


        }
    }
}
