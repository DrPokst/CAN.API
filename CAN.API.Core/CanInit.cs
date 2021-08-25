using Iot.Device.Mcp25xxx;
using Iot.Device.Mcp25xxx.Register;
using System.Device.Spi;


namespace CAN.API.Core
{
    public class CanInit
    {
        public Mcp25xxx mcp2515 { get; set; }

        public CanInit()
        {
            var settings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 10_000_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            var spiDevice = SpiDevice.Create(settings);
            mcp2515 = new Mcp25625(spiDevice);

            mcp2515.Write(Address.Cnf1, new byte[] { 0b0000_0000 });
            mcp2515.Write(Address.Cnf2, new byte[] { 0b1001_0001 });
            mcp2515.Write(Address.Cnf3, new byte[] { 0b0000_0001 });
        }

    }
}
