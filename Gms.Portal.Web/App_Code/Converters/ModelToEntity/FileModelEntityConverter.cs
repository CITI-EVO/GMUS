using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class FileModelEntityConverter : SingleModelConverterBase<FileModel, GM_File>
    {
        public FileModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_File Convert(FileModel source)
        {
            var target = EntityFactory.CreateEntity<GM_File>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_File target, FileModel source)
        {
            target.ParentID = source.ParentID.GetValueOrDefault();
            target.Name = source.Name;
            target.Description = source.Description;
            target.FileName = source.FileName;
            target.FileData = source.FileData;
        }

    }
}