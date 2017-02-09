using System;
using System.Linq;

using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Enums;

public partial class Controls_MessageControl : BaseUserControl
{
    private const String objectIdKey = "$_objectId_$";

    public Guid? ObjectId
    {
        get
        {
            return ViewState[objectIdKey] as Guid?;
        }
        set
        {
            ViewState[objectIdKey] = value;
        }
    }

    public void Show()
    {
        FillMessageSubjects();
        ResetForm();
        mpeMessage.Show();
    }

    public void Update()
    {
        upnlMessage.Update();
    }

    private bool ShowHideDeleteButton(Guid id)
    {
        return id != Guid.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btMessageOK_Click(object sender, EventArgs e)
    {
        //if (String.IsNullOrWhiteSpace(tbxMessage.Text))
        //{
        //    lbMessageError.Text = "შეავსეთ მესიჯის ტექსტი!";
        //    mpeMessage.Show();
        //    upnlMessage.Update();
        //    return;
        //}

        //if (String.IsNullOrWhiteSpace(tbxSubject.Text))
        //{
        //    lbMessageError.Text = "შეავსეთ მესიჯის სათაური!";
        //    mpeMessage.Show();
        //    upnlMessage.Update();
        //    return;
        //}

        //if (ObjectId == null)
        //{
        //    return;
        //}


        //Guid messageId;
        //Guid.TryParse(ddlSubjects.Value.ToString(), out messageId);

        //var message = (from n in DataContext.UM_Messages
        //               where n.DateDeleted == null &&
        //                     n.ID == messageId
        //               select n).FirstOrDefault();

        //if (message == null || messageId == Guid.Empty)
        //{
        //    message = new UM_Message();
        //    message.ID = Guid.NewGuid();
        //    message.ObjectID = ObjectId.Value;
        //    message.DateCreated = DateTime.Now;

        //    DataContext.UM_Messages.InsertOnSubmit(message);
        //}


        //message.Text = tbxMessage.Text;
        //message.Subject = tbxSubject.Text;
        //message.Type = (int)Enum.Parse(typeof(MessageTypeEnum), ddlMessageType.SelectedItem.Value.ToString());
        //message.DateChanged = DateTime.Now;

        //HbSession.SubmitChanges();
    }

    private void ResetForm()
    {
        lbMessageError.Text =
        tbxMessage.Text = tbxSubject.Text = String.Empty;
        ddlMessageType.SelectedIndex = 0;
        ddlSubjects.SelectedIndex = 0;
        btDelete.Enabled = false;

    }

    private void FillMessageSubjects()
    {
        //var messageSubjects = (from n in DataContext.UM_Messages
        //                       where n.DateDeleted == null &&
        //                             n.ObjectID == ObjectId
        //                       select new
        //                       {
        //                           n.ID, 
        //                           n.Subject
        //                       }).ToList();

        //messageSubjects.Insert(0, new { ID = Guid.Empty, Subject = "ახალი" });
        //ddlSubjects.DataSource = messageSubjects;
        //ddlSubjects.DataBind();

    }

    protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {

        //Guid messageId;
        //Guid.TryParse(ddlSubjects.Value.ToString(), out messageId);

        //btDelete.Enabled = ShowHideDeleteButton(messageId);

        //var message = (from n in DataContext.UM_Messages
        //               where n.DateDeleted == null &&
        //                     n.ID == messageId
        //               select n).FirstOrDefault();

        //if (message == null || messageId == Guid.Empty)
        //{
        //    ResetForm();
        //    mpeMessage.Show();
        //    upnlMessage.Update();
        //    return;
        //}

        //tbxMessage.Text = message.Text;
        //tbxSubject.Text = message.Subject;
        //ddlMessageType.SelectedItem = ddlMessageType.Items.FindByValue(Convert.ToString(message.Type));
        //mpeMessage.Show();
        //upnlMessage.Update();

    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        //var message = (from n in DataContext.UM_Messages
        //               where n.DateDeleted == null &&
        //                     n.ID == Guid.Parse(ddlSubjects.Value.ToString())
        //               select n).First();

        //message.DateDeleted = DateTime.Now;
        //HbSession.SubmitChanges();

    }
}