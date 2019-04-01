using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sonic06Randomiser
{
    class SetClass
    {
        static public void Extract(bool extractFolder, int outputFolderType, string output, string filepath)
        {
            string setName = "";

            if (outputFolderType == 0 && output == "") { outputFolderType = 1; } //Default to saving in the program directory if the user doesn't specify a path with Output Folder Type set to custom.

            if (!extractFolder)
            {
                var setData = new HedgeLib.Sets.S06SetData();
                setName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                setName = setName.Remove(setName.Length - 4);
                setName = setName.Replace("\\", "");
                setData.Load(filepath);
                switch (outputFolderType)
                {
                    case 0: //Custom
                        setData.ExportXML(output + "\\" + setName + ".xml");
                        break;
                    case 1: //Source
                        setData.ExportXML(filepath.Remove(filepath.Length - 4) + ".xml");
                        break;
                    case 2: //Program
                        setData.ExportXML(setName + ".xml");
                        break;
                }
            }
            else
            {
                string[] sets = Directory.GetFiles(filepath, "*.set", SearchOption.AllDirectories);
                Console.WriteLine("Found " + sets.Length + " xml files");
                foreach (string set in sets)
                {
                    var setData = new HedgeLib.Sets.S06SetData();
                    //Load XML
                    setName = set.Remove(0, Path.GetDirectoryName(set).Length);
                    setName = setName.Remove(setName.Length - 4);
                    setName = setName.Replace("\\", "");
                    Console.WriteLine(Path.GetDirectoryName(set) + "\\" + setName + ".set");
                    setData.Load(set);
                    switch (outputFolderType)
                    {
                        case 0: //Custom
                            setData.ExportXML(output + "\\" + setName + ".xml");
                            break;
                        case 1: //Source
                            setData.ExportXML(Path.GetDirectoryName(set) + "\\" + setName + ".xml");
                            break;
                        case 2: //Program
                            setData.ExportXML(setName + ".xml");
                            break;
                    }
                }
            }
        }

        static public void Import(bool importFolder, int outputFolderType, string output, string filepath)
        {
            string xmlName = "";

            if (outputFolderType == 0 && output == "") { outputFolderType = 1; } //Default to saving in the program directory if the user doesn't specify a path with Output Folder Type set to custom.

            if (!importFolder)
            {
                var setData = new HedgeLib.Sets.S06SetData();
                xmlName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                xmlName = xmlName.Remove(xmlName.Length - 4);
                xmlName = xmlName.Replace("\\", "");
                switch (outputFolderType)
                {
                    case 0: //Custom
                        setData.ImportXML(output + "\\" + xmlName + ".xml");
                        File.Delete(output + "\\" + xmlName + ".set");
                        setData.Save(output + "\\" + xmlName + ".set");
                        break;
                    case 1: //Source
                        setData.ImportXML(filepath.Remove(filepath.Length - 4) + ".xml");
                        File.Delete(filepath.Remove(filepath.Length - 4) + ".set");
                        setData.Save(filepath.Remove(filepath.Length - 4) + ".set");
                        break;
                    case 2: //Program
                        setData.ImportXML(xmlName + ".xml");
                        File.Delete(xmlName + ".set");
                        setData.Save(xmlName + ".set");
                        break;
                }
            }
            else
            {
                string[] xmls = Directory.GetFiles(filepath, "*.xml", SearchOption.AllDirectories);
                foreach (string xml in xmls)
                {
                    var setData = new HedgeLib.Sets.S06SetData();
                    //Save XML
                    xmlName = xml.Remove(0, Path.GetDirectoryName(xml).Length);
                    xmlName = xmlName.Remove(xmlName.Length - 4);
                    xmlName = xmlName.Replace("\\", "");
                    setData.ImportXML(xml);
                    Console.WriteLine(Path.GetDirectoryName(xml) + "\\" + xmlName + ".xml");
                    switch (outputFolderType)
                    {
                        case 0: //Custom
                            File.Delete(output + "\\" + xmlName + ".set");
                            setData.Save(output + "\\" + xmlName + ".set");
                            break;
                        case 1: //Source
                            File.Delete(Path.GetDirectoryName(xml) + "\\" + xmlName + ".set");
                            setData.Save(Path.GetDirectoryName(xml) + "\\" + xmlName + ".set");
                            break;
                        case 2: //Program
                            File.Delete(xmlName + ".set");
                            setData.Save(xmlName + ".set");
                            break;
                    }
                }
            }
        }
    }
}
