<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectsControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.ProjectsControl" %>
<div class="ibox-content">

    <div class="table-responsive">
        <ce:ASPxGridView ID="gvData" runat="server" KeyFieldName="ID"  CssClass="table table-striped table-bordered table-hover"
            Property="{ProjectsModel.List=DataSource, Mode=Assigne}"  EnableRowsCache="False">

            <Columns>
                <dx:GridViewDataColumn VisibleIndex="4" Name="Edit">
                    <DataItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<% #Eval("ID")%>'
                           CssClass="btn btn-primary btn-sm fa fa-edit" ToolTip="რედაქტირება"
                            OnCommand="btnEdit_OnCommand" />
                        <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<% #Eval("ID")%>'
                            CssClass="btn btn-danger btn-sm btn fa fa-trash-o" ToolTip="წაშლა" OnCommand="btnDelete_OnCommand"
                            OnClientClick="return confirm('გსურთ მოდულის წაშლა?');" />
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn Caption="ID" FieldName="ID" VisibleIndex="1" Visible="true" />
                <dx:GridViewDataColumn Caption="მოდული" FieldName="Name" VisibleIndex="2">
                    <HeaderCaptionTemplate>
                        <asp:Label runat="server">მოდული</asp:Label>
                    </HeaderCaptionTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn Caption="სტატუსი" FieldName="IsActive" VisibleIndex="3">
                    <HeaderCaptionTemplate>
                        <asp:Label ID="Label2" runat="server">სტატუსი</asp:Label>
                    </HeaderCaptionTemplate>
                </dx:GridViewDataColumn>
            </Columns>
        </ce:ASPxGridView>
    </div>
</div>
