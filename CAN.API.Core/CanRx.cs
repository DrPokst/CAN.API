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

    }
}
