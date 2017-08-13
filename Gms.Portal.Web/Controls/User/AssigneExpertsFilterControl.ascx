<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AssigneExpertsFilterControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.AssigneExpertsFilterControl" %>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Recipient group</ce:Label>
    <div class="col-lg-3">
        <ce:DropDownList runat="server" AppendDataBoundItems="True" ID="ddlRecipientGroups" Property="{AssigneExpertsFilterModel.GroupID=SelectedValue}" CssClass="chosen-select" DataTextField="Name" DataValueField="ID">
            <Items>
                <asp:ListItem Text="Select an Option" Value="" />
            </Items>
        </ce:DropDownList>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Assigned experts</ce:Label>
    <div class="col-lg-3">
        <div class="i-checks">
            <ce:CheckBox runat="server" ID="cbxAssigneds" Property="{AssigneExpertsFilterModel.Assigneds=Checked}">
            </ce:CheckBox>
        </div>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Show all users</ce:Label>
    <div class="col-lg-3">
        <div class="i-checks">
            <ce:CheckBox runat="server" ID="chkAllUsers" Property="{AssigneExpertsFilterModel.AllUsers=Checked}">
            </ce:CheckBox>
        </div>
    </div>
</div>