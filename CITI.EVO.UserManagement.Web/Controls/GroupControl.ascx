<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.GroupControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{GroupModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{GroupModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdProjectID" Property="{GroupModel.ProjectID=Value}" />


  <div class="form-group">
            <label class="col-sm-4 control-label">ჯგუფის სახელი</label>
            <div class="col-sm-6">
        <asp:TextBox ID="tbxName" runat="server"  class="form-control"  Property="{GroupModel.Name=Text}"></asp:TextBox>

            </div>
        </div>

