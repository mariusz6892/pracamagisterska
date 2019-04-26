using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Text.RegularExpressions;
using PeNet.ImpHash;
using HeyRed.Mime;

public class BasicProperties
{

    public BasicProperties(string FiletoScan, ref XMLParser raport)
    {
        try
        {
            var peHeader = new PeNet.PeFile(FiletoScan);
            raport.AddBasicProperties(peHeader.MD5, peHeader.SHA1, AuthentihashCheckSum(FiletoScan), peHeader.ImpHash, MimeGuesser.GuessFileType(FiletoScan).MimeType, peHeader.FileSize.ToString());
        }
        catch(Exception)
        {
            raport.AddBasicProperties(MD5CheckSum(FiletoScan), SHA1CheckSum(FiletoScan), AuthentihashCheckSum(FiletoScan), "", MimeGuesser.GuessFileType(FiletoScan).MimeType, "");
        }
    }
       

    public static string AuthentihashCheckSum(string filePath)
    {
        var runspaceConfiguration = RunspaceConfiguration.Create();
        using (var runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
        {
            runspace.Open();
            using (var pipeline = runspace.CreatePipeline())
            {
                pipeline.Commands.AddScript("Get-AppLockerFileInformation  \"" + filePath + "\"");
                var results = pipeline.Invoke();
                runspace.Close();
                var result = results[0].BaseObject.ToString();
                string regex = "SHA256\\s0x(.+),";
                string authentihash = Regex.Match(result, regex, RegexOptions.IgnoreCase).Groups[1].Value;
                return authentihash;
            }
        }
    }
    public static string MD5CheckSum(string filePath)
    {
        using (MD5 MD5 = MD5.Create())
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return BitConverter.ToString(MD5.ComputeHash(fileStream)).Replace("-", string.Empty);
        }
    }
    public static string SHA1CheckSum(string filePath)
    {
        using (SHA1 SHA1 = SHA1.Create())
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return BitConverter.ToString(SHA1.ComputeHash(fileStream)).Replace("-", string.Empty);
        }
    }
}
