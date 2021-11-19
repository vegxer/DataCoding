using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding.Coders
{
    public abstract class Encoder : Coder<List<byte>, byte[]>, IEncodable
    {
        protected Encoder(string inputFilePath, string outputFilePath) : base(inputFilePath, outputFilePath) { }

        public double GetCompressionCoeff()
        {
            return (1 - (double)File.ReadAllBytes(outputFilePath).Length / File.ReadAllBytes(inputFilePath).Length) * 100;
        }

        protected List<byte> WriteAllBytes(string bytes)
        {
            List<byte> res = new List<byte>();
            for (int i = 0; i < bytes.Length; i += 8)
                res.Add(WriteByte(bytes.Substring(i, Math.Min(8, bytes.Length - i)), 0, 0));
            return res;
        }

        protected byte WriteByte(string strByte, byte codingByte, byte position)
        {
            int start = position;
            int size = start + strByte.Length;
            for (; position < size; ++position)
                codingByte |= (byte)(int.Parse(strByte[position - start].ToString())
                    << (8 - position - 1));

            return codingByte;
        }

        public override void WriteFile(List<byte> encodedText)
        {
            File.WriteAllBytes(outputFilePath, encodedText.ToArray());
        }

        public override byte[] ReadFile()
        {
            return File.ReadAllBytes(inputFilePath);
        }

        public abstract void Encode();
        public abstract double GetEncodingPrice();
    }
}
