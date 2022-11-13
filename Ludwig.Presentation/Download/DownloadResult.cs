using System;
using EnTier.Results;

namespace Ludwig.Presentation.Download
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

        public DownloadResult<T> Succeed(T value)
        {
            return new DownloadResult<T>
            {
                Value = value,
                Success = true,
                Exception = null
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
        
    }
}