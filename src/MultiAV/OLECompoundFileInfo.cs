
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
        CompoundFile file = new CompoundFile(FileToScan);
        CFStream summaryinformation = file.RootStorage.GetStream("\u0005SummaryInformation");
        PropertySetStream propertySetStream = summaryinformation.AsOLEProperties();
        raport.InitializeOle();
        for (int i = 0; i < propertySetStream.PropertySet0.NumProperties; i++)
        {
            raport.AddSummInfoAtt(propertySetStream.PropertySet0.PropertyIdentifierAndOffsets.ElementAt(i).PropertyIdentifier, propertySetStream.PropertySet0.Properties[i].Value.ToString()); 
        }
        CFStream documentSummaryinformation = file.RootStorage.TryGetStream("\u0005DocumentSummaryInformation");
        if (documentSummaryinformation != null)
        {
            PropertySetStream propertyDSSetStream = documentSummaryinformation.AsOLEProperties();
            raport.InitializeDocOle();
            for (int i = 0; i < propertyDSSetStream.PropertySet0.NumProperties; i++)
            {
                raport.AddDocSummInfoAtt(propertyDSSetStream.PropertySet0.PropertyIdentifierAndOffsets.ElementAt(i).PropertyIdentifier, propertyDSSetStream.PropertySet0.Properties[i].Value.ToString());
            }
        }
    }
}
