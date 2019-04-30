using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;



public class PESections
{
    private readonly double log2 = 1.44269504088896340736;
    public PESections(string FiletoScan, ref XMLParser raport)
    {
            var peHeader = new PeNet.PeFile(FiletoScan);
            for (int i = 0; i <= peHeader.ImageSectionHeaders.Length - 1 ; i++)
            {
                uint[] byte_count = new uint[256];
                byte[] tempmd5 = new byte[peHeader.ImageSectionHeaders.ElementAt(i).SizeOfRawData];
                for (uint j = peHeader.ImageSectionHeaders.ElementAt(i).PointerToRawData;
                j < peHeader.ImageSectionHeaders.ElementAt(i).PointerToRawData + peHeader.ImageSectionHeaders.ElementAt(i).SizeOfRawData; j++)
                {
                    ++byte_count[(char)peHeader.Buff[j]];
                    tempmd5[j - peHeader.ImageSectionHeaders.ElementAt(i).PointerToRawData] = peHeader.Buff[j];
                }
                using (MD5 MD5 = MD5.Create())
                {
                    raport.AddPESection(PeNet.Utilities.FlagResolver.ResolveSectionName(peHeader.ImageSectionHeaders.ElementAt(i).Name), peHeader.ImageSectionHeaders.ElementAt(i).VirtualAddress.ToString(),
                    peHeader.ImageSectionHeaders.ElementAt(i).VirtualSize.ToString(), peHeader.ImageSectionHeaders.ElementAt(i).SizeOfRawData.ToString(),
                    CountEntropy(byte_count, peHeader.ImageSectionHeaders.ElementAt(i).SizeOfRawData).ToString(), BitConverter.ToString(MD5.ComputeHash(tempmd5)).Replace("-", string.Empty));
                }
            }
            
    }

    private double CountEntropy(uint[] byte_count, uint sizeOfRawData)
    {
        double entropy = 0.0;
        for (uint i = 0; i < 256; i++)
        {
            double temp = (double)byte_count[i] / sizeOfRawData;
            if (temp > 0) entropy += Math.Abs(temp * (Math.Log(temp) * log2));
        }
        return Math.Round(entropy,2);
    }


}