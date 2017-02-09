using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using CITI.EVO.Rpc.Utils;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Utils;

namespace CITI.EVO.Rpc.Processing
{
    public abstract class RequestProcessorBase
    {
        private readonly Encoding encoding = Encoding.UTF8;

        public virtual void ProcessRequest(HttpRequest request, HttpResponse response)
        {
            ProcessRequest(request.InputStream, response.OutputStream);
        }

        public virtual void ProcessRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            ProcessRequest(request.InputStream, response.OutputStream);
        }

        public virtual void ProcessRequest(Stream inputStream, Stream ouputStream)
        {
            var reader = new BinaryReader(inputStream);
            var writer = new BinaryWriter(ouputStream);

            ProcessRequest(reader, writer);
        }

        public virtual void ProcessRequest(BinaryReader reader, BinaryWriter writer)
        {
            var fullData = CommonUtil.ReadToEnd(reader);

            var peer = GetPeerName(fullData);
            var responseRawData = ProcessRequest(peer, fullData);

            writer.Write(responseRawData);
            writer.Flush();
        }

        public virtual byte[] ProcessRequest(byte[] bytes)
        {
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream(4096))
                {
                    ProcessRequest(inputStream, outputStream);

                    return outputStream.ToArray();
                }
            }
        }

        protected abstract byte[] ProcessRequest(String peer, byte[] bytes);

        protected String GetPeerName(byte[] bytes)
        {
            var len = BitConverter.ToInt32(bytes, 0);

            var dataBytes = new byte[len];
            Buffer.BlockCopy(bytes, sizeof(int), dataBytes, 0, len);

            return encoding.GetString(dataBytes);
        }

        protected RequestEntity GetRequestEntity(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var reader = new BinaryReader(stream);

                var headerLen = reader.ReadInt32();
                stream.Seek(headerLen, SeekOrigin.Current);

                var compression = reader.ReadByte();

                var dataLen = reader.ReadInt32();
                var dataBytes = reader.ReadBytes(dataLen);

                switch (compression)
                {
                    case 1:
                        dataBytes = CompressionUtil.DecompressDef(dataBytes);
                        break;
                    case 2:
                        dataBytes = CompressionUtil.DecompressLZ(dataBytes, false);
                        break;
                }

                var entity = SerializationUtil.Deserialize<RequestEntity>(dataBytes);
                return entity;
            }
        }

        protected byte[] GetResponseBytes(ResponseEntity responseEntity)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);

                var resultBytes = TrySerializeResponse(responseEntity);

                if (ConfigUtil.ConfigSection != null && ConfigUtil.ConfigSection.Client != null)
                {
                    switch (ConfigUtil.ConfigSection.Client.Compression.ToLower())
                    {
                        case "def":
                            {
                                writer.Write((byte)1);
                                resultBytes = CompressionUtil.CompressDef(resultBytes);
                            }
                            break;
                        case "lz":
                            {
                                writer.Write((byte)2);
                                resultBytes = CompressionUtil.CompressLZ(resultBytes, false);
                            }
                            break;
                        default:
                            writer.Write((byte)0);
                            break;
                    }
                }

                writer.Write(resultBytes.Length);
                writer.Write(resultBytes);

                writer.Flush();

                return stream.ToArray();
            }
        }

        protected byte[] TrySerializeResponse(ResponseEntity responseEntity)
        {
            try
            {
                return SerializationUtil.Serialize(responseEntity);
            }
            catch (Exception ex)
            {
                responseEntity.ErrorCode = 1;
                responseEntity.ErrorMessage = ex.ToString();
                responseEntity.ResultObject = null;

                return SerializationUtil.Serialize(responseEntity);
            }
        }
    }
}
