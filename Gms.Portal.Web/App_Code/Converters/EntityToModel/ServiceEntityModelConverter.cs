using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.ServiceStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class ServiceEntityModelConverter : SingleModelConverterBase<GM_Service, ServiceModel>
    {
        public ServiceEntityModelConverter(ISession session) : base(session)
        {
        }

        public override ServiceModel Convert(GM_Service source)
        {
            var target = new ServiceModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(ServiceModel target, GM_Service source)
        {
            target.ID = source.ID;
            target.Url = source.Url;
            target.Name = source.Name;

            target.Entity = XmlUtil.Deserialize<ServiceEntity>(source.XmlData);
        }
    }
}