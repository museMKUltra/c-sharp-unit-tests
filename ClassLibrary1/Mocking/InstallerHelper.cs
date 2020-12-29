using System;
using System.Net;

namespace ClassLibrary1.Mocking
{
    public class InstallerHelper
    {
        private readonly IFileDownloader _fileDownloader;
        private string _setupDestinationFile;

        public InstallerHelper(IFileDownloader fileDownloader = null)
        {
            _fileDownloader = fileDownloader ?? new FileDownloader();
            // if you use DI framework, then don't need to use poor man's DI approach
            // _fileDownloader = fileDownloader;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                _fileDownloader.DownloadFile(string.Format("http://example.com/{0}/{1}", customerName, installerName),
                    _setupDestinationFile);
                return true;
            }
            catch (WebException)
            {
                // WebException is the what we concern, others no need to be hidden to return false
                return false;
            }
        }
    }
}