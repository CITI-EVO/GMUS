using System.IO;

namespace CITI.EVO.Rpc.Utils
{
    public static class CommonUtil
    {
        public static byte[] ReadToEnd(BinaryReader reader)
        {
            using (var destStream = new MemoryStream())
            {
                int readed;
                var buffer = new byte[8192];

                while ((readed = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, readed);
                }

                return destStream.ToArray();
            }
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            using (var destStream = new MemoryStream())
            {
                int readed;
                var buffer = new byte[8192];

                while ((readed = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, readed);
                }

                return destStream.ToArray();
            }
        }
    }
}
