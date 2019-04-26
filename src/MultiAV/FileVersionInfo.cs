using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

public class FileversionInfo
{
    private XMLParser raport;

    public FileversionInfo(string FiletoScan, ref XMLParser raport)
    {
        this.raport = raport;
        FileVersionInfo filever = FileVersionInfo.GetVersionInfo(FiletoScan);
        try
        {
            var peHeader = new PeNet.PeFile(FiletoScan);
            if (peHeader.IsSigned)
            {
                raport.AddFileVersionSigned(filever.Comments != null ? filever.Comments.ToString() : "", filever.CompanyName != null ? filever.CompanyName.ToString() : ""
                                            , filever.FileBuildPart.ToString() != null ? filever.FileBuildPart.ToString() : "", filever.FileDescription != null ? filever.FileDescription.ToString() : "",
                                            filever.FileVersion != null ? filever.FileVersion.ToString() : "", filever.InternalName != null ? filever.InternalName.ToString() : ""
                                            , filever.Language != null ? filever.Language.ToString() : "", filever.SpecialBuild != null ? filever.SpecialBuild.ToString() : "");

                if (peHeader.IsSignatureValid)
                {
                    raport.AddFileVersionSignedValid(peHeader.PKCS7);
                }
                else
                {
                    raport.AddFileVersionSignedInvalid();
                }

            }
            else
            {
                raport.AddFileVersionNotSigned(filever.Comments != null ? filever.Comments.ToString() : "", filever.CompanyName != null ? filever.CompanyName.ToString() : ""
                                            , filever.FileBuildPart.ToString() != null ? filever.FileBuildPart.ToString() : "", filever.FileDescription != null ? filever.FileDescription.ToString() : "",
                                            filever.FileVersion != null ? filever.FileVersion.ToString() : "", filever.InternalName != null ? filever.InternalName.ToString() : ""
                                            , filever.Language != null ? filever.Language.ToString() : "", filever.SpecialBuild != null ? filever.SpecialBuild.ToString() : "");
            }
        }
        catch (Exception)
        {
           
        }
        
      
           
        
    }

}
