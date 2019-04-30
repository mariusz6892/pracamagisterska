using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Salaros.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

class Program
{

    [DllImport("ole32.dll")]
    static extern int StgIsStorageFile(
  [MarshalAs(UnmanagedType.LPWStr)]
    string pwcsName);

    public static string SHA256CheckSum(string filePath)
    {
        using (SHA256 SHA256 = SHA256.Create())
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return BitConverter.ToString(SHA256.ComputeHash(fileStream)).Replace("-", string.Empty);
        }
    }

    static int Main(string[] args)
    {
        Console.WriteLine("Press enter to scan");
        Console.ReadLine();
        var sw = Stopwatch.StartNew();

        //var fileToScan = @"C:\Users\MarWin\Desktop\Praca magisterska\eicar.com";
        //var fileToScan = @"C:\Users\MarWin\Desktop\Cities.Skylines.Industries.Update.v1.11.1-f2\Update\Setup.exe";
        //var fileToScan = @"C:\Users\MarWin\Downloads\ProtonVPN_win_v1.7.4.exe";
        //var fileToScan = @"C:\Users\MarWin\Downloads\MicrosoftSpeechPlatformSDK.msi";
        //var fileToScan = @"C:\Users\MarWin\Downloads\TIPS 3.31 wytyczne 2018-2019.doc";
        //var fileToScan = @"C:\Users\MarWin\Downloads\Hdk-i-nagrodowy.docx";
        //var fileToScan = @"C:\Users\MarWin\Desktop\Praca magisterska\PEReader.dll";
        //var fileToScan = @"D:\TS\createfileassoc.exe";
        //var fileToScan = @"C:\Users\MarWin\Desktop\Praca magisterska\eicardropper.pdf";
        var fileToScan = @"C:\Users\MarWin\Pictures\10a.jpg";

        if (!File.Exists(fileToScan))
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return 0;
        }
       //ścieżka do pliku skanowanego
       var fileInfo = new FileInfo(fileToScan);
       //wczytanie configu
       var configFileFromPath = new ConfigParser(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\config.cfg", new ConfigParserSettings
       {
            MultiLineValues = MultiLineValues.Simple | MultiLineValues.AllowValuelessKeys,
            Culture = new CultureInfo("en-US")
       });
        var filenameWithoutPath = Path.GetFileName(fileToScan);
        var size = fileInfo.Length;
        string sha256 = SHA256CheckSum(fileToScan);
        XMLParser raport = new XMLParser(filenameWithoutPath,size,sha256);
        //stworzenie kolejki skanerów na podstawie configu
        ScanQueue queue = new ScanQueue(fileToScan, configFileFromPath, ref raport);
        sw.Stop();
        //BasicProperties
        BasicProperties basic = new BasicProperties(fileToScan, ref raport);
        //FileVersionInfo
        FileversionInfo fileversioninfo = new FileversionInfo(fileToScan, ref raport);

        if (StgIsStorageFile(fileToScan) == 0) // jezeli jest OLE
        {
            OLECompoundFileInfo ole = new OLECompoundFileInfo(fileToScan, ref raport);
        }
        else
        {
            try
            {
                //PEHeader
                PEHeader peheader = new PEHeader(fileToScan, ref raport);
                PESections pesections = new PESections(fileToScan, ref raport);
                PEImports peimports = new PEImports(fileToScan, ref raport);
                PEResources peresources = new PEResources(fileToScan, ref raport);
            }
            catch (ArgumentException) //jeżeli nie jest valid PE, ponieważ może nie być OLE i nie być PE 
            {
            }
        }
        //EXIF
        //Wykorzystanie programu EXIFTOOLS do wybrania metadanych
        ExIF exif = new ExIF(fileToScan, ref raport);
        //wypisanie XDocument
        raport.WritetoFile("raport"+ sha256 + ".xml");
        Console.WriteLine($"Completed scan in {sw.ElapsedMilliseconds}ms");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
        return 0;
    }
}
