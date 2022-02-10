using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core
{
   enum CMD
   {
        SET = 0x01,
        GET = 0x02,
        WRITE = 0x03,
        BROADCAST = 0x04,
   }
}
