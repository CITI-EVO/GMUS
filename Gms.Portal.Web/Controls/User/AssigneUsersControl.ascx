<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AssigneUsersControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.AssigneUsersControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdRecordID" Property="{AssigneUsersModel.RecordID=Value}" />

<div class="row">
    <asp:Panel runat="server" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-2 control-label">Groups</ce:Label>
        <div class="col-lg-10">
            <ce:DropDownList runat="server" ID="cbxRecipientsGroup" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="cbxRecipientsGroup_OnSelectedIndexChanged">
            </ce:DropDownList>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-2 control-label">User</ce:Label>
        <div class="col-lg-9" style="padding-right: 2px;">
            <ce:DropDownList runat="server" ID="cbxUsers" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" CssClass="chosen-select">
            </ce:DropDownList>
        </div>
        <div class="col-lg-1" style="padding-left: 2px;">
            <ce:LinkButton runat="server" ToolTip="Save" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-plus" />
        </div>
    </asp:Panel>
    <div class="hr-line-dashed"></div>
    <asp:Panel runat="server" CssClass="form-group">
        <div class="col-lg-12">
            <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div style="white-space: nowrap;">
                                <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("UserID") %>' OnCommand="btnDeleteUser_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text='Login Name' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ce:Label runat="server" Text='<%# GetUserLogin(Eval("UserID")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text='User Email' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ce:Label runat="server" Text='<%# GetUserEmail(Eval("UserID")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text='User Name' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ce:Label runat="server" Text='<%# GetUserName(Eval("UserID")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </ce:GridView>
        </div>
    </asp:Panel>
</div>
