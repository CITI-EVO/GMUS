<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="FormDataGrid.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.FormDataGrid" %>

<%@ Register TagPrefix="local" TagName="FormElementControl" Src="~/Controls/Management/FormElementControl.ascx" %>
<%@ Register TagPrefix="local" TagName="FormStructureControl" Src="~/Controls/Management/FormStructureControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div>
        <asp:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" Text="New"></asp:LinkButton>
    </div>
    <div>
        <asp:GridView runat="server" ID="gvData" AutoGenerateColumns="False" EnableViewState="False">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnEdit" Text="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand"/>
                        <asp:LinkButton runat="server" ID="btnView" Text="View" CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand"/>
                        <asp:LinkButton runat="server" ID="btnDelete" Text="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
