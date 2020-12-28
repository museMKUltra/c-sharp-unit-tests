using Newtonsoft.Json;

namespace ClassLibrary1.Mocking
{
    public class VideoService
    {
        private IFileReader _fileReader;

        // constructor injection
        public VideoService(IFileReader fileReader = null)
        {
            _fileReader = fileReader ?? new FileReader();
        }

        public string ReadVideoTitle()
        {
            // step1. isolate an class
            var str = _fileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";

            return video.Title;
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }
}