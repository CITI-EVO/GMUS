<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringProjectFilesControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringProjectItemControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{MonitoringProjectFilesModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdFlaw" Property="{MonitoringProjectFilesModel.Flaw=Value}" />

<ce:Label runat="server" ID="lblResult"></ce:Label>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Files</ce:Label>
    <div class="col-lg-10">
        <div class="fileinput fileinput-new input-group" data-provides="fileinput">
            <div class="form-control" data-trigger="fileinput">
                <i class="glyphicon glyphicon-file fileinput-exists"></i>
                <span class="fileinput-filename"></span>
            </div>
            <span class="input-group-addon btn btn-default btn-file">
                <span class="fileinput-new">Select file</span>
                <span class="fileinput-exists">Change</span>
                <asp:FileUpload runat="server" ID="fuFiles" AllowMultiple="True" accept=".doc,.docx,.xls,.xlsx,.pdf" />
            </span>
            <a href="#" class="input-group-addon btn btn-default fileinput-exists" data-dismiss="fileinput">Remove</a>
        </div> 
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Url</ce:Label>
    <div class="col-lg-10">
        <ce:TextBox runat="server" CssClass="form-control" Property="{MonitoringProjectFilesModel.FileUrl=Text}" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Description</ce:Label>
    <div class="col-lg-10">
        <ce:TextBox runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" Property="{MonitoringProjectFilesModel.Description=Text}" />
    </div>
</div>

<div class="form-group">

    <ce:GridView ID="gvData"
        runat="server"
        AutoGenerateColumns="False"
        UseAccessibleHeader="True"
        TableSectionHeader="True"
        ShowHeaderWhenEmpty="True"
        EnableViewState="True"
        CssClass="tableStd table table-striped table-bordered table-hover"
        data-page-size="8">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="btnDownload" ToolTip='გადმოწერა' CssClass="btn btn-primary btn-sm fa fa-download" NavigateUrl='<%# GetUrl(Container.DataItem)  %>' Target="_blank" />
                    <ce:LinkButton runat="server" ID="lnkDelete" Visible='<%# GetDeleteVisible() %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" ToolTip="Delete/წაშლა" CssClass="btn btn-danger btn-sm fa fa-trash" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')"></ce:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File name">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='File name' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("FileName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Url">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='File Url' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%#  Eval("FileUrl") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File size">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='File size' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetFileSize(Eval("FileData")) %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Description">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Description' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Description") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
