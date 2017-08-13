using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.LogicStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class LogicModelEntityConverter : SingleModelConverterBase<LogicModel, GM_Logic>
    {
        public LogicModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Logic Convert(LogicModel source)
        {
            var entity = new GM_Logic
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
            };

            FillObject(entity, source);

            return entity;
        }

        public override void FillObject(GM_Logic target, LogicModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type;
            target.SourceType = source.SourceType;
            target.SourceID = source.SourceID;

            var entity = LogicUtil.ConvertToEntity(source);
            if (entity != null)
                target.RawData = XmlUtil.Serialize(entity);
        }
    }
}
