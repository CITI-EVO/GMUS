using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Web.Entities.GovTalk.Common;

namespace CITI.EVO.CommonData.Web.Helpers
{
	public static class GovTalkHelper
	{
		static GovTalkHelper()
		{
			ServicePointManager.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
		}

		public static GovTalkMessageEntity CreateBaseRequest(String serviceName, BaseMessageEntity message)
		{
			var auth = new AuthenticationEntity
			{
				Method = "clear",
				Value = ConfigurationManager.AppSettings["GovTalkPassword"]
			};

			var idAuth = new IDAuthenticationEntity
			{
				SenderID = ConfigurationManager.AppSettings["GovTalkUserName"],
				Authentication = auth,
			};

			var senderDetails = new SenderDetailsEntity
			{
				EmailAddress = "nomail",
				IDAuthentication = idAuth,
			};

			var messageDetails = new MessageDetailsEntity
			{
				Class = serviceName,
				Function = "submit",
				Qualifier = "request",
				CorrelationID = "cor",
			};

			var header = new HeaderEntity
			{
				SenderDetails = senderDetails,
				MessageDetails = messageDetails,
			};

			var body = new BodyEntity
			{
				Entity = message,
			};

			var error = new ErrorEntity
			{
				EnvelopeVersion = "369.25",
				Number = 2695,
				RaisedBy = "me",
				Text = "Exception",
				Type = "TypedError"
			};

			var govTalkMessage = new GovTalkMessageEntity
			{
				EnvelopeVersion = 2.0M,
				Header = header,
				Body = body,
			};

			return govTalkMessage;
		}

		public static GovTalkMessageEntity SendRequest(GovTalkMessageEntity requestEntity)
		{
			var serviceUrl = GetServiceUrl();

			if (String.IsNullOrWhiteSpace(serviceUrl))
				throw new Exception();

			using (var client = new WebClient())
			{
				var requestBytes = Serialize(requestEntity);
				LogXml("Req", requestBytes);

				var responseBytes = client.UploadData(serviceUrl, requestBytes);
				LogXml("Res", responseBytes);

				var responseEntity = Deserialize(responseBytes);
				return responseEntity;
			}
		}

		public static byte[] Serialize(GovTalkMessageEntity entity)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new StreamWriter(stream, Encoding.UTF8))
				{
					var ns = new XmlSerializerNamespaces();
					ns.Add("", "http://www.govtalk.gov.uk/CM/envelope");

					var xmlSer = new XmlSerializer(typeof(GovTalkMessageEntity));
					xmlSer.Serialize(writer, entity, ns);

					return stream.ToArray();
				}
			}
		}

		public static GovTalkMessageEntity Deserialize(byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					var xmlSer = new XmlSerializer(typeof(GovTalkMessageEntity));
					return (GovTalkMessageEntity)xmlSer.Deserialize(reader);
				}
			}
		}

		private static String GetServiceUrl()
		{
			return ConfigurationManager.AppSettings["GovTalkServiceUrl"];
		}

		private static void LogXml(String suffix, byte[] xmlBytes)
		{
			var currDate = DateTime.Now;
			var fileName = $"{currDate:yyyy-MM-dd_hh.mm.ss}_{suffix}.xml";

			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GovTalkLogs", fileName);
			File.WriteAllBytes(filePath, xmlBytes);
		}

		private static bool ServerCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}