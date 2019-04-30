using AlphaOmega.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using HeyRed;
using System.Runtime.InteropServices;
using System.Globalization;

public class PEResources
{

    private Dictionary<string, int> ResourcesbyType = new Dictionary<string, int>();
    private Dictionary<string, int> ResourcesbyLanguage = new Dictionary<string, int>();

    public PEResources(string FiletoScan, ref XMLParser raport)
    {
            //var peHeader = new PeNet.PeFile(FiletoScan);
            using (PEFile info = new PEFile(StreamLoader.FromFile(FiletoScan)))
            {
                if (!info.Resource.IsEmpty)
                {
                    Int32 directoriesCount = 0;

                    foreach (var dir in info.Resource)
                    {
                        directoriesCount++;
                        foreach (var dir1 in dir)
                        {
                            foreach (var dir2 in dir1)
                            {
                                if (dir2.DirectoryEntry.IsDataEntry)
                                {
                                    ListResourcesbyLanguage(dir2.Name);
                                    ListResourcesbyType(dir.Name);
                                    Byte[] bytesM = dir2.GetData();
                                    using (SHA256 SHA256 = SHA256.Create())
                                    {
                                        if (Convert.ToInt32(dir2.Name) > 0)
                                        {
                                            CultureInfo LCID = new CultureInfo(Convert.ToInt32(dir2.Name), false);
                                            raport.AddPEResources(dir.Name, LCID.Name, BitConverter.ToString(SHA256.ComputeHash(bytesM)).Replace("-", string.Empty));
                                        }
                                        else raport.AddPEResources(dir.Name, "Neutral", BitConverter.ToString(SHA256.ComputeHash(bytesM)).Replace("-", string.Empty));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var items = from pair in ResourcesbyType
                        orderby pair.Value descending
                        select pair;
            raport.AddPEResourcesbyType(items);
            var items2 = from pair in ResourcesbyLanguage
                        orderby pair.Value descending
                        select pair;
            raport.AddPEResourcesbyLanguage(items2);
    }

    private void ListResourcesbyType(string type)
    {
        if (!ResourcesbyType.ContainsKey(type))
        {
            ResourcesbyType.Add(type, 1);
        }
        else ResourcesbyType[type]++;
    }
    private void ListResourcesbyLanguage(string language)
    {
        if (!ResourcesbyLanguage.ContainsKey(language))
        {
            ResourcesbyLanguage.Add(language, 1);
        }
        else ResourcesbyLanguage[language]++;
    }


}
