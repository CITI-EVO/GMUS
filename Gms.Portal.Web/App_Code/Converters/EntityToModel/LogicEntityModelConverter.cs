using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.LogicStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class LogicEntityModelConverter : SingleModelConverterBase<GM_Logic, LogicModel>
    {
        public LogicEntityModelConverter(ISession session) : base(session)
        {
        }

        public override LogicModel Convert(GM_Logic source)
        {
            var target = new LogicModel();

            FillObject(target, source);

            return target;
        }

        public override void FillObject(LogicModel target, GM_Logic source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Type = source.Type;
            target.SourceType = source.SourceType;
            target.SourceID = source.SourceID;

            var logicXElem = source.RawData;
            if (logicXElem == null)
                return;

            var entity = XmlUtil.Deserialize<LogicEntity>(source.RawData);
            var model = LogicUtil.ConvertToModel(entity);

            target.Query = model.Query;
            target.ExpressionsLogic = model.ExpressionsLogic;
        }
    }
}
