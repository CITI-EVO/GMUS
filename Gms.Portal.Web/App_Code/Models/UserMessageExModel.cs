using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class UserMessageExModel : UserMessageModel
    {
        public UserMessageExModel()
        {
        }

        public UserMessageExModel(UserMessageModel model)
        {
            ID = model.ID;
            Subject = model.Subject;
            ParentID = model.ParentID;
            FormID = model.FormID;
            RecordID = model.RecordID;
            FromUserID = model.FromUserID;
            ToUserID = model.ToUserID;
            StatusUserID = model.StatusUserID;
            StatusID = model.StatusID;
            Readed = model.Readed;
            Text = model.Text;
            DateCreated = model.DateCreated;
            DateChanged = model.DateChanged;
            DateDeleted = model.DateDeleted;
        }

        public String Replay { get; set; }
    }
}