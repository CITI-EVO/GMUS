using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using CITI.EVO.Tools.Comparers;
using QuickLZ.Managed;
using QuickLZ.PInvoke;

namespace CITI.EVO.Tools.Extensions
{
	public static class ByteArrayExtensions
	{
		private static readonly IComparer<byte[]> bytesComparer = new ByteArrayComparer();

		private static readonly QuickLZNative quickLZNative = new QuickLZNative();
		private static readonly QuickLZLimited quickLZLimited = new QuickLZLimited();

		public static byte[] CompressDef(this byte[] bytes)
		{
			using (var srcStream = new MemoryStream(bytes))
			{
				using (var destStream = new MemoryStream())
				{
					using (var defStream = new DeflateStream(destStream, CompressionMode.Compress))
					{
						int readed = 0;
						var buffer = new byte[8192];

						while ((readed = srcStream.Read(buffer, 0, buffer.Length)) > 0)
						{
							defStream.Write(buffer, 0, readed);
						}
					}

					return destStream.ToArray();
				}
			}
		}

		public static byte[] DecompressDef(this byte[] bytes)
		{
			using (var srcStream = new MemoryStream(bytes))
			{
				using (var defStream = new DeflateStream(srcStream, CompressionMode.Decompress))
				{
					using (var destStream = new MemoryStream())
					{
						int readed = 0;
						var buffer = new byte[8192];

						while ((readed = defStream.Read(buffer, 0, buffer.Length)) > 0)
						{
							destStream.Write(buffer, 0, readed);
						}

						return destStream.ToArray();
					}
				}
			}
		}

		public static byte[] CompressLZ(this byte[] bytes)
		{
			return CompressLZ(bytes, true);
		}
		public static byte[] CompressLZ(this byte[] bytes, bool native)
		{
			if (bytes == null)
			{
				return null;
			}

			if (native)
			{
				//var quickLZNative = new QuickLZNative();
				return quickLZNative.Compress(bytes);
			}
			else
			{
				//var quickLZLimited = new QuickLZLimited();
				return quickLZLimited.Compress(bytes);
			}
		}

		public static byte[] DecompressLZ(this byte[] bytes)
		{
			return DecompressLZ(bytes, true);
		}
		public static byte[] DecompressLZ(this byte[] bytes, bool native)
		{
			if (bytes == null)
			{
				return null;
			}

			if (native)
			{
				//var quickLZNative = new QuickLZNative();
				return quickLZNative.Decompress(bytes);
			}
			else
			{
				//var quickLZLimited = new QuickLZLimited();
				return quickLZLimited.Decompress(bytes);
			}
		}

		public static bool ArrayEquals(this byte[] xArray, byte[] yArray)
		{
			return bytesComparer.Compare(xArray, yArray) == 0;
		}
	}
}
