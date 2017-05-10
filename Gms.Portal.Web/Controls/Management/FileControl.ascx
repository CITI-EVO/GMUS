<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FileControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{FileModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{FileModel.ParentID=Value}" />

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" Property="{FileModel.Name=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" Property="{FileModel.Description=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">File</ce:Label>
    <div class="col-lg-10">
        <asp:FileUpload runat="server" ID="fuData" Property="{FileModel.FileName=FileName,Mode=Receive}{FileModel.FileData=FileBytes,Mode=Receive}"  CssClass="form-control"></asp:FileUpload>
    </div>
</div>

