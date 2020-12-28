namespace ClassLibrary1.Mocking
{
    // step3. create an fake class
    // or you can call it MockFileReader/StubFileReader
    public class FakeFileReader: IFileReader
    {
        public string Read(string path)
        {
            return "";
        }
    }
}