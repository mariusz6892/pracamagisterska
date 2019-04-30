
using OpenMcdf;
using OpenMcdf.Extensions;
using OpenMcdf.Extensions.OLEProperties;
using OpenMcdf.Extensions.OLEProperties.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OLECompoundFileInfo
{
    public OLECompoundFileInfo(string FileToScan, ref XMLParser raport)
    {
        //OpenMcdf.Extensions.OLEProperties.
        //CFStorage
        CompoundFile file = new CompoundFile(FileToScan);
        CFStream summaryinformation = file.RootStorage.GetStream("\u0005SummaryInformation");
        PropertySetStream propertySetStream = summaryinformation.AsOLEProperties();
        raport.InitializeOle();
        for (int i = 0; i < propertySetStream.PropertySet0.NumProperties; i++)
        {
            raport.AddSummInfoAtt(propertySetStream.PropertySet0.PropertyIdentifierAndOffsets.ElementAt(i).PropertyIdentifier, propertySetStream.PropertySet0.Properties[i].Value.ToString()); 
        }
    }
}
