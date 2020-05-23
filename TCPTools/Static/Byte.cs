using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPTools.Static
{
    static class ByteHelper
    {
        public static IEnumerable<byte[]> Split(this byte[] value, int bufferLength)
        {
            int countOfArray = value.Length / bufferLength;
            if (value.Length % bufferLength > 0)
                countOfArray++;
            for (int i = 0; i < countOfArray; i++)
            {
                yield return value.Skip(i * bufferLength).Take(bufferLength).ToArray();
            }
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
