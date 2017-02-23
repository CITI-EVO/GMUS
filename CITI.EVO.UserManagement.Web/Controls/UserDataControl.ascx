<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserDataControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UserDataControl" %>
<div class="col-sm-6">
    <div class="ibox-content form-horizontal">
        <div class="form-group">
            <label class="col-sm-4 control-label">მომხმარებლის სახელი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbLoginName" runat="server" class="form-control" Property="{UserModel.LoginName=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">პაროლი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbPassword" runat="server" class="form-control" Property="{UserModel.Password=Text}" />

            </div>
        </div>
        <asp:CheckBox ID="chkChangePassword" runat="server" AutoPostBack="true" Property="{UserModel.ChangePassword=Checked}" OnCheckedChanged="chkChangePassword_CheckChanged" />

        <div class="form-group">
            <label class="col-sm-4 control-label">სახელი</label>
            <div class="col-sm-6">

                <asp:TextBox ID="tbFirstName" runat="server" class="form-control" Property="{UserModel.FirstName=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">გვარი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbLastName" runat="server" class="form-control" Property="{UserModel.LastName=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">ელ-ფოსტა</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbEmail" runat="server" class="form-control" Property="{UserModel.Email=Text}"></asp:TextBox>

            </div>
        </div>

    </div>



</div>
<div class="col-sm-6">
    <div class="ibox-content form-horizontal">

        <div class="form-group">
            <label class="col-sm-4 control-label">მისამართი</label>
            <div class="col-sm-6">
                <asp:TextBox ID="tbAddress" runat="server" class="form-control" Property="{UserModel.Address=Text}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">აქტივაცია</label>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkActivate" runat="server" Property="{UserModel.IsActive=Checked}" />

            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">ვალიდურობის თარიღი</label>
            <div class="col-sm-6">
                <dx:ASPxDateEdit runat="server" class="form-control" ID="dePasswordExpire" Property="{UserModel.PasswordExpire=Value}"></dx:ASPxDateEdit>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-4 control-label">კატეგორია</label>
            <div class="col-sm-6">
                <dx:ASPxComboBox runat="server" ID="cbxUserCategory" class="form-control" ValueField="ID" TextField="Name" ValueType="System.Guid">
                </dx:ASPxComboBox>
            </div>
        </div>
    </div>

</div>

