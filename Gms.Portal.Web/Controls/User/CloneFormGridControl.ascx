<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CloneFormGridControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.CloneFormGridControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdSourceGridID" Property="{CloneFormGridModel.SourceGridID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Grid</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxCloneDataGrid" DataTextField="Name" DataValueField="ID" Property="{CloneFormGridModel.TargetGridID=SelectedValue}" CssClass="chosen-select" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Mode</ce:Label>
    <div class="col-lg-10">
        <ce:DropDownList runat="server" ID="cbxMode" Property="{CloneFormGridModel.Mode=SelectedValue}" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Add" Value="Add" />
                <asp:ListItem Text="Merge" Value="Merge" />
                <asp:ListItem Text="Overwrite" Value="Overwrite" />
            </Items>
        </ce:DropDownList>
    </div>
</div>
