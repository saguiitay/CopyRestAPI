using System;
using CopyRestAPI.Models;
using Newtonsoft.Json;

namespace CopyRestAPI.Helpers
{
    public class ServerException : Exception
    {
        public ServerException()
        { }

        public ServerException(string message)
            : base(message)
        { }

        public ServerException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ServerException(int statusCode, string content)
        {
            StatusCode = statusCode;

            var error = JsonConvert.DeserializeObject<ErrorResponse>(content);

            ErrorCode = error.Error;
            Message = error.Message;
        }

        public override string ToString()
        {
            var s = string.Format("HttpStatusCode: {0}", StatusCode);
            s += Environment.NewLine + string.Format("ErrorCode: {0}", ErrorCode);
            s += Environment.NewLine + string.Format("Message: {0}", Message);
            s += Environment.NewLine + base.ToString();

            return s;
        }

        public int StatusCode { get; set; }

        public int ErrorCode { get; set; }

        public new string Message { get; set; }
    }
}
