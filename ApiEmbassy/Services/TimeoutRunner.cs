using System;
using System.Threading;
using System.Threading.Tasks;
using EnTier.Results;
using Microsoft.VisualBasic;

namespace ApiEmbassy.Services
{
    public class TimeoutRunner
    {

        public Task<Result<T>> LoopWithTimeout<T>(Func<Result<T>> loopCode, long timeout)
        {

            return Task.Run(() =>
            {
                long now = Now();

                while (Now()-now< timeout)
                {
                    var loopWorked =  loopCode();

                    if (loopWorked)
                    {
                        return loopWorked;
                    }
                
                    Thread.Sleep(500);
                }

                return new Result<T>().FailAndDefaultValue();
            });
        }
        
        public Task Loop(Action loopCode, Func<bool> keepRunning)
        {

            return Task.Run(() =>
            {
                while (keepRunning())
                {
                    loopCode();
                }
            });
        }


        private long Now()
        {
            DateTime now = DateTime.UtcNow;
            long unixTimeMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();

            return unixTimeMilliseconds;
        }
    }
}