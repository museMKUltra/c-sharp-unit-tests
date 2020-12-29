using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClassLibrary1.Mocking
{
    public class VideoService
    {
        private IFileReader _fileReader;
        private IVideoRepository _videoRepository;

        // constructor injection
        public VideoService(IFileReader fileReader = null, IVideoRepository videoRepository = null)
        {
            _fileReader = fileReader ?? new FileReader();
            _videoRepository = videoRepository ?? new VideoRepository();
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

        public string GetUnprocessedVideoAsCsv()
        {
            var videoIds = new List<int>();

            var videos = _videoRepository.GetUnprocessedVideos();
            foreach (var v in videos) videoIds.Add(v.Id);

            return String.Join(",", videoIds);
        }
    }

    public class VideoContext : IDisposable
    {
        public List<Video> Videos { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }
}