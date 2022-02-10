using Iot.Device.Mcp25xxx.Register;
using Iot.Device.Mcp25xxx.Register.CanControl;
using Iot.Device.Mcp25xxx.Register.MessageTransmit;

namespace CAN.API.Core
{
    public class CanTx : CanInit
        
    {
        public CanTx()
        {
        }
        private void SetTxParameters(int msgSize, byte canID, byte CMD)
        {
            mcp2515.WriteByte(
               new CanCtrl(CanCtrl.PinPrescaler.ClockDivideBy8,
                   false,
                   false,
                   false,
                   Iot.Device.Mcp25xxx.Tests.Register.CanControl.OperationMode.NormalOperation));

            mcp2515.Write(
                Address.TxB0Sidh,
                new byte[]
                {
                    new TxBxSidh(0, canID).ToByte(), new TxBxSidl(0, 0b000, false, CMD).ToByte(),
                    new TxBxEid8(0, 0b0000_0000).ToByte(), new TxBxEid0(0, 0b0000_0000).ToByte(),
                    new TxBxDlc(0, msgSize, false).ToByte()
                });
        }
        public void TransmitMessage(byte[] data, byte canID, byte CMD)
        {
            SetTxParameters(8, canID, CMD);
            mcp2515.Write(Address.TxB0D0, data);
            mcp2515.RequestToSend(true, false, false);
        }
        public void WriteToBuffer(byte[] data)
        {
            mcp2515.Write(Address.TxB0D0, data);
        }
    }
}
