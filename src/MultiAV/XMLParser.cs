using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

public class XMLParser
{
        
    private XmlDocument _doc;
    public XmlDocument Doc
    {
        get { return _doc; }
        set { _doc = value; }
    }
    //Nodes of XML file
    private XmlNode raportNode;
    private XmlNode avsNode;
    private XmlNode fileversioninfoNode;
    private XmlNode DSNode;
    private XmlNode peheaderNode;
    private XmlNode pesectionsNode;
    private XmlNode peimportsNode;
    private XmlNode peresourcestypeNode;
    private XmlNode peresourseclanguageNode;
    private XmlNode peresourcesNode;
    private XmlNode exiftoolNode;
    private XmlNode basicPropertiesNode;
    private XmlNode oleNode;
    private XmlNode summaryoleNode;


    public XMLParser(string filename, long size, string sha256)
    {
        _doc = new XmlDocument();
        XmlNode docNode = _doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        _doc.AppendChild(docNode);
        raportNode = _doc.CreateElement("Raport");
        _doc.AppendChild(raportNode);
        XmlAttribute Size = _doc.CreateAttribute("Wielkosc_pliku");
        raportNode.Attributes.Append(Size);
        Size.Value = size.ToString();
        XmlAttribute Sha256 = _doc.CreateAttribute("Sha256");
        raportNode.Attributes.Append(Sha256);
        Sha256.Value = sha256;
        XmlAttribute Data = _doc.CreateAttribute("Date");
        raportNode.Attributes.Append(Data);
        Data.Value = DateTime.Now.ToString();
        XmlAttribute FileName = _doc.CreateAttribute("Nazwa_pliku");
        raportNode.Attributes.Append(FileName);
        FileName.Value = filename;
        avsNode = _doc.CreateElement("Antywirusy");
        raportNode.AppendChild(avsNode);

        basicPropertiesNode = _doc.CreateElement("Podstawowe_Wlasciwosci");
        raportNode.AppendChild(basicPropertiesNode);

        fileversioninfoNode = _doc.CreateElement("FileVersionInfo");
        raportNode.AppendChild(fileversioninfoNode);

        peheaderNode = _doc.CreateElement("PE_Header");
        raportNode.AppendChild(peheaderNode);

        pesectionsNode = _doc.CreateElement("PE_Sections");
        raportNode.AppendChild(pesectionsNode);

        peimportsNode = _doc.CreateElement("PE_Imports");
        raportNode.AppendChild(peimportsNode);

        peresourcestypeNode = _doc.CreateElement("Number_of_PE_resources_by_type");
        raportNode.AppendChild(peresourcestypeNode);

        peresourseclanguageNode = _doc.CreateElement("Number_of_PE_resources_by_language");
        raportNode.AppendChild(peresourseclanguageNode);

        peresourcesNode = _doc.CreateElement("PE_resources");
        raportNode.AppendChild(peresourcesNode);


        exiftoolNode = _doc.CreateElement("ExifTool_file_metadata");
        raportNode.AppendChild(exiftoolNode);


    }

    #region Antywirus methods
    public void AddAntywirusyFound(string AVname, string AVversion, string result, string pestname)
    {
        XmlNode AVNode = _doc.CreateElement("Antywirus");
        avsNode.AppendChild(AVNode);
          //avNode Attributes
        XmlAttribute NazwaAV = _doc.CreateAttribute("Nazwa_AV");
        XmlAttribute Result = _doc.CreateAttribute("Result");
        XmlAttribute PestName = _doc.CreateAttribute("Nazwa_szkodnika");
        XmlAttribute VersionAV = _doc.CreateAttribute("AVVersion");
        VersionAV.Value = AVversion;
        NazwaAV.Value = AVname;
        Result.Value = result;
        PestName.Value = pestname;
        AVNode.Attributes.Append(PestName);
        AVNode.Attributes.Append(Result);
        AVNode.Attributes.Append(VersionAV);
        AVNode.Attributes.Append(NazwaAV);
    }
    public void AddAntywirusyNoFound(string AVname, string AVversion, string result)
    {
        XmlNode AVNode = _doc.CreateElement("Antywirus");
        avsNode.AppendChild(AVNode);
        //avNode Attributes
        XmlAttribute NazwaAV = _doc.CreateAttribute("Nazwa_AV");
        XmlAttribute Result = _doc.CreateAttribute("Result");
        XmlAttribute VersionAV = _doc.CreateAttribute("AVVersion");
        VersionAV.Value = AVversion;
        NazwaAV.Value = AVname;
        Result.Value = result;
        AVNode.Attributes.Append(Result);
        AVNode.Attributes.Append(VersionAV);
        AVNode.Attributes.Append(NazwaAV);
        
       
    }
    #endregion

