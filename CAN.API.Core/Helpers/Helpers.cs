using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAN.API.Core.Helpers
{
    public class Helpers
    {
        public static int getIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32) throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);

            return array[0];
        }
        public IEnumerable<byte> GetBytesFromByteString(string s)
        {
            for (int index = 0; index < s.Length; index += 2)
            {
                yield return Convert.ToByte(s.Substring(index, 2), 16);
            }
        }
    }
}
