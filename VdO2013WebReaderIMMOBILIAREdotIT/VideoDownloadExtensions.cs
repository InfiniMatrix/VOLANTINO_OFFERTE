using System;
using System.IO;
using System.Net;
using VdO2013WebMgr;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    public static class VideoDownloadExtensions
    {
        /// <summary>
        /// Downloads a video from the specified URL to the local file system
        /// </summary>
        /// <param name="videoUrl">The URL of the video to download</param>
        /// <param name="fileName">The local file path where the video will be saved</param>
        /// <param name="error">Output parameter containing any error that occurred</param>
        /// <param name="overwrite">Whether to overwrite existing file</param>
        /// <returns>True if download succeeded, false otherwise</returns>
        public static bool DownloadVideo(this string videoUrl, string fileName, out Exception error, bool overwrite = true)
        {
            error = null;
            
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(videoUrl))
                {
                    error = new ArgumentNullException(nameof(videoUrl));
                    return false;
                }
                
                if (string.IsNullOrEmpty(fileName))
                {
                    error = new ArgumentNullException(nameof(fileName));
                    return false;
                }
                
                // Use VideoDownloader to download the file
                using (var downloader = new VideoDownloader(videoUrl))
                {
                    downloader.Download(fileName, out error);
                    return error == null && File.Exists(fileName);
                }
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }
        
        /// <summary>
        /// Determines the file extension for a video based on its URL or content type
        /// </summary>
        /// <param name="videoUrl">The video URL</param>
        /// <returns>The appropriate file extension (e.g., ".mp4", ".webm", ".avi")</returns>
        public static string GetVideoFileExtension(this string videoUrl)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return ".mp4"; // Default to mp4
            
            var url = videoUrl.ToLower();
            
            // Check common video extensions in URL
            if (url.Contains(".mp4"))
                return ".mp4";
            if (url.Contains(".webm"))
                return ".webm";
            if (url.Contains(".avi"))
                return ".avi";
            if (url.Contains(".mov"))
                return ".mov";
            if (url.Contains(".wmv"))
                return ".wmv";
            if (url.Contains(".flv"))
                return ".flv";
            if (url.Contains(".mkv"))
                return ".mkv";
            
            // Default to mp4 for unknown types
            return ".mp4";
        }
    }
}