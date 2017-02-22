using System.Xml.Linq;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Utils
{
    public static class XmlUtil
    {
        public static XDocument Serialize<TEntity>(TEntity entity)
        {
            if (entity == null)
                return null;

            var xDoc = new XDocument();

            using (var xmlWriter = xDoc.CreateWriter())
            {
                var serializer = new XmlSerializer(typeof(TEntity));
                serializer.Serialize(xmlWriter, entity);

                return xDoc;
            }
        }

        public static TEntity Deserialize<TEntity>(XDocument xDoc)
        {
            if (xDoc == null)
                return default(TEntity);

            using (var xmlReader = xDoc.CreateReader())
            {
                var serializer = new XmlSerializer(typeof(TEntity));

                var entity = (TEntity)serializer.Deserialize(xmlReader);
                return entity;
            }
        }


    }
}