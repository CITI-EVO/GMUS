<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringBudgetDataControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringBudgetDataControl" %>

<asp:Panel runat="server" ID="pnlMessage">
    <asp:Label runat="server" ForeColor="Red" ID="lblError">
    </asp:Label>
</asp:Panel>
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
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="dropdown">
                        <span class="btn btn-info btn-circle fa fa-window-restore"></span>
                        <div class="dropdown-content">
                            <ce:LinkButton runat="server" ID="btnView" ToolTip="View" Visible='<%# GetViewVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnView_OnCommand" CssClass="btn btn-info btn-sm fa fa-eye" />
                            <ce:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" Visible='<%# GetEditVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm fa fa-edit" />
                            <ce:LinkButton runat="server" ID="btnFiles" ToolTip="Files" Visible='<%# GetFilesVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnFiles_OnCommand" CssClass="btn btn-success btn-sm fa fa-folder" />
                            <ce:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" Visible='<%# GetDeleteVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                            <ce:LinkButton runat="server" ID="btnStatus" ToolTip="Status" Visible='<%# GetStatusVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnStatus_OnCommand" CssClass="btn btn-warning btn-sm fa fa-cog" />
                            <ce:LinkButton runat="server" ID="btnHistory" ToolTip="History" Visible='<%# GetHistoryVisible(Container.DataItem) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="btnHistory_OnCommand" CssClass="btn btn-primary btn-sm fa fa-history" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    <div style="width: 80px;">
                        <asp:HiddenField runat="server" ID="hdID" />
                        <asp:HiddenField runat="server" ID="hdRecordID" />

                        <ce:LinkButton runat="server" ID="btnSave" OnClick="btnSave_OnClick" CssClass="btn-sm btn-success fa fa-save" />
                        <ce:LinkButton runat="server" ID="btnCancel" OnClick="btnCancel_OnClick" CssClass="btn-sm btn-warning fa fa-close" />
                    </div>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date of transfer">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Date of transfer' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# GetDatePart(Eval("DateOfTransfer")) %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <div class="input-group date" style="width: 160px;">
                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                        <asp:TextBox runat="server" ID="tbxDateOfTransfer" placeholder="Date Of Transfer" CssClass="form-control input-sm"></asp:TextBox>
                    </div>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Incoming">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Incoming' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Incoming") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox runat="server" ID="tbxIncoming" placeholder="Incoming" Width="80" CssClass="decSpinEdit-neg-sm input-sm"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Outgoing">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Outgoing' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Outgoing") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox runat="server" ID="tbxOutgoing" placeholder="Outgoing" Width="80" CssClass="decSpinEdit-neg-sm input-sm"></asp:TextBox>
                </FooterTemplate>
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
                <FooterTemplate>
                    <div style="width: 150px;">
                        <ce:DropDownList runat="server" Width="145" ID="cbxParagraphs" placeholder="Paragraph" DataTextField="Name" DataValueField="ID" CssClass="form-control" />
                    </div>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Goal">
                <HeaderTemplate>
                    <ce:Label runat="server" Text='Goal' />
                </HeaderTemplate>
                <ItemTemplate>
                    <ce:Label runat="server" Text='<%# Eval("Goal") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox runat="server" ID="tbxGoal" placeholder="Goal" CssClass="form-control input-sm" />
                </FooterTemplate>
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
