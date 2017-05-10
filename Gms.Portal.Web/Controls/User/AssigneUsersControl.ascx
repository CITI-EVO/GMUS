<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AssigneUsersControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.AssigneUsersControl" %>

<%@ Register TagPrefix="local" TagName="SearchUserControl" Src="~/Controls/User/SearchUserControl.ascx" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdRecordID" Property="{AssigneUsersModel.RecordID=Value}" />

<div class="row">
    <asp:Panel runat="server" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-2 control-label">Status</ce:Label>
        <div class="col-lg-10">
            <asp:TextBox runat="server" ID="seStep" CssClass="intSpinEdit" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-2 control-label">Status</ce:Label>
        <div class="col-lg-10">
            <ce:DropDownList runat="server" ID="cbxUsers" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="True" CssClass="chosen-select">
            </ce:DropDownList>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="form-group">
        <div class="col-lg-12">
            <ce:LinkButton runat="server" ToolTip="Save" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="form-group">
        <div class="col-lg-12">
            <ce:GridView runat="server" ID="gvData" AutoGenerateColumns="False" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" EnableViewState="False" CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div style="white-space: nowrap;">
                                <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# GetCommandArg(Eval("Step"), Eval("UserID")) %>' OnCommand="btnDeleteUser_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text='Step' />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ce:Label runat="server" Text='<%# Eval("Step") %>' />
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
