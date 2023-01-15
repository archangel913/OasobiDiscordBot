using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Application.Interface;

namespace Infrastructure.LocalFile
{
    internal class FileReader : IFileReader
    {
        public XElement GetXml(string path)
        {
            if (File.Exists(path) is false)
            {
                throw new FileNotFoundException($"There is no file : {path} ");
            }
            return XElement.Load(path);
        }

        public JObject GetJson(string path)
        {
            if (File.Exists(path) is false)
            {
                throw new FileNotFoundException($"There is no file : {path} ");
            }
            string jsonData;
            using (var sr = new StreamReader(path))
            {
                jsonData = sr.ReadToEnd();
            }
            return JObject.Parse(jsonData);
        }
    }
}
