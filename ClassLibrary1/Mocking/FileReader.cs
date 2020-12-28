using System.IO;

namespace ClassLibrary1.Mocking
{
    // step2. extract an interface
    public interface IFileReader
    {
        string Read(string path);
    }

    public class FileReader : IFileReader
    {
        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}