using Salaros.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class ScanQueue
    {
        private string _FileToScan; //ścieżka pliku skanowanego
        private ConfigParser _config; //congig
        private string[] Scannersstring; //tablica pomocnicza nazw antywirusów
        private Scanner[] Scanners; //kolejka skanerów

        public ScanQueue(string FiletoScan, ConfigParser config, ref XMLParser raport)
        {
            _FileToScan = FiletoScan;
            _config = config;
            Scannersstring = config.GetArrayValue("Antivirus", "Scanners");
            Scanners = new Scanner[Scannersstring.Length];
            for (int i = 0; i < Scannersstring.Length; i++)
            {
                var arguments = config.GetValue(Scannersstring[i], "arguments");
                arguments = string.Format(arguments, _FileToScan);
                Scanners[i] = new Scanner(config.GetValue(Scannersstring[i], "exeLocation"), arguments, config.GetValue(Scannersstring[i], "regex"), ref raport, Scannersstring[i]);
            }

            for (int i = 0; i < Scanners.Length; i++)
            {
                var result = Scanners[i].Scan(_FileToScan , 10000);
                Console.WriteLine(result);

            }
        }
    }