    #region BasicProperties methods
    public void AddBasicProperties(string md5, string sha1, string authentihash, string imphash, string filetype, string filesize)
    {
        //avNode Attributes
        XmlAttribute MD5 = _doc.CreateAttribute("MD5");
        XmlAttribute SHA1 = _doc.CreateAttribute("SHA1");
        XmlAttribute Authentihash = _doc.CreateAttribute("Authentihash");
        XmlAttribute Imphash = _doc.CreateAttribute("Imphash");
        XmlAttribute Filetype = _doc.CreateAttribute("FileType");
        XmlAttribute FileSize = _doc.CreateAttribute("FileSize");

        if (md5 == "")
        {
        }
        else
        {
            MD5.Value = md5;
            basicPropertiesNode.Attributes.Append(MD5);
        }
        if (sha1 == "")
        {
        }
        else
        {
            SHA1.Value = sha1;
            basicPropertiesNode.Attributes.Append(SHA1);
        }
        if (authentihash == "")
        {
        }
        else
        {
            Authentihash.Value = authentihash;
            basicPropertiesNode.Attributes.Append(Authentihash);
        }
        if (imphash == "")
        {
        }
        else
        {
            Imphash.Value = imphash;
            basicPropertiesNode.Attributes.Append(Imphash);
        }
        if (filesize == "")
        {
        }
        else
        {
            FileSize.Value = filesize;
            basicPropertiesNode.Attributes.Append(FileSize);
        }
        if (filetype == "")
        {
        }
        else
        {
            Filetype.Value = filetype;
            basicPropertiesNode.Attributes.Append(Filetype);
        }

      
    }
    #endregion

    #region Fileversion methods
    public void AddFileVersionSigned(string Comments, string CompanyName, string FileBuildPart, string FileDescription, string FileVersion, string InternalName, string Language,
                                        string SpecialBuild)
    {
        DSNode = _doc.CreateElement("Podpis_Cyfrowy");
        fileversioninfoNode.AppendChild(DSNode);
        //avNode Attributes
        
        AddFver(Comments, CompanyName, FileBuildPart, FileDescription, FileVersion, InternalName, Language, SpecialBuild);
        XmlAttribute isSigned = _doc.CreateAttribute("IsSigned");
        isSigned.Value = "True";
        DSNode.Attributes.Append(isSigned);
    }
    public void AddFileVersionSignedValid(System.Security.Cryptography.X509Certificates.X509Certificate2 pKCS7)
    {
        
        XmlAttribute ValidFrom = _doc.CreateAttribute("ValidFrom");
        ValidFrom.Value = pKCS7.NotBefore.ToString();
        DSNode.Attributes.Append(ValidFrom);
        XmlAttribute ValidTo = _doc.CreateAttribute("ValidTo");
        ValidTo.Value = pKCS7.NotAfter.ToString();
        DSNode.Attributes.Append(ValidTo);
        XmlAttribute alg = _doc.CreateAttribute("Algorithm");
        alg.Value = pKCS7.SignatureAlgorithm.FriendlyName;
        DSNode.Attributes.Append(alg);
        XmlAttribute issuer = _doc.CreateAttribute("Issuer");
        issuer.Value = Regex.Match(pKCS7.Issuer, "CN=(.+?),", RegexOptions.IgnoreCase).Groups[1].Value;
        DSNode.Attributes.Append(issuer);
        XmlAttribute thumb = _doc.CreateAttribute("Thumbprint");
        thumb.Value = pKCS7.Thumbprint;
        DSNode.Attributes.Append(thumb);
        XmlAttribute serialnb = _doc.CreateAttribute("SerialNumber");
        serialnb.Value = pKCS7.SerialNumber;
        DSNode.Attributes.Append(serialnb);
        XmlAttribute isvalid = _doc.CreateAttribute("IsValid");
        isvalid.Value = "True";
        DSNode.Attributes.Append(isvalid);
    }
    public void AddFileVersionSignedInvalid()
    {
        XmlAttribute isvalid = _doc.CreateAttribute("IsValid");
        isvalid.Value = "False";
        DSNode.Attributes.Append(isvalid);
    }

