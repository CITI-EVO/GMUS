using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class FileEntityModelConverter : SingleModelConverterBase<GM_File, FileModel>
    {
        public FileEntityModelConverter(ISession session) : base(session)
        {
        }

        public override FileModel Convert(GM_File source)
        {
            var target = new FileModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(FileModel target, GM_File source)
        {
            target.ID = source.ID;
            target.ParentID = source.ParentID;
            target.Name = source.Name;
            target.Description = source.Description;
            target.FileData = source.FileData;
            target.FileName = source.FileName;
        }
    }
}