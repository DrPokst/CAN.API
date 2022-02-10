using Iot.Device.Mcp25xxx;
using Iot.Device.Mcp25xxx.Register;
using System;
using System.Collections;
using System.Linq;

namespace CAN.API.Core
{
    public class CanRx : CanInit
    {
        private BitArray CANINTF { get; set; }
        private byte[] rxBufferData { get; set; }


        public CanRx()
        {
            SetRxParameters();
            ClearRxBuff();
        }

        private void SetRxParameters()
        {
            mcp2515.Write(Address.RxB0Ctrl, new byte[] { 0b0110_0000 });
            mcp2515.Write(Address.RxB1Ctrl, new byte[] { 0b0110_0000 });
        }
        public void ClearRxBuff()
        {
            mcp2515.Write(Address.CanIntF, new byte[] { 0b0000_0000 });
        }
        public int ReceiveDLC()
        {
            var RxB0Dlc = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB0Dlc)).ToArray());
            RxB0Dlc[6] = false;
            RxB0Dlc[5] = false;
            RxB0Dlc[4] = false;
            mcp2515.Write(Address.RxB0Dlc, new byte[] { 0b0000_0000 });
            return (Helpers.getIntFromBitArray(RxB0Dlc));
        }
        public int ReceiveCanId()
        {
            BitArray RxBnSIDH;
            do
            {
                CANINTF = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.CanIntF)).ToArray());
            } while (CANINTF[0] == false && CANINTF[1] == false);

            if (CANINTF[0] == true)
            {
                RxBnSIDH = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB0Sidh)).ToArray());
            }
            else
            {
                RxBnSIDH = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB1Sidh)).ToArray());
            }
            
            return (Helpers.getIntFromBitArray(RxBnSIDH));

        }

        public int ReceiveCMD()
        {
            BitArray RxBnSIDL;
            do
            {
                CANINTF = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.CanIntF)).ToArray());
            } while (CANINTF[0] == false && CANINTF[1] == false);

            if (CANINTF[0] == true)
            {
                RxBnSIDL = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB0Sidl)).ToArray());
            }
            else
            {
                RxBnSIDL = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB1Sidl)).ToArray());
            }

            RxBnSIDL[0] = false;
            RxBnSIDL[1] = false;
            RxBnSIDL[2] = false;
            RxBnSIDL[3] = false;
            RxBnSIDL[4] = false;
            var res = new BitArray(3);
            res[2] = RxBnSIDL[7];
            res[1] = RxBnSIDL[6];
            res[0] = RxBnSIDL[5];
            return (Helpers.getIntFromBitArray(res));
        }

        public CanMessage ReadRxBuffer()
        {
            BitArray RxBnSIDL;
            BitArray RxBnSIDH;
            do
            {
                CANINTF = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.CanIntF)).ToArray());
            } while (CANINTF[0] == false && CANINTF[1] == false);

            if (CANINTF[0] == true)
            {
                rxBufferData = mcp2515.ReadRxBuffer(RxBufferAddressPointer.RxB0D0, 8);
                RxBnSIDH = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB0Sidh)).ToArray());
                RxBnSIDL = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB0Sidl)).ToArray());
            }
            else
            {
                rxBufferData = mcp2515.ReadRxBuffer(RxBufferAddressPointer.RxB1D0, 8);
                RxBnSIDH = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB1Sidh)).ToArray());
                RxBnSIDL = new BitArray(BitConverter.GetBytes(mcp2515.Read(Address.RxB1Sidl)).ToArray());
            }
            

            CanMessage canMessage = new CanMessage();
            canMessage.CanId = (Helpers.getIntFromBitArray(RxBnSIDH));
            RxBnSIDL[0] = false;
            RxBnSIDL[1] = false;
            RxBnSIDL[2] = false;
            RxBnSIDL[3] = false;
            RxBnSIDL[4] = false;
            var res = new BitArray(3);
            res[2] = RxBnSIDL[7];
            res[1] = RxBnSIDL[6];
            res[0] = RxBnSIDL[5];
            canMessage.CMD = Helpers.getIntFromBitArray(res);
            canMessage.Data = rxBufferData;

            ClearRxBuff();


            return (canMessage);
        }

    }
}
