using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coding.Coders;

namespace coding.Coders.Hamming
{
    public class HammingEncoder : Encoder
    {
        public HammingEncoder(string inputFilePath, string outputFilePath) : base(inputFilePath, outputFilePath) { }

        public override void Encode()
        {
            byte[] input = ReadFile();
            int controlBitsCount = GetControlBitsCount(input.Length * 8);
            string output = CreateOutputPattern(input, controlBitsCount);
            string controlBits = HammingCoder.GetControlBits(controlBitsCount, output);
            output = InsertControlBits(output, controlBits);

            List<byte> toFile = WriteAllBytes(output);
            toFile.Insert(0, (byte)((((output.Length % 8) + 7) % 8) + 1));
            WriteFile(toFile);
        }

        private int GetControlBitsCount(int messageLength)
        {
            int controlBytes = 1;

            while (messageLength + controlBytes + 1 > (1 << controlBytes))
                ++controlBytes;

            return controlBytes;
        }

        private string CreateOutputPattern(byte[] input, int controlBitsCount)
        {
            string pattern = "";

            foreach (byte b in input)
            {
                string num = Calc.ConvertToBase(b.ToString(), 10, 2, 1e-16);
                pattern += new string('0', 8 - num.Length) + num;
            }
            pattern.TrimStart('0');

            for (int i = 0; i < controlBitsCount; ++i)
                pattern = pattern.Insert((1 << i) - 1, "0");

            return pattern;
        }

        private string InsertControlBits(string text, string controlBits)
        {
            StringBuilder encodedText = new StringBuilder(text);

            for (int i = 0; i < controlBits.Length; ++i)
                encodedText[(1 << i) - 1] = controlBits[i];

            return encodedText.ToString();
        }

        public override double GetEncodingPrice()
        {
            byte[] input = ReadFile();
            int controlBitsCount = GetControlBitsCount(input.Length);
            string output = CreateOutputPattern(input, controlBitsCount);
            string controlBits = HammingCoder.GetControlBits(controlBitsCount, output);
            output = InsertControlBits(output, controlBits);

            return (double)output.Length / input.Length;
        }
    }
}
