using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding.Coders.Hamming
{
    static class HammingCoder
    {
        public static string GetControlBits(int controlBitsCount, string encodingText)
        {
            byte[] controlBits = new byte[controlBitsCount];

            for (int i = 0; i < controlBitsCount; ++i)
            {
                int currDegree = 1 << i;
                for (int j = currDegree - 1; j < encodingText.Length; j += 2 * currDegree)
                {
                    for (int k = j; k < j + currDegree && k < encodingText.Length; ++k)
                    {
                        controlBits[i] ^= (byte)(encodingText[k] - 48);
                    }
                }
            }

            return string.Join("", controlBits);
        }
    }
}
