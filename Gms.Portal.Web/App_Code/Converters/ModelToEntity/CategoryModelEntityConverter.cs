using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class CategoryModelEntityConverter : SingleModelConverterBase<CategoryModel, GM_Category>
    {
        public CategoryModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Category Convert(CategoryModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Category>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Category target, CategoryModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.ParentID = source.ParentID;
            target.Visible = source.Visible;
            target.OrderIndex = source.OrderIndex;
        }
    }
}