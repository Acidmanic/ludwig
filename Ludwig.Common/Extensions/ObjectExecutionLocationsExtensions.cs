using System.IO;
using System.Reflection;

namespace Ludwig.Common.Extensions
{
    public static  class ObjectExecutionLocationsExtensions
    {



        public static string ExecutionDirectory(this object o)
        {
            var executionDirectory = new FileInfo(Assembly.GetEntryAssembly()?.Location ?? "").Directory?.FullName ?? "";

            return executionDirectory;
        }


        public static string FilePathInExecutionDirectory(this object o,string name)
        {
            return Path.Combine(ExecutionDirectory(o), name);
        }
    }
}