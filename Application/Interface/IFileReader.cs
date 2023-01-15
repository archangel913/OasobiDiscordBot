using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Application.Interface
{
    public interface IFileReader
    {
        XElement GetXml(string path);
        JObject GetJson(string path);
    }
}
