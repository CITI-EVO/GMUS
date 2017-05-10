using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class EventModelEntityConverter : SingleModelConverterBase<EventModel, GM_Event>
    {
        public EventModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Event Convert(EventModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Event>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Event target, EventModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.Visible = source.Visible;

            target.XmlData = XmlUtil.Serialize(source.Entity);
        }
    }
}