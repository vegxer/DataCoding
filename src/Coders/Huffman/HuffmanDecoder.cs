using coding.Coders.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding.Coders.Huffman
{
    public class HuffmanDecoder : DictionaryDecoder
    {
        public HuffmanDecoder(string inputFilePath, string outputFilePath) : base(inputFilePath, outputFilePath) { }
    }
}
