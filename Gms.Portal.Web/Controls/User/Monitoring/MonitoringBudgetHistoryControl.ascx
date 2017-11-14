<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringBudgetHistoryControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringBudgetHistoryControl" %>


<div class="table-responsive">
    <ce:GridView ID="gvData"
                 runat="server"
                 ShowFooter="False"
                 TableSectionFooter="True"
                 AutoGenerateColumns="False"
                 UseAccessibleHeader="True"
                 TableSectionHeader="True"
                 ShowHeaderWhenEmpty="True"
                 EnableViewState="False"
                 CssClass="tableStd table table-striped table-bordered table-hover"
                 data-page-size="8"
                 data-filter="#filter"
                 OnRowDataBound="gvData_OnRowDataBound"
                 Property="{MonitoringBudgetDataModel.DataView=DataSource, Mode=Assigne}">
        <Columns>
            <asp:TemplateField HeaderText="Date of transfer">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Date of transfer' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetDatePart(Eval("DateOfTransfer")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Incoming">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Incoming' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Incoming") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Outgoing">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Outgoing' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Outgoing") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remain">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Remain' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Remain") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Paragraph">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Paragraph' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetParagraphName(Eval("ParagraphID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Goal">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Goal' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Goal") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Comment">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Comment' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Comment") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Status") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status user">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status user' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetUserName(Eval("StatusUserID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status date">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Status date' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("StatusDate") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Expire date">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Expire date' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("ExpireDate") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Flaw">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Flaw' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetFlewsNames(Eval("FlawsID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Flew scores">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Flew scores' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# GetFlewsScores(Eval("FlawsID")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date of creation">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Date of creation' />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DateCreated") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ce:GridView>
</div>
