using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Xml;
using System.Xml.Linq;

public class PDFInfo
{

    public PDFInfo(string FiletoScan, ref XMLParser raport)
    {
        doPython(FiletoScan, ref raport);
    }


    private static void doPython(string FiletoScan,ref XMLParser raport)
    {
        var engine = Python.CreateEngine();
        var searchPaths = engine.GetSearchPaths();
        searchPaths.Add(@"C:\Python27\Lib");
        engine.SetSearchPaths(searchPaths);

        

        List<String> argv = new List<String>();
        argv.Add(FiletoScan);

        engine.GetSysModule().SetVariable("argv", argv); // podanie argumentów do skryptu
        var scope = engine.ExecuteFile(AppDomain.CurrentDomain.BaseDirectory + "pdfid.py");
        dynamic pdfid = scope.GetVariable("PDFiD");
        var xml = pdfid(FiletoScan); // otrzymanie xml z metody pdfid
        dynamic cPDFiD = scope.GetVariable("cPDFiD"); //klasa cPDFiD
        dynamic cpdf = cPDFiD(xml,true); //objekt klasy cPDFiF
        IronPython.Runtime.PythonDictionary d = cpdf.keywords; //słownik z wynikami
        raport.InitializePDFInfo();
        foreach (string key in cpdf.keywords)
        {
            raport.AddPDFInfoAtt(key, d.get(key).ToString());
        }
    }

}
