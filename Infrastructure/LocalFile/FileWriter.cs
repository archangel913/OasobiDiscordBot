using Application.Interface;
using System.IO;

namespace Infrastructure.LocalFile
{
    internal class FileWriter : IFileWriter
    {
        private static readonly object LockObject = new object();

        static bool LogInit = false;
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
