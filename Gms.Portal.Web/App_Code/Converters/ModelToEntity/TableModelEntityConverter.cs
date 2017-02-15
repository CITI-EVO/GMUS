using System;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class TableModelEntityConverter : SingleModelConverterBase<TableModel, GM_Table>
    {
        public TableModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Table Convert(TableModel source)
        {
            var entity = new GM_Table
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };

            FillObject(entity, source);

            return entity;
        }

        public override void FillObject(GM_Table target, TableModel source)
        {
            target.Name = source.Name.Trim();
            target.Status = source.Status;
        }
    }
}