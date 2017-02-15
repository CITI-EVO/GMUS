using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class TableEntityModelConverter : SingleModelConverterBase<GM_Table, TableModel>
    {
        public TableEntityModelConverter(ISession session) : base(session)
        {
        }

        public override TableModel Convert(GM_Table source)
        {
            var target = new TableModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(TableModel target, GM_Table source)
        {
            target.ID = source.ID;
            target.Name = source.Name.Trim();
            target.Status = source.Status;
            target.Columns = new List<ColumnModel>();

            var columns = source.Columns.Where(n => n.DateDeleted == null);
            FillColumns(target.Columns, columns);
        }

        public void FillColumns(List<ColumnModel> models, IEnumerable<GM_Column> entities)
        {
            foreach (var entity in entities.OrderBy(n => n.DateCreated))
            {
                var model = new ColumnModel();
                FillColumn(model, entity);

                models.Add(model);
            }
        }
        public void FillColumn(ColumnModel model, GM_Column entity)
        {
            model.ID = entity.ID;
            model.Name = entity.Name.Trim();
            model.Type = entity.Type;
            model.TableID = entity.TableID;
        }
    }
}