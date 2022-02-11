using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
    public class CanMessageDTO
    {
        public int CanId { get; set; }
        public int CMD { get; set; }
        public List<byte> Data { get; set; }
    }
}
