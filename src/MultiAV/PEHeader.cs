using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PEHeader
{
    public bool moreheader;

    public PEHeader(string FiletoScan, ref XMLParser raport)
    {
        try
        {
            var peHeader = new PeNet.PeFile(FiletoScan);
            var containedsections = peHeader.ImageSectionHeaders.Length;
            var entrypoint = peHeader.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
            var targetmachine = PeNet.Utilities.FlagResolver.ResolveTargetMachine(peHeader.ImageNtHeaders.FileHeader.Machine);
            var compilationtimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(peHeader.ImageNtHeaders.FileHeader.TimeDateStamp);
            raport.AddPEHeader(targetmachine, compilationtimestamp.ToString(), entrypoint.ToString(), containedsections.ToString());
            moreheader = true;
        }
        catch(Exception)
        {
            raport.AddPEHeader("", "", "", "");
            moreheader = false;
        }
    }
}
