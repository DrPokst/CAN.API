using System;

namespace CAN.API.Web.DTOs
{
    public class CanDto
    {
        public int MsgLength { get; set; }
        public byte[] Msg { get; set; }
        public DateTime DateReceived { get; set; }
    }
}
