<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserDataControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UserDataControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{UserModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdIsSuperAdmin" Property="{UserModel.IsSuperAdmin=Value}" />

<div class="col-sm-6">
    <div class="ibox-content form-horizontal">
        <div class="form-group">
            <label class="col-sm-6 font-normal">მომხმარებლის სახელი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbLoginName" runat="server" class="form-control" Property="{UserModel.LoginName=Text}" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">პაროლის შეცვლა</label>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkChangePassword" runat="server" AutoPostBack="true" Property="{UserModel.ChangePassword=Checked}" OnCheckedChanged="chkChangePassword_CheckChanged" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">პაროლი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbPassword" runat="server" class="form-control" Property="{UserModel.Password=Text}" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">სახელი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbFirstName" runat="server" class="form-control" Property="{UserModel.FirstName=Text}" />
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">გვარი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbLastName" runat="server" class="form-control" Property="{UserModel.LastName=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">ელ-ფოსტა</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbEmail" runat="server" class="form-control" Property="{UserModel.Email=Text}"></asp:TextBox>
            </div>
        </div>
        
         <div class="form-group">
            <label class="col-sm-6 font-normal">ტელეფონი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbxPhone" runat="server" class="form-control" Property="{UserModel.Phone=Text}"></asp:TextBox>
            </div>
        </div>
    </div>
</div>
<div class="col-sm-6">
    <div class="ibox-content form-horizontal">
        <div class="form-group">
            <label class="col-sm-6 font-normal">მისამართი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbAddress" runat="server" class="form-control" Property="{UserModel.Address=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-6 font-normal">აქტივაცია</label>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkActivate" runat="server" Property="{UserModel.IsActive=Checked}" />

            </div>
        </div>

        <div class="form-group" id="data_1">
            <label class="col-sm-6 font-normal" style="padding-right: 0;">ვალიდურობის თარიღი</label>
            <div class="col-sm-6 input-group date" style="padding-left:15px;">
                <span class="input-group-addon">
                    <i class="fa fa-calendar"></i>
                </span>
                <asp:TextBox runat="server" CssClass="form-control" Width="100" ID="tbxPasswordExpire" property="{UserModel.PasswordExpire=Text}"/>
            </div>
        </div>
    </div>
</div>

