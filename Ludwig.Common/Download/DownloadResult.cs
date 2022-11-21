using System;
using System.Net;
using Acidmanic.Utilities.Results;

namespace Ludwig.Common.Download
{
    public class DownloadResult<T> : Result<T, Exception>
    {
        public Exception Exception
        {
            get => Secondary;
            set => Secondary = value;
        }

        public T Value
        {
            get => Primary;
            set => Primary = value;
        }

        public DownloadResult<T> Succeed(T value,WebHeaderCollection responseHeaders)
        {
            return new DownloadResult<T>
            {
                Value = value,
                Success = true,
                Exception = null,
                ResponseHeaders = responseHeaders
            };
        }
        
        public DownloadResult<T> Fail(Exception exception)
        {
            return new DownloadResult<T>
            {
                Value = default,
                Success = false,
                Exception = exception
            };
        }

        public WebHeaderCollection ResponseHeaders { get; set; } = new WebHeaderCollection();

    }
}