using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class CollectionModelEntityConverter : SingleModelConverterBase<CollectionModel, GM_Collection>
    {
        public CollectionModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Collection Convert(CollectionModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Collection>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Collection target, CollectionModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.XmlData = XmlUtil.Serialize(source.Entity);
        }

    }
}