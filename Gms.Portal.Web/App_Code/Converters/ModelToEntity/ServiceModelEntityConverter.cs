using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class ServiceModelEntityConverter : SingleModelConverterBase<ServiceModel, GM_Service>
    {
        public ServiceModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Service Convert(ServiceModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Service>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Service target, ServiceModel source)
        {
            //target.ID = source.ID;
            target.Url = source.Url;
            target.Name = source.Name;

            target.XmlData = XmlUtil.Serialize(source.Entity);
        }
    }
}