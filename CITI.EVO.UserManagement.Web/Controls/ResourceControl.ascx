<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourceControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ResourceControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ResourceModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ResourceModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdProjectID" Property="{ResourceModel.ProjectID=Value}" />

<div class="form-group col-sm12">
    <label class="col-sm-6  font-normal">რესურსის სახელი</label>
    <div class="col-sm-6">
        <asp:TextBox ID="tbResourceName" CssClass="form-control" runat="server" Property="{ResourceModel.Name=Text}"></asp:TextBox>

    </div>
</div>
<div class="form-group col-sm12">
    <label class="col-sm-6  font-normal">რესურსის ტიპი</label>
    <div class="col-sm-6">

        <ce:ASPxComboBox ID="cmbResourceType" TextField="Key" ValueField="Value" runat="server" CssClass="chosen-select form-control" ValueType="System.Int32" Property="{ResourceModel.Type=Value}">
        </ce:ASPxComboBox>
    
    </div>
</div>
<div class="form-group col-sm12">
    <label class="col-sm-6  font-normal">მნიშვნელობა</label>
    <div class="col-sm-6">
            <asp:TextBox ID="tbResourceValue" runat="server" CssClass="form-control" Property="{ResourceModel.Value=Text}"></asp:TextBox>

    </div>
</div>
