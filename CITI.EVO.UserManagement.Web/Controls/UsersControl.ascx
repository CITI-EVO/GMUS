<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UsersControl" %>

<div class="table-responsive">
   <div class="dataTables_wrapper form-inline dt-bootstrap">       
        <ce:GridView runat="server" ID="gvData" UseAccessibleHeader="True" TableSectionHeader="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{UsersModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <ce:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-primary btn-sm fa fa-edit" ToolTip="რედაქტირება" CommandArgument='<% #Eval("ID")%>' OnCommand="btnEdit_OnCommand" />
                        <ce:LinkButton ID="lnkView" runat="server" CssClass="btn btn-info btn-sm fa fa-search" ToolTip="ნახვა" CommandArgument='<% #Eval("ID")%>' OnCommand="btnView_OnCommand" />
                        <ce:LinkButton ID="btnNewMessage" runat="server" CssClass="btn btn-primary btn-sm fa fa-plus-circle" ToolTip="მესიჯის დამატება" CommandArgument='<% #Eval("ID")%>' OnCommand="btnNewMessage_OnCommand" />
                        <ce:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger btn-sm fa fa-trash-o" ToolTip="წაშლა" OnClientClick="return confirm('გსურთ მომხმარებილის წაშლა?');" CommandArgument='<% #Eval("ID")%>' OnCommand="btnDelete_OnCommand" />
                        <ce:LinkButton ID="btnSetAttribute" runat="server" CssClass="btn btn-primary btn-sm fa fa-plus-circle" ToolTip="ატრიბუტების დამატება" CommandArgument='<% #Eval("ID")%>' OnCommand="btnSetAttribute_OnCommand" />
                        <ce:LinkButton ID="btnViewAttributes" runat="server" CssClass="btn btn-info btn-sm fa fa-search" ToolTip="ატრიბუტების ნახვა" CommandArgument='<% #Eval("ID")%>' OnCommand="btnViewAttributes_OnCommand" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="ID" DataField="ID" />
                <asp:BoundField HeaderText="მომხმარებელი" DataField="LoginName" />
                <asp:BoundField HeaderText="პაროლი" DataField="Password" />
                <asp:BoundField HeaderText="სახელი" DataField="FirstName" />
                <asp:BoundField HeaderText="გვარი" DataField="LastName" />
                <asp:BoundField HeaderText="აქტივაცია" DataField="IsActive" />
                <asp:BoundField HeaderText="ელ.ფოსტა" DataField="Email" />
                <asp:BoundField HeaderText="ტელეფონი" DataField="Phone" />
                <asp:BoundField HeaderText="მისამართი" DataField="Address" />
                <asp:BoundField HeaderText="პაროლის ვალიდურობის თარიღი" DataField="PasswordExpire" />
            </Columns>
        </ce:GridView>
    </div>
</div>
