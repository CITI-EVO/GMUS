using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.EventStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class EventEntityModelConverter : SingleModelConverterBase<GM_Event, EventModel>
    {
        public EventEntityModelConverter(ISession session) : base(session)
        {
        }

        public override EventModel Convert(GM_Event source)
        {
            var target = new EventModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(EventModel target, GM_Event source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Visible = source.Visible;

            target.Entity = XmlUtil.Deserialize<EventEntity>(source.XmlData);
        }
    }
}