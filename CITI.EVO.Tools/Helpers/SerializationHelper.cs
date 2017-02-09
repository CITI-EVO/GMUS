using System.Xml.Linq;
using System.Xml.Serialization;

namespace CITI.EVO.Tools.Helpers
{
	public static class SerializationHelper
	{
		public static XElement Serialize<TItem>(TItem config)
		{
			var xDoc = new XDocument();

			using (var xmlWriter = xDoc.CreateWriter())
			{
				var xmlSer = new XmlSerializer(typeof(TItem));
				xmlSer.Serialize(xmlWriter, config);
			}

			return xDoc.Root;
		}

		public static TItem Deserialize<TItem>(XElement xElement)
		{
			var xDoc = new XDocument(xElement);

			using (var xmlReader = xDoc.CreateReader())
			{
				var xmlSer = new XmlSerializer(typeof(TItem));
				return (TItem)xmlSer.Deserialize(xmlReader);
			}
		}
	}

}
