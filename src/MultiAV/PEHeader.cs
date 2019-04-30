using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PEHeader
{

    public PEHeader(string FiletoScan, ref XMLParser raport)
    {
        try
        {
            var peHeader = new PeNet.PeFile(FiletoScan);
            if (peHeader.ImageNtHeaders.Signature != 17744) throw new ArgumentException("No PE", "original");
            // 1774 = 4550h czyli po ludzku PE00 kazdy plik PE ma taką wartość, bez niej nie ma sensu sprawdzać header
            var containedsections = peHeader.ImageSectionHeaders.Length;
            var entrypoint = peHeader.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
            var targetmachine = PeNet.Utilities.FlagResolver.ResolveTargetMachine(peHeader.ImageNtHeaders.FileHeader.Machine);
            var compilationtimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(peHeader.ImageNtHeaders.FileHeader.TimeDateStamp);
            raport.AddPEHeader(targetmachine, compilationtimestamp.ToString(), entrypoint.ToString(), containedsections.ToString());
        }
        catch (Exception)
        {
            throw new ArgumentException("No PE", "original");
        } 
    }
}