    private void AddFver(string Comments, string CompanyName, string FileBuildPart, string FileDescription, string FileVersion, string InternalName, string Language,
                         string SpecialBuild)
    {
        XmlNode fver = _doc.CreateElement("File_Ver_Info");
        fileversioninfoNode.AppendChild(fver);

        XmlAttribute comments = _doc.CreateAttribute("Comments");
        if(Comments == "")
        {
        }
        else
        {
            comments.Value = Comments;
            fver.Attributes.Append(comments);
        }
        
        
        XmlAttribute companyname = _doc.CreateAttribute("CompanyName");
        if (CompanyName == "")
        {
        }
        else
        {
            companyname.Value = CompanyName;
            fver.Attributes.Append(companyname);
        }
        
        XmlAttribute filebuildpart = _doc.CreateAttribute("FileBuildPart");
        if (FileBuildPart == "")
        {
        }
        else
        {
            filebuildpart.Value = FileBuildPart;
            fver.Attributes.Append(filebuildpart);
        }
       
        XmlAttribute filedescription = _doc.CreateAttribute("FileDescription");
        if (FileDescription == "")
        {
        }
        else
        {
            filedescription.Value = FileDescription;
            fver.Attributes.Append(filedescription);
        }
        
        XmlAttribute fileversion = _doc.CreateAttribute("FileVersion");
        if (FileVersion == "")
        {
        }
        else
        {
            fileversion.Value = FileVersion;
            fver.Attributes.Append(fileversion);
        }
        
        XmlAttribute internalname = _doc.CreateAttribute("InternalName");
        if (InternalName == "")
        {
        }
        else
        {
            internalname.Value = InternalName;
            fver.Attributes.Append(internalname);
        }
        
        XmlAttribute language = _doc.CreateAttribute("Language");
        if (Language == "")
        {
        }
        else
        {
            language.Value = Language;
            fver.Attributes.Append(language);
        }
        
        XmlAttribute specialbuild = _doc.CreateAttribute("SpecialBuild");
        if (SpecialBuild == "")
        {
        }
        else
        {
            specialbuild.Value = SpecialBuild;
            fver.Attributes.Append(specialbuild);
        }
        
    }

    public void AddFileVersionNotSigned(string Comments, string CompanyName, string FileBuildPart, string FileDescription, string FileVersion, string InternalName, string Language,
                                        string SpecialBuild)
    {
        XmlNode DSNode = _doc.CreateElement("Podpis_Cyfrowy");
        fileversioninfoNode.AppendChild(DSNode);
        XmlAttribute isSigned = _doc.CreateAttribute("IsSigned");
        isSigned.Value = "False";
        DSNode.Attributes.Append(isSigned);

        AddFver(Comments, CompanyName, FileBuildPart, FileDescription, FileVersion, InternalName, Language, SpecialBuild);

    }
    #endregion

