using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class Scanner
{
    private readonly string runLocation;
    private string arguments;
    private string regex;
    private string nazwa;
    private XMLParser raport;
    private string version;

    /// <summary>
    /// Creates a new scanner
    /// </summary>
    /// <param name="runLocation">The location of the file e.g. C:\Program Files\Windows Defender\MpCmdRun.exe</param>
    public Scanner(string runLocation, string arguments, string regex, ref XMLParser raport, string nazwa)
    {
        if (!File.Exists(runLocation))
        {
            throw new FileNotFoundException();
        }
        this.runLocation = new FileInfo(runLocation).FullName;
        version = FileVersionInfo.GetVersionInfo(runLocation).FileVersion;
        this.arguments = arguments ?? throw new ArgumentNullException("Arguments in config file cannot be null!");
        this.regex = regex ?? throw new ArgumentNullException("Regex in config file cannot be null!");
        this.nazwa = nazwa;
        this.raport = raport;
    }

    /// <summary>
    /// Scan a single file
    /// </summary>
    /// <param name="file">The file to scan</param>
    /// <param name="timeoutInMs">The maximum time in milliseconds to take for this scan</param>
    /// <returns>The scan result</returns>
    /// 
    public string Scan(string file, int timeoutInMs = 30000)
    {
        if (!File.Exists(file))
        {
            raport.AddAntywirusyNoFound(nazwa, version, "FileNotFound");
            return "FileNotFound";
        }

        var fileInfo = new FileInfo(file);

        var process = new Process();

        var startInfo = new ProcessStartInfo(this.runLocation)
        {
            Arguments = arguments,
            CreateNoWindow = true,
            ErrorDialog = false,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        process.StartInfo = startInfo;
        process.Start();
       
        process.WaitForExit(timeoutInMs);
        if (!process.HasExited)
        {
            process.Kill();
            raport.AddAntywirusyNoFound(nazwa, version, "Timeout");
            return "Timeout";
        }
        string output = process.StandardOutput.ReadToEnd();
        if (Regex.IsMatch(output, regex, RegexOptions.IgnoreCase))
        {
            string pest_name = Regex.Match(output, regex, RegexOptions.IgnoreCase).Groups[1].Value;
            pest_name = Regex.Replace(pest_name, @"\r\n?|\n", "");
            raport.AddAntywirusyFound(nazwa, version,"ThreatFound", pest_name);
            return "ThreatFound";
        }
        else
        {
            raport.AddAntywirusyNoFound(nazwa, version, "NoThreatFound");
            return "NoThreatFound";
        }
    }
}
