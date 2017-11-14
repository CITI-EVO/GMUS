<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SummaryBudgetDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.SummaryBudgetDataControl" %>

<asp:Panel runat="server" ID="pnlMessage">
    <asp:Label runat="server" ForeColor="Red" ID="lblError">
    </asp:Label>
</asp:Panel>
<div class="table-responsive">
    <ce:GridView ID="gvData"
        runat="server"
        AutoGenerateColumns="False"
        UseAccessibleHeader="True"
        TableSectionHeader="True"
        TableSectionFooter="true"
        ShowHeaderWhenEmpty="True"
        EnableViewState="False"
        ShowFooter="true"
        CssClass="tableStd table table-striped table-bordered table-hover"
        data-page-size="8"
        data-filter="#filter"
        OnRowDataBound="gridView_OnRowDataBound"
        Property="{SummaryBudgetDataModel.DataView=DataSource, Mode=Assigne}">
        <Columns>
            <asp:TemplateField HeaderText="Paragraph">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Paragraph' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Paragraph") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='Sum' ForeColor="Red" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Previous">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Previous' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("RemainPrevious") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' EnableViewState="False" ForeColor="Red" ID="lblRemainPrevious" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Plan (Grossing)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Plan (Grossing)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("PlanGrossing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' EnableViewState="False" ForeColor="Red" ID="lblPlanGrossing" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Plan (Current)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Plan (Current)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("PlanCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblPlanCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Enroll (Grossing)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Enroll (Grossing)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("EnrollGrossing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblEnrollGrossing" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Enroll (Current)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Enroll (Current)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("EnrollCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblEnrollCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Cash expense (Grossing)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Cash expense (Grossing)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("CashExpenseGrossing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblCashExpenseGrossing" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cash expense (Current)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Cash expense (Current)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("CashExpenseCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblCashExpenseCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Confirmed expense (Grossing)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Confirmed expense (Grossing)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("ConfirmedExpenseGrossing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblConfirmedExpenseGrossing" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Confirmed expense (Current)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Confirmed expense (Current)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("ConfirmedExpenseCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblConfirmedExpenseCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Unconfirmed expense (Grossing)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Unconfirmed expense (Grossing)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("UnconfirmedExpenseGrossing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblUnconfirmedExpenseGrossing" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Unconfirmed expense (Current)">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Unconfirmed expense (Current)' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("UnconfirmedExpenseCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblUnconfirmedExpenseCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remain">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Remain' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("RemainCurrent") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <b>
                        <asp:Label runat="server" Text='' ForeColor="Red" ID="lblRemainCurrent" />
                    </b>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
