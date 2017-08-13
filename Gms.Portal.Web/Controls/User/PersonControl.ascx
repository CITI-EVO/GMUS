<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PersonControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.PersonControl" %>

<fieldset>
    <legend></legend>
    <div class="form-group">
        <ce:Label runat="server" CssClass="col-lg-2 control-label">Personal ID</ce:Label>
        <div class="col-lg-10">
            <asp:TextBox runat="server" ID="tbxPersonalID" CssClass="form-control" />
        </div>
    </div>
    <div class="form-group">
        <ce:Label runat="server" CssClass="col-lg-2 control-label">Birth year</ce:Label>
        <div class="col-lg-10">
            <asp:TextBox runat="server" ID="seBirthYear" CssClass="intSpinEdit" />
        </div>
    </div>
    <div>
        <asp:Button runat="server" ID="btnOK" CssClass="btn btn-primary" OnClick="btnOK_OnClick" Text="OK" />
    </div>
</fieldset>

