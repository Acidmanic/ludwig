using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiEmbassy.Extensions
{
    public static class StreamExtensions
    {




        public static async Task<byte[]> ReadAllBytes(this Stream stream)
        {

            if (stream is { CanRead: true })
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var buffer = new List<byte>();

                int readAsync = 1;
                
                while (readAsync >0)
                {
                    int length = 4096;
                    var bytes = new byte[length];
                    readAsync= await stream.ReadAsync(bytes,0 , length);
                    for (int i = 0; i < readAsync; i++)
                    {
                        buffer.Add(bytes[i]);
                    }
                }

                return buffer.ToArray();
            }

            return new byte[]{};
        }
    }
}