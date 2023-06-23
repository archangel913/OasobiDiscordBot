using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Application.Interface;

namespace Infrastructure.LocalFile
{
    internal class FileRepository : IFileRepository
    {
        private static readonly object LockObject = new object();

        static bool LogInit = false;

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

        public void WriteLogFile(string path, string text)
        {
            lock (LockObject)
            {
                if (Directory.Exists("Log") is false)
                {
                    Directory.CreateDirectory("Log");
                }
                if (File.Exists(path) && !LogInit)
                {
                    File.Move(path, "Log/" + File.GetLastWriteTime(path).ToString().Replace("/", "-").Replace(":", "-") + ".txt");
                    File.Delete(path);
                    LogInit = true;
                }
                File.AppendAllText(path, text + "\n");
            }
        }
    }
}
