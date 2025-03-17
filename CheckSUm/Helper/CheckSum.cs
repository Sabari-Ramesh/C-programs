using System.Numerics;

namespace CheckSUm.Helper
{
    public class CheckSum
    {
        public  string Sender(string number)
        {
            BigInteger num = BigInteger.Parse(number);
            string binary = ToBinaryString(num);
            int blockSize = 16;
            binary = binary.PadLeft((int)Math.Ceiling(binary.Length / 16.0) * 16, '0');
            string[] blocks = SplitIntoBlocks(binary, blockSize);
            ushort sum = CalculateChecksum(blocks);
            ushort checksum = (ushort)~sum;
            return $"{binary}|{Convert.ToString(checksum, 2).PadLeft(16, '0')}";
        }

        // Receiver: Validate the checksum
        public string Validate(string receivedData)
        {
            var parts = receivedData.Split('|');
            string data = parts[0];
            string checksumBin = parts[1];
            string[] blocks = SplitIntoBlocks(data, 16);
            ushort sum = CalculateChecksum(blocks);
            ushort receivedChecksum = Convert.ToUInt16(checksumBin, 2);
            sum += receivedChecksum;
            sum = (ushort)((sum & 0xFFFF) + (sum >> 16));
            if (sum == 0xFFFF) { 
                return sum.ToString();
            }
            return "Invaild Account Number";
        }

        // Helper to convert number to binary string
        private static string ToBinaryString(BigInteger num)
        {
            if (num == 0) return "0";
            string binary = "";
            num = BigInteger.Abs(num);
            while (num > 0)
            {
                binary = (num % 2) + binary;
                num /= 2;
            }
            return binary;
        }

        // Helper to split binary into blocks
        private static string[] SplitIntoBlocks(string binary, int blockSize)
        {
            int numBlocks = binary.Length / blockSize;
            string[] blocks = new string[numBlocks];
            for (int i = 0; i < numBlocks; i++)
                blocks[i] = binary.Substring(i * blockSize, blockSize);
            return blocks;
        }

        // Helper to calculate checksum with carry handling
        private static ushort CalculateChecksum(string[] blocks)
        {
            ushort sum = 0;
            foreach (var block in blocks)
            {
                sum += Convert.ToUInt16(block, 2);
                sum = (ushort)((sum & 0xFFFF) + (sum >> 16)); 
            }
            return sum;
        }

    }
}
