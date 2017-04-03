using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class CategoryEntityModelConverter : SingleModelConverterBase<GM_Category, CategoryModel>
    {
        public CategoryEntityModelConverter(ISession session) : base(session)
        {
        }

        public override CategoryModel Convert(GM_Category source)
        {
            var target = new CategoryModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(CategoryModel target, GM_Category source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Visible = source.Visible;
            target.OrderIndex = source.OrderIndex;
        }
    }
}