using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace bioscoop_app.Service
{
    public class UploadService
    {
        private readonly string _base64FileString;


        public UploadService(string base64FileString)
        {
            this._base64FileString = base64FileString;
        }
        
        public bool CheckIsImage()
        {
            return true;
        }

        private string GetMimeType()
        {
            // regular expression for get mime type from base64 string
            Regex getMimeRegex = new Regex(@"/data:([a-zA-Z0-9]+\/[a-zA-Z0-9-.+]+).*,.*/");
            
            var mineType = this._base64FileString;
            return null;
        }
        
        
    }
}