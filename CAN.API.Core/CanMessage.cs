using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public class CanMessage
    {
        public int CanId { get; set; }
        public int CMD { get; set; }
        public byte[] Data { get; set; }
    }
}
