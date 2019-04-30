using Brain2CPU.ExifTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class ExIF
{

    public ExIF(string FiletoScan, ref XMLParser raport)
    {
        Dictionary<string, string> d;
        using (var etw = new ExifToolWrapper(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\exiftool(-k).exe"))
        {
            etw.Start();
            d = etw.FetchExifFrom(FiletoScan);
            if (!d.ContainsKey("Error"))
            {
                d.Remove("ExifTool Version Number");
                d.Remove("File Name");
                d.Remove("Directory");
                d.Remove("File Size");
                d.Remove("File Modification Date/Time");
                d.Remove("File Access Date/Time");
                d.Remove("File Creation Date/Time");
                d.Remove("File Permissions");
                raport.AddExIF(d);
            }
        }
    }
}