    #region PEHeader methods
    public void AddPEHeader(string targetmachine, string compilationtimestamp, string  entrypoint, string containedsections)
    {
        XmlAttribute Targetmachine = _doc.CreateAttribute("TargetMachine");
        XmlAttribute Compilationtimestamp = _doc.CreateAttribute("CompilationTimestamp");
        XmlAttribute Entrypoint = _doc.CreateAttribute("EntryPoint");
        XmlAttribute Containedsections = _doc.CreateAttribute("ContainedSections");

        if (targetmachine == "")
        {
        }
        else
        {
            Targetmachine.Value = targetmachine;
            peheaderNode.Attributes.Append(Targetmachine);
        }
        if (compilationtimestamp == "")
        {
        }
        else
        {
            Compilationtimestamp.Value = compilationtimestamp;
            peheaderNode.Attributes.Append(Compilationtimestamp);
        }
        if (entrypoint == "")
        {
        }
        else
        {
            Entrypoint.Value = entrypoint;
            peheaderNode.Attributes.Append(Entrypoint);
        }
        if (containedsections == "")
        {
        }
        else
        {
            Containedsections.Value = containedsections;
            peheaderNode.Attributes.Append(Containedsections);
        }
    }
    #endregion

    #region PESections methods
    public void AddPESection(string name, string virtualadress, string virtualsize, string rawsize, string entropy, string md5)
    {
        XmlNode SectionName = _doc.CreateElement(name.Substring(1));
        pesectionsNode.AppendChild(SectionName);
        XmlAttribute VirtualAddress = _doc.CreateAttribute("VirtualAddress");
        XmlAttribute VirtualSize = _doc.CreateAttribute("VirtualSize");
        XmlAttribute RawSize = _doc.CreateAttribute("RawSize");
        XmlAttribute Entropy = _doc.CreateAttribute("Entropy");
        XmlAttribute MD5 = _doc.CreateAttribute("MD5");

        if (virtualadress == "")
        {
        }
        else
        {
            VirtualAddress.Value = virtualadress;
            SectionName.Attributes.Append(VirtualAddress);
        }
        if (virtualsize == "")
        {
        }
        else
        {
            VirtualSize.Value = virtualsize;
            SectionName.Attributes.Append(VirtualSize);
        }
        if (rawsize == "")
        {
        }
        else
        {
            RawSize.Value = rawsize;
            SectionName.Attributes.Append(RawSize);
        }
        if (entropy == "")
        {
        }
        else
        {
            Entropy.Value = entropy;
            SectionName.Attributes.Append(Entropy);
        }
        if (md5 == "")
        {
        }
        else
        {
            MD5.Value = md5;
            SectionName.Attributes.Append(MD5);
        }
    }
    #endregion

    #region PEImports methods
    public void AddPEImportDLL(Dictionary<string, List<string>> DLLs)
    {
        foreach (KeyValuePair<string, List<string>> kvp in DLLs)
        {
            XmlNode SectionName = _doc.CreateElement(kvp.Key);
            peimportsNode.AppendChild(SectionName);
            foreach(string el in kvp.Value)
            {
                XmlNode FunctionName = _doc.CreateElement(el);
                SectionName.AppendChild(FunctionName);
            }
        }
    }
    #endregion

    #region PEResources methods
    public void AddPEResources(string type, string language, string sha256)
    {
        XmlNode SectionName = _doc.CreateElement(type);
        peresourcesNode.AppendChild(SectionName);
        XmlAttribute Language = _doc.CreateAttribute("Language");
        XmlAttribute Sha256 = _doc.CreateAttribute("Sha256");


        Language.Value = language;
        SectionName.Attributes.Append(Language);

        Sha256.Value = sha256;
        SectionName.Attributes.Append(Sha256);

    }
    public void AddPEResourcesbyType(IOrderedEnumerable<KeyValuePair<string,int>> Types)
    {
        foreach (KeyValuePair<string, int> kvp in Types)
        {
            XmlNode SectionName = _doc.CreateElement(kvp.Key);
            peresourcestypeNode.AppendChild(SectionName);
            XmlAttribute Number = _doc.CreateAttribute("Number");
            Number.Value = kvp.Value.ToString();
            SectionName.Attributes.Append(Number);
        }
    }

