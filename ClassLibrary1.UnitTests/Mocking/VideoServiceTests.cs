using System.Collections.Generic;
using ClassLibrary1.Mocking;
using Moq;
using NUnit.Framework;

namespace ClassLibrary1.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepository;
        private VideoService _videoService;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _videoRepository.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");
            // var service = new VideoService(new FakeFileReader());

            var result = _videoService.ReadVideoTitle();
            // var result = service.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideoAsCsv_AllVideosAreProcessed_ReturnAnEmptyString()
        {
            _videoRepository.Setup(c => c.GetUnprocessedVideos()).Returns(new List<Video>());

            var videos = _videoService.GetUnprocessedVideoAsCsv();

            Assert.That(videos, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideoAsCsv_AFewUnprocessedVideos_ReturnAStringWithIdOfUnprocessedVideos()
        {
            _videoRepository.Setup(c => c.GetUnprocessedVideos()).Returns(new List<Video>
            {
                new Video {Id = 1},
                new Video {Id = 2},
                new Video {Id = 3}
            });

            var videos = _videoService.GetUnprocessedVideoAsCsv();

            Assert.That(videos, Is.EqualTo("1,2,3"));
        }
    }
}