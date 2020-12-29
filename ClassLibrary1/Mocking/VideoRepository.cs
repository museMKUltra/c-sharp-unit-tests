using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Mocking
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetUnprocessedVideos();
    }

    public class VideoRepository : IVideoRepository
    {
        public IEnumerable<Video> GetUnprocessedVideos()
        {
            using (var context = new VideoContext())
            {
                return (from video in context.Videos
                    where !video.IsProcessed
                    select video).ToList();
            }
        }
    }
}