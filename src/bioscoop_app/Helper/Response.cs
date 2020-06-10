using Chromely.Core.Network;
using FluentValidation;
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
        /// Lambda function to generate a ChromelyResponse when the JSON input could not be parsed.
        /// </summary>
        public static Func<string, ChromelyResponse> ParseError = id =>
        {
            return new Response
            {
                status = 400,
                statusText = "Failed to parse request"
            }.ChromelyWrapper(id);
        };
        /// <summary>
        /// Lambda function to generate a ChromelyResponse for when the validation on the sent data has failed.
        /// </summary>
        public static Func<string, ValidationException, ChromelyResponse> ValidationError = (id, exception) =>
        {
            return new Response
            {
                status = 400,
                statusText = "Bad input",
                data = JsonConvert.SerializeObject(exception.Errors)
            }.ChromelyWrapper(id);
        };
        /// <summary>
        /// Lambda function to generate a ChromelyResponse for when a closed Repository is used.
        /// </summary>
        public static Func<string, ChromelyResponse> TransactionProtocolViolation = id =>
        {
            return new Response
            {
                status = 500,
                statusText = "Transaction Protocol Violation: Unsafe data update detected."
            }.ChromelyWrapper(id);
        };
        public static Func<string, string, ChromelyResponse> IllegalUpdate = (id, msg) =>
        {
            return new Response
            {
                status = 400,
                statusText = msg
            }.ChromelyWrapper(id);
        };

        /// <summary>
        /// Converts current instance to a ChromelyResponse.
        /// </summary>
        /// <param name="reqId">The request Id</param>
        /// <returns>This as a ChromelyResponse</returns>
        /// <exception cref="InvalidDataException">If the status code is invalid, or if data is send with status code 204.</exception>
        public ChromelyResponse ChromelyWrapper(string reqId)
        {
            if (status < 100 || status >= 600) throw new InvalidDataException("Invalid status code");
            if (status == 204 && data is object) throw new InvalidDataException("Response with code 204 can't have data");
            return new ChromelyResponse(reqId)
            {
                Data = JsonConvert.SerializeObject(this)
            };
        }
    }
}
