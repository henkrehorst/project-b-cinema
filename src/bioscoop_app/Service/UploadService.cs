using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace bioscoop_app.Service
{
    /// <summary>
    /// Single use service object that stores an image in the image folder.
    /// </summary>
    public class UploadService
    {
        private readonly string _base64FileString;
        private string _fileName;

        /// <summary>
        /// Initializes an uploadservice object for one image.
        /// </summary>
        /// <param name="base64FileString">the image in base 64 string</param>
        public UploadService(string base64FileString)
        {
            this._base64FileString = base64FileString;
        }

        /// <returns>True iff the file is an image.</returns>
        public bool CheckIsImage()
        {
            return GetMimeType() == "image/jpeg" || GetMimeType() == "image/png";
        }

        /// <summary>
        /// Retrieves the mime type from a base64 string.
        /// </summary>
        /// <returns>the mime type</returns>
        private string GetMimeType()
        {
            string mimeTyperesult = "";
            // regular expression for get mime type from base64 string
            Regex mimeTypeRegex = new Regex("(?<=data:)(.*)(?=;)");

            // get result
            mimeTyperesult = mimeTypeRegex.Match(this._base64FileString).Value;

            return mimeTyperesult;
        }

        /// <returns>The file extension of the data provided</returns>
        private string GetFileExtension()
        {
            string[] splitMimeType = GetMimeType().Split("/").ToArray();

            return splitMimeType[1];
        }

        /// <summary>
        /// Creates the file in the upload folder and writes all data.
        /// </summary>
        public void CreateFileInUploadFolder()
        {
            //convert base64
            string convertBase64 = this._base64FileString.Replace($"data:{GetMimeType()};base64,", String.Empty);

            File.WriteAllBytes($"{StorageService.GetUploadPath()}{GetFileName()}",
                Convert.FromBase64String(convertBase64));
        }

        /// <summary>
        /// Finds the file in the upload folder and deletes it.
        /// </summary>
        /// <param name="filename"></param>
        public void DeleteFile(string filename)
        {
            //delete file if file exists
            if (File.Exists($"{StorageService.GetUploadPath()}{filename}"))
            {
                File.Delete($"{StorageService.GetUploadPath()}{filename}");
            }
        }

        /// <summary>
        /// Get method for the filename. Initialized if null or empty string.
        /// </summary>
        /// <returns>the filename</returns>
        public string GetFileName()
        {
            if (String.IsNullOrEmpty(this._fileName))
            {
                this._fileName = $"{Guid.NewGuid().ToString()}.{GetFileExtension()}";
            }

            return this._fileName;
        }
    }
}