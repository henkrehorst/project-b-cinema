using Chromely.Core.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bioscoop_app.Helper
{
    class Response
    {
        /// <summary>
        /// The Http Status
        /// </summary>
        public int status;
        /// <summary>
        /// The message associated with the Status
        /// </summary>
        public string statusText;
        /// <summary>
        /// The data in json string.
        /// </summary>
        public string data;

        /// <summary>
        /// Converts current instance to a ChromelyResponse.
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <returns>This as a ChromelyResponse</returns>
        /// <exception cref="InvalidDataException">If the status code is invalid.</exception>
        public ChromelyResponse ChromelyWrapper(string reqId)
        {
            if (status < 100 || status >= 600) throw new InvalidDataException("Invalid status code");
            return new ChromelyResponse(reqId)
            {
                Data = JsonConvert.SerializeObject(this)
            };
        }
    }
}
