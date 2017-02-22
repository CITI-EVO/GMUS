using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class CollectionEntityModelConverter : SingleModelConverterBase<GM_Collection, CollectionModel>
    {
        public CollectionEntityModelConverter(ISession session) : base(session)
        {
        }

        public override CollectionModel Convert(GM_Collection source)
        {
            var target = new CollectionModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(CollectionModel target, GM_Collection source)
        {
            target.ID = source.ID;
            target.Name = source.Name;

            target.Entity = XmlUtil.Deserialize<CollectionEntity>(source.XmlData);
        }
    }
}