using System;
using System.IO;
using System.Net;
using CITI.EVO.Rpc.Common;

namespace CITI.EVO.Rpc.Transporters
{
	public class HttpDataTransporter : IDataTranporter
	{
		private readonly HttpWebRequest _request;

		public HttpDataTransporter(Uri url, int timeout)
		{
			Url = url;
			Timeout = timeout;

			_request = (HttpWebRequest)WebRequest.Create(Url);
			_request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			_request.AllowAutoRedirect = true;
			_request.Timeout = Timeout;
		}

		public Uri Url { get; private set; }

		public int Timeout { get; private set; }

		public byte[] Send(byte[] bytes)
		{
			if (bytes != null)
			{
				_request.Method = "POST";

				var inputStream = _request.GetRequestStream();
				inputStream.Write(bytes, 0, bytes.Length);
			}

			var result = GetResponseBytes(_request);
			return result;
		}

		private byte[] GetResponseBytes(WebRequest request)
		{
			using (var response = request.GetResponse())
			{
				using (var outputStream = response.GetResponseStream())
				{
					if (outputStream == null)
						return null;

					using (var memoryStream = new MemoryStream())
					{
						int readed;
						var buffer = new byte[4096];

						while ((readed = outputStream.Read(buffer, 0, buffer.Length)) > 0)
							memoryStream.Write(buffer, 0, readed);

						return memoryStream.ToArray();
					}
				}
			}
		}

		public void Dispose()
		{
		}
	}
}
