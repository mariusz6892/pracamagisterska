using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExIF
{
    private XMLParser raport;

    public ExIF(string FiletoScan, ref XMLParser raport)
    {
        this.raport = raport;
    }
}
