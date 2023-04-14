using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Application.Interface
{
    public interface IFileRepository
    {
        public XElement GetXml(string path);

        public JObject GetJson(string path);

        public void WriteLogFile(string path, string text);
    }
}
