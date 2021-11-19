using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding.Coders.Hamming
{
    public class HammingDecoder : Decoder
    {
        public HammingDecoder(string inputFilePath, string outputFilePath) : base(inputFilePath, outputFilePath) { }

        public override void Decode()
        {
            List<byte> input = ReadFile().ToList();
            string text = ReadText(input);
            int controlBitsCount = GetControlBitsCount(text.Length);
            int errorIndex = int.Parse(Calc.ConvertToBase(new string(HammingCoder.GetControlBits(controlBitsCount, text)
                .Reverse().ToArray()), 2, 10, 1e-16)) - 1;
            StringBuilder binaryOutput = new StringBuilder(text);
            if (errorIndex > -1)
                binaryOutput[errorIndex] = (char)(49 - (binaryOutput[errorIndex] - 48));
            string output = RemoveControlBits(binaryOutput.ToString(), controlBitsCount);

            List<byte> byteOutput = new List<byte>();
            for (int i = 0; i < output.Length; i += 8)
                byteOutput.Add(byte.Parse(Calc.ConvertToBase(output.Substring(i, 8), 2, 10, 1e-16)));
            WriteFile(new string(Encoding.GetEncoding(1251).GetChars(byteOutput.ToArray())));
        }

        private string RemoveControlBits(string text, int controlBitsCount)
        {
            for (int i = controlBitsCount - 1; i >= 0; --i)
                text = text.Remove((1 << i) - 1, 1);

            return text;
        }

        private string ReadText(List<byte> text)
        {
            byte lastByteUsed = text[0];
            text.RemoveAt(0);

            string res = "";

            for (int i = 0; i < text.Count - 1; ++i)
            {
                string num = Calc.ConvertToBase(text[i].ToString(), 10, 2, 1e-16);
                res += new string('0', 8 - num.Length) + num;
            }
            res += Calc.ConvertToBase((text[text.Count - 1] >> (8 - lastByteUsed)).ToString(), 10, 2, 1e-16);

            return res;
        }

        private int GetControlBitsCount(int messageLength)
        {
            int controlBytes = 1;

            while (messageLength + 1 > (1 << controlBytes))
                ++controlBytes;

            return controlBytes;
        }
    }
}
