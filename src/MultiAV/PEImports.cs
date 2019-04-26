using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 public class PEImports
{
    private Dictionary<string, List<string>> DLLs = new Dictionary<string, List<string>>();

    public PEImports(string FiletoScan, ref XMLParser raport)
    {
        try
        {
            var peHeader = new PeNet.PeFile(FiletoScan);
            for (int i = 0; i <= peHeader.ImportedFunctions.Length - 1; i++)
            {
                if (!DLLs.ContainsKey(peHeader.ImportedFunctions.ElementAt(i).DLL))
                {
                    DLLs.Add(peHeader.ImportedFunctions.ElementAt(i).DLL, new List<string>());
                    DLLs[peHeader.ImportedFunctions.ElementAt(i).DLL].Add(peHeader.ImportedFunctions.ElementAt(i).Name);
                }

                if (DLLs.ContainsKey(peHeader.ImportedFunctions.ElementAt(i).DLL))
                {
                    if (!DLLs[peHeader.ImportedFunctions.ElementAt(i).DLL].Contains(peHeader.ImportedFunctions.ElementAt(i).Name))
                        DLLs[peHeader.ImportedFunctions.ElementAt(i).DLL].Add(peHeader.ImportedFunctions.ElementAt(i).Name);
                }
            }
            raport.AddPEImportDLL(DLLs);
        }
        catch (Exception)
        {

        }
        
    }
}
