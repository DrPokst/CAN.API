using Iot.Device.Mcp25xxx.Register;
using Iot.Device.Mcp25xxx.Register.CanControl;
using Iot.Device.Mcp25xxx.Register.MessageTransmit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public class CanTx
    {
        public CanInit canTx { get; set; }
        public CanTx(CanInit canInit)
        {
            canTx = canInit;
            SetTxParameters();
        }
        private void SetTxParameters()
        {
            canTx.mcp2515.WriteByte(
               new CanCtrl(CanCtrl.PinPrescaler.ClockDivideBy8,
                   false,
                   false,
                   false,
                   Iot.Device.Mcp25xxx.Tests.Register.CanControl.OperationMode.NormalOperation));

            canTx.mcp2515.Write(
                Address.TxB0Sidh,
                new byte[]
                {
                    new TxBxSidh(0, 0b0000_0000).ToByte(), new TxBxSidl(0, 0b000, false, 0b000).ToByte(),
                    new TxBxEid8(0, 0b0000_0000).ToByte(), new TxBxEid0(0, 0b0000_0000).ToByte(),
                    new TxBxDlc(0, 8, false).ToByte()
                });
        }
        public void TransmitMessage(byte[] data)
        {
            canTx.mcp2515.Write(Address.TxB0D0, data);
            // Send with TxB0 buffer.
            canTx.mcp2515.RequestToSend(true, false, false);
        }
    }
}
