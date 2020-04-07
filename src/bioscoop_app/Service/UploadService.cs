using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace bioscoop_app.Service
{
    public class UploadService
    {
        private string _base64FileString;
        private string _fileName;


        public UploadService(string base64FileString)
        {
            this._base64FileString = base64FileString;
        }

        public bool CheckIsImage()
        {
            return GetMimeType() == "image/jpeg" || GetMimeType() == "image/png";
        }

        private string GetMimeType()
        {
            string mimeTyperesult = "";
            // regular expression for get mime type from base64 string
            Regex mimeTypeRegex = new Regex("(?<=data:)(.*)(?=;)");

            // get result
            mimeTyperesult = mimeTypeRegex.Match(this._base64FileString).Value;

            return mimeTyperesult;
        }

        private string GetFileExtension()
        {
            string[] splitMimeType = GetMimeType().Split("/").ToArray();

            return splitMimeType[1];
        }

        public void CreateFileInUploadFolder()
        {
            //convert base64
            string convertBase64 = this._base64FileString.Replace($"data:{GetMimeType()};base64,", String.Empty);

            File.WriteAllBytes($"{StorageService.GetUploadPath()}{GetFileName()}",
                Convert.FromBase64String(convertBase64));
        }

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