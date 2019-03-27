using System.IO;

namespace Sonic06Randomiser
{
    class SetExtract
    {
        static public void Extract(string filepath, string output, bool sourceOutput)
        {
            var setData = new HedgeLib.Sets.S06SetData();
            string xmlName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
            xmlName = xmlName.Remove(xmlName.Length - 4);
            xmlName = xmlName.Replace("\\", "");
            setData.Load(filepath);
            if (output != "")
            {
                setData.ExportXML(output + "\\" + xmlName + ".xml");
            }
            else
            {
                if (!sourceOutput)
                {
                    setData.ExportXML(xmlName + ".xml");
                }
                else
                {
                    setData.ExportXML(filepath.Remove(filepath.Length - 4) + ".xml");
                }
            }
        }
    }
}