    public void AddPEResourcesbyLanguage(IOrderedEnumerable<KeyValuePair<string, int>> Languages)
    {
        XmlNode SectionName; 
        foreach (KeyValuePair<string, int> kvp in Languages)
        {
            if (Convert.ToInt32(kvp.Key) > 0 )
            {
                CultureInfo LCID = new CultureInfo(Convert.ToInt32(kvp.Key), false);
                SectionName = _doc.CreateElement(LCID.Name);
            }
            else SectionName = _doc.CreateElement("Neutral");

            peresourseclanguageNode.AppendChild(SectionName);
            XmlAttribute Number = _doc.CreateAttribute("Number");
            Number.Value = kvp.Value.ToString();
            SectionName.Attributes.Append(Number);
        }
    }

    #endregion

    #region OLE
    public void InitializeOle()
    {
        oleNode = _doc.CreateElement("OLECompoundFileInfo");
        raportNode.AppendChild(oleNode);
        summaryoleNode = _doc.CreateElement("SummaryInformation");
        oleNode.AppendChild(summaryoleNode);
    }

    public void AddSummInfoAtt(uint PropertyIdentifier, string PropertyValue)
    {
        string PropertyName;
        PropertyValue = PropertyValue.Replace("\0", String.Empty);
        switch (PropertyIdentifier){
            case 1:
                PropertyName = "CodePage";
                break;
            case 2:
                PropertyName = "Title";
                break;
            case 3:
                PropertyName = "Subject";
                break;
            case 4:
                PropertyName = "Author";
                break;
            case 5:
                PropertyName = "Keywords";
                break;
            case 6:
                PropertyName = "Comments";
                break;
            case 7:
                PropertyName = "Template";
                break;
            case 8:
                PropertyName = "LastSavedBy";
                break;
            case 9:
                PropertyName = "RevisionNumber";
                break;
            case 10:
                PropertyName = "TotalEditingTime";
                PropertyValue = Convert.ToDateTime(PropertyValue).ToUniversalTime().Subtract(new DateTime(1601,01,01)).ToString();
                break;
            case 11:
                PropertyName = "LastPrinted";
                break;
            case 12:
                PropertyName = "CreateTimeDate";
                break;
            case 13:
                PropertyName = "LastSavedTimeDate";
                break;
            case 14:
                PropertyName = "Pages";
                break;
            case 15:
                PropertyName = "Words";
                break;
            case 16:
                PropertyName = "Characters";
                break;
            case 17:
                PropertyName = "Thumbnail";
                break;
            case 18:
                PropertyName = "ApplicationName";
                break;
            case 19:
                PropertyName = "Security";
                break;
            default:
                PropertyName = "Unknown";
                break;
        }
        XmlAttribute AttName = _doc.CreateAttribute(PropertyName);
        AttName.Value = PropertyValue;
        summaryoleNode.Attributes.Append(AttName);
    }

    #endregion

    #region EXIF methods
    public void AddExIF(Dictionary<string, string> exif)
    {
        foreach (KeyValuePair<string, string> kvp in exif)
        {
            XmlAttribute exifatt = _doc.CreateAttribute(kvp.Key.Replace(" ", string.Empty).Replace("/", string.Empty));
            exifatt.Value = kvp.Value;
            exiftoolNode.Attributes.Append(exifatt);
        }
    }
    #endregion

    public void WritetoConsole()
    {
            _doc.Save(Console.Out);
    }
    public void WritetoFile(string filename)
    {
        _doc.Save(filename);
    }
}
