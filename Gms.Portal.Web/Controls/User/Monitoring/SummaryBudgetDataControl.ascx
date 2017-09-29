<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SummaryBudgetDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.SummaryBudgetDataControl" %>

<asp:Panel runat="server" ID="pnlMessage">
    <asp:Label runat="server" ForeColor="Red" ID="lblError">
    </asp:Label>
</asp:Panel>
<ce:GridView ID="gvData"  
    runat="server"
    AutoGenerateColumns="False" 
    UseAccessibleHeader="True" 
    TableSectionHeader="True" 
    ShowHeaderWhenEmpty="True" 
    EnableViewState="False" 
    CssClass="tableStd table table-striped table-bordered table-hover" 
    data-page-size="8" 
    data-filter="#filter"
    OnRowDataBound="gridView_OnRowDataBound" 
    Property="{SummaryBudgetDataModel.DataView=DataSource, Mode=Assigne}">
    <Columns>
        <asp:TemplateField HeaderText="Task">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Task' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Task") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Start balance">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Start balance' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("StartBalance") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Plan">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Plan' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Plan") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Enroll">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Enroll' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Enroll") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Cash expense">
            <HeaderTemplate>
                <ce:Label runat="server" Text='CashExpense' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("CashExpense") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Confirmed expense">
            <HeaderTemplate>
                <ce:Label runat="server" Text='ConfirmedExpanse' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("ConfirmedExpanse") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Unconfirmed">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Unconfirmed' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Unconfirmed") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Balance">
            <HeaderTemplate>
                <ce:Label runat="server" Text='Balance' />
            </HeaderTemplate>
            <ItemTemplate>
                <ce:Label runat="server" Text='<%# Eval("Balance") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</ce:GridView>