<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ProjectsControl" %>

<ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
    CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{ProjectsModel.List=DataSource, Mode=Assigne}">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<% #Eval("ID")%>'
                    CssClass="btn btn-primary btn-sm fa fa-edit" ToolTip="რედაქტირება"
                    OnCommand="btnEdit_OnCommand" />
                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<% #Eval("ID")%>'
                    CssClass="btn btn-danger btn-sm btn fa fa-trash-o" ToolTip="წაშლა" OnCommand="btnDelete_OnCommand"
                    OnClientClick="return confirm('გსურთ მოდულის წაშლა?');" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="ID" DataField="ID" />
        <asp:BoundField HeaderText="მოდული" DataField="Name" />
        <asp:BoundField HeaderText="სტატუსი" DataField="IsActive" />
    </Columns>
</ce:GridView>
