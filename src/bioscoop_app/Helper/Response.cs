using Chromely.Core.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public ChromelyResponse ChromelyWrapper(string reqId)
        {
            return new ChromelyResponse(reqId)
            {
                Data = JsonConvert.SerializeObject(this)
            };
        }
    }
}
