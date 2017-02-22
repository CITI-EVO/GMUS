using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class FormModelEntityConverter : SingleModelConverterBase<FormModel, GM_Form>
    {
        public FormModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Form Convert(FormModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Form>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Form target, FormModel source)
        {
            //target.ID = source.ID;
            target.Name = source.Name;
            target.Number = source.Number;
            target.Visible = source.Visible;
            target.OrderIndex = source.OrderIndex;
            target.XmlData = XmlUtil.Serialize(source.Entity);
        }

    }
}