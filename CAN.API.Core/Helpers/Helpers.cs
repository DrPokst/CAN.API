using System;
using System.Collections;
using System.Collections.Generic;

namespace CAN.API.Core
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
        public static IEnumerable<byte> GetBytesFromByteString(string s)
        {
            for (int index = 0; index < s.Length; index += 2)
            {
                yield return Convert.ToByte(s.Substring(index, 2), 16);
            }
        }
        public static Tuple<int, byte> IdCalculation(int id)
        {
            int tarpinis = (id - 1) / 10;
            int slotNr = id - (tarpinis * 10);
            byte ID = Convert.ToByte(tarpinis);

            return Tuple.Create(slotNr, ID);
        }
    }
}
