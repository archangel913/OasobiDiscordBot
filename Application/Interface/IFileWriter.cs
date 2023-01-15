namespace Application.Interface
{
    public interface IFileWriter
    {
        public void WriteLogFile(string fileName, string text);
    }
}
