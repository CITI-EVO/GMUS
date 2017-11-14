<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormControl" %>

<%@ Register Src="~/Controls/Management/RateControl.ascx" TagPrefix="local" TagName="RateControl" %>
<%@ Register Src="~/Controls/Management/ElementControl.ascx" TagPrefix="local" TagName="ElementControl" %>
<%@ Register Src="~/Controls/Management/ElementsControl.ascx" TagPrefix="local" TagName="ElementsControl" %>
<%@ Register Src="~/Controls/Management/ElementMoveControl.ascx" TagPrefix="local" TagName="ElementMoveControl" %>
<%@ Register Src="~/Controls/Management/ElementPasteControl.ascx" TagPrefix="local" TagName="ElementPasteControl" %>
<%@ Register Src="~/Controls/Management/PrintTemplateControl.ascx" TagPrefix="local" TagName="PrintTemplateControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>
<%@ Register Src="~/Controls/Management/MonitoringFlawControl.ascx" TagPrefix="local" TagName="MonitoringFlawControl" %>
<%@ Register Src="~/Controls/Management/MonitoringPrintFieldControl.ascx" TagPrefix="local" TagName="MonitoringPrintFieldControl" %>


<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{FormModel.ID=Value}" />

<div class="tabs-container">
    <ul class="nav nav-tabs">
        <li>
            <a data-toggle="tab" href="#tab-base" aria-expanded="true">Base</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-elements" aria-expanded="false">Elements</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-templates" aria-expanded="false">Templates</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-rating" aria-expanded="false">Rating</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-monitoring" aria-expanded="false">Monitoring Templates</a>
        </li>
    </ul>
    <div class="tab-content">
        <div id="tab-base" class="tab-pane active">
            <div class="panel-body">
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxName" Property="{FormModel.Name=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Abbreviation</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxAbbreviation" Property="{FormModel.Abbreviation=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Year</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxYear" Property="{FormModel.Year=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Number</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxNumber" Property="{FormModel.Number=Text}" CssClass="form-control" Enabled="False" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Order Index</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="seOrderIndex" Property="{FormModel.OrderIndex=Text}" CssClass="intSpinEdit" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Visible</ce:Label>
                    <div class="col-lg-10">
                        <asp:CheckBox runat="server" ID="chkVisible" Property="{FormModel.Visible=Checked}" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Visible Expression</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxVisibleExpression" Property="{FormModel.VisibleExpression=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Filling Validation Expression</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxFillingValidationExpression" Property="{FormModel.FillingValidationExpression=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Filling Validation Error Message</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="txtFillingValidationMessage" Property="{FormModel.FillingValidationMessage=Text}" CssClass="form-control" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">In Category</ce:Label>
                    <div class="col-lg-10">
                        <ce:DropDownList runat="server" ID="cbxCategory" DataTextField="Name" DataValueField="ID" Property="{FormModel.CategoryID=SelectedValue}" CssClass="chosen-select" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Filling mode</ce:Label>
                    <div class="col-lg-10">
                        <ce:DropDownList runat="server" ID="cbxUserMode" Property="{FormModel.UserMode=SelectedValue}" CssClass="chosen-select">
                            <Items>
                                <asp:ListItem Text="Standard" Value="Standard" />
                                <asp:ListItem Text="Single record" Value="SingleRecord" />
                            </Items>
                        </ce:DropDownList>
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Form type</ce:Label>
                    <div class="col-lg-10">
                        <ce:DropDownList runat="server" ID="cbxFormType" Property="{FormModel.FormType=SelectedValue}" CssClass="chosen-select">
                            <Items>
                                <asp:ListItem Text="Standard" Value="Standard" />
                                <asp:ListItem Text="Personal Profile" Value="PersonalProfile" />
                                <asp:ListItem Text="Organization Profile" Value="OrganizationProfile" />
                            </Items>
                        </ce:DropDownList>
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Requires Approve</ce:Label>
                    <div class="col-lg-10">
                        <asp:CheckBox runat="server" ID="chkRequiresApprove" Property="{FormModel.RequiresApprove=Checked}" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Approval Deadline</ce:Label>
                    <div class="col-lg-10">
                        <asp:TextBox runat="server" ID="tbxApprovalDeadline" Property="{FormModel.ApprovalDeadline=Text}" CssClass="intSpinEdit" />
                    </div>
                </div>
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:Label runat="server" CssClass="col-lg-2 control-label">Default filters</ce:Label>
                    <div class="col-lg-10">
                        <asp:CheckBox runat="server" ID="chkDefaultFilters" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-elements" class="tab-pane">
            <div class="panel-body">
                <div class="hr-line-dashed"></div>
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewElement" OnClick="btnNewElement_OnClick" CssClass="btn btn-primary fa fa-plus" />
                </div>
                <div class="form-group">
                    <local:ElementsControl runat="server" ID="elementsControl"
                        OnNew="elementsControl_OnNew"
                        OnEdit="elementsControl_OnEdit"
                        OnDelete="elementsControl_OnDelete"
                        OnCopy="elementsControl_OnCopy"
                        OnPaste="elementsControl_OnPaste"
                        OnMove="elementsControl_OnMove" />
                </div>
            </div>
        </div>
        <div id="tab-validations" class="tab-pane">
            <div class="panel-body">
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewValidation" OnClick="btnNewValidation_OnClick" CssClass="btn btn-primary fa fa-plus" />
                </div>
                <div class="form-group">
                    <ce:ASPxTreeList ID="tlValidations" runat="server" EnableViewState="False" AutoGenerateColumns="False"
                        KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
                        ViewStateMode="Disabled">
                        <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Both" ShowTreeLines="True" />
                        <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="True" AllowFocusedNode="true" />
                        <SettingsEditing Mode="EditFormAndDisplayNode" />
                        <SettingsPager Position="Bottom" PageSize="25">
                            <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
                            <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
                        </SettingsPager>
                        <SettingsLoadingPanel Text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
                        <Styles>
                            <Header ForeColor="#5D5D5D" Wrap="true" HorizontalAlign="Center">
                                <border bordercolor="#F7F7F7" borderstyle="Solid"></border>
                            </Header>
                            <AlternatingNode Enabled="true" />
                            <FocusedNode BackColor="#d7d7d7" ForeColor="#003399" />
                            <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf" Border-BorderWidth="1px">
                                <border bordercolor="#CFCFCF" borderwidth="1px" />
                            </Cell>
                            <Header HorizontalAlign="Center" />
                        </Styles>
                        <Columns>
                            <dx:TreeListTextColumn VisibleIndex="1">
                                <DataCellTemplate>
                                    <ce:LinkButton runat="server" ID="btnAddValidation" ToolTip="Add" CommandArgument='<%# Eval("ID") %>' OnCommand="btnAddRate_OnCommand" CssClass="btn btn-primary btn-xs fa fa-plus" />
                                    <ce:LinkButton runat="server" ID="btnEditValidation" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEditRate_OnCommand" CssClass="btn btn-info btn-xs fa fa-edit" />
                                    <ce:LinkButton runat="server" ID="btnDeleteValidation" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteRate_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="2">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Name" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("Name") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="2">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Field" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("Field") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="4">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Expression" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("Expression") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="5">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="ErrorMessage" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("ErrorMessage") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                        </Columns>
                    </ce:ASPxTreeList>
                </div>
            </div>
        </div>
        <div id="tab-templates" class="tab-pane">
            <div class="panel-body">
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewTemplate" OnClick="btnNewTemplate_OnClick" CssClass="btn btn-primary fa fa-plus" />
                </div>
                <div class="form-group">
                    <ce:GridView runat="server" ID="gvTemplates" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
                        CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="150" />
                                <ItemTemplate>
                                    <ce:LinkButton runat="server" ID="btnEditTemplate" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEditTemplate_OnCommand" CssClass="btn btn-primary btn-xs fa fa-edit" />
                                    <ce:LinkButton runat="server" ID="btnDeleteTemplate" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteTemplate_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Name" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ce:GridView>
                </div>
            </div>
        </div>
        <div id="tab-rating" class="tab-pane">
            <div class="panel-body">
                <div class="form-group">
                    <div>
                        <ce:Label runat="server" Text="Selector Expression" />
                    </div>
                    <div>
                        <asp:TextBox runat="server" ID="tbxSelectorExpression" Width="100%" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div>
                        <ce:Label runat="server" Text="Summary Expression" />
                    </div>
                    <div>
                        <asp:TextBox runat="server" ID="tbxSummaryExpression" Width="100%" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div>
                        <ce:Label runat="server" Text="Print Template" />
                    </div>
                    <div>
                        <asp:TextBox runat="server" ID="tbxPrintTemplate" TextMode="MultiLine" Width="100%" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div>
                        <ce:Label runat="server" Text="Mail Template" />
                    </div>
                    <div>
                        <asp:TextBox runat="server" ID="tbxMailTemplate" TextMode="MultiLine" Width="100%" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewRate" OnClick="btnNewRate_OnClick" CssClass="btn btn-primary fa fa-plus" />
                </div>
                <div class="form-group">
                    <ce:ASPxTreeList ID="tlRates" runat="server" EnableViewState="False" AutoGenerateColumns="False"
                        KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%"
                        ViewStateMode="Disabled" OnHtmlRowPrepared="tlRates_OnHtmlRowPrepared">
                        <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Both" ShowTreeLines="True" />
                        <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="True" AllowFocusedNode="true" />
                        <SettingsEditing Mode="EditFormAndDisplayNode" />
                        <SettingsPager Position="Bottom" PageSize="25">
                            <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
                            <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
                        </SettingsPager>
                        <SettingsLoadingPanel Text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
                        <Styles>
                            <Header ForeColor="#5D5D5D" Wrap="true" HorizontalAlign="Center">
                                <border bordercolor="#F7F7F7" borderstyle="Solid"></border>
                            </Header>
                            <AlternatingNode Enabled="true" />
                            <FocusedNode BackColor="#d7d7d7" ForeColor="#003399" />
                            <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf" Border-BorderWidth="1px">
                                <border bordercolor="#CFCFCF" borderwidth="1px" />
                            </Cell>
                            <Header HorizontalAlign="Center" />
                        </Styles>
                        <Columns>
                            <dx:TreeListTextColumn VisibleIndex="1">
                                <DataCellTemplate>
                                    <ce:LinkButton runat="server" ID="btnAddRate" ToolTip="Add" CommandArgument='<%# Eval("ID") %>' OnCommand="btnAddRate_OnCommand" CssClass="btn btn-primary btn-xs fa fa-plus" />
                                    <ce:LinkButton runat="server" ID="btnEditRate" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEditRate_OnCommand" CssClass="btn btn-info btn-xs fa fa-edit" />
                                    <ce:LinkButton runat="server" ID="btnDeleteRate" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteRate_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="2">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Name" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("Name") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="4">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Min" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("MinScore") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn VisibleIndex="5">
                                <HeaderCaptionTemplate>
                                    <ce:Label runat="server" Text="Max" />
                                </HeaderCaptionTemplate>
                                <DataCellTemplate>
                                    <ce:Label runat="server" Text='<%#Eval("MaxScore") %>' />
                                </DataCellTemplate>
                            </dx:TreeListTextColumn>
                        </Columns>
                    </ce:ASPxTreeList>
                </div>
            </div>
        </div>
        <div id="tab-monitoring" class="tab-pane">
            <div class="panel-body">
                <h5><ce:Label runat="server">Printing</ce:Label></h5>
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewMonitoringFieldNew" OnClick="btnNewMonitoringFieldNew_OnClick" CssClass="btn btn-success fa fa-plus" />
                </div>
                <div class="form-group">
                    <ce:GridView runat="server" ID="gvMonitoringPrint" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
                        CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text=" " />
                                </HeaderTemplate>
                                <ItemStyle Width="150" />
                                <ItemTemplate>
                                    <ce:LinkButton runat="server" ID="btnEditMonitoringPrint" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEditMonitoringPrintField_OnCommand" CssClass="btn btn-primary btn-xs fa fa-edit" />
                                    <ce:LinkButton runat="server" ID="btnDeleteMonitoringPrint" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteMonitoringPrintField_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Name" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Print Type" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("PrintType") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Budget" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# GetCheckStatus(Eval("BudgetForm")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Summary Budget" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# GetCheckStatus(Eval("SummaryBudgetForm")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Projects" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# GetCheckStatus(Eval("ProjectsForm")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ce:GridView>
                </div>
            </div>
            <div class="panel-body">
                <h5><ce:Label runat="server">Flaws</ce:Label></h5>
                <div class="form-group">
                    <ce:LinkButton runat="server" ID="btnNewMonitoringFlaw" OnClick="btnNewMonitoringFlaw_OnClick" CssClass="btn btn-success fa fa-plus" />
                </div>
                <div class="form-group">
                    <ce:GridView runat="server" ID="gvMonitoringFlaws" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
                        CssClass="tableStd table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" ShowHeaderWhenEmpty="True">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text=" " />
                                </HeaderTemplate>
                                <ItemStyle Width="150" />
                                <ItemTemplate>
                                    <ce:LinkButton runat="server" ID="btnEditMonitoringFlaw" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEditMonitoringFlaw_OnCommand" CssClass="btn btn-primary btn-xs fa fa-edit" />
                                    <ce:LinkButton runat="server" ID="btnDeleteMonitoringFlaw" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteMonitoringFlaw_OnCommand" CssClass="btn btn-danger btn-xs fa fa-trash" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Name" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Score" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("Score") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <ce:Label runat="server" Text="Type" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ce:Label runat="server" Text='<%# Eval("Type") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ce:GridView>
                </div>
            </div>
        </div>
    </div>
</div>

<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeElement" runat="server" Style="display: none" DefaultButton="btnElementOK">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Element" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="lblFormElement" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:ElementControl runat="server" ID="elementControl" OnDataChanged="elementControl_OnDataChanged" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnElementOK" OnClick="btnElementOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnElementCancel" OnClick="btnElementCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>

<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeElementMove" runat="server" Style="display: none" DefaultButton="btnElementMoveOK">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Element" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="lblElementMoveElement" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:ElementMoveControl runat="server" ID="elementMoveControl" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnElementMoveOK" OnClick="btnElementMoveOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnElementMoveCancel" OnClick="btnElementMoveCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>

<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeElementPaste" runat="server" Style="display: none" DefaultButton="btnElementPasteOK">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Element" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="lblPasteResult" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:ElementPasteControl runat="server" ID="elementPasteControl" OnDataChanged="elementPasteControl_OnDataChanged" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnElementPasteOK" OnClick="btnElementPasteOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnElementPasteCancel" OnClick="btnElementPasteCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>

<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpePrintTemplate" runat="server" Style="display: none" DefaultButton="btnPrintTemplateOK">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Template" />
                    </h5>
                </div>
                <div class="modal-body">
                    <local:PrintTemplateControl runat="server" ID="printTemplateControl" OnDataChanged="printTemplateControl_OnDataChanged" />
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnPrintTemplateOK" OnClick="btnPrintTemplateOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnPrintTemplateCancel" OnClick="btnPrintTemplateCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>

<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeRate" runat="server" Style="display: none" DefaultButton="btnRateOK">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Element" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="lblRateMessage" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:RateControl runat="server" ID="rateControl" OnDataChanged="rateControl_OnDataChanged" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnRateOK" OnClick="btnRateOK_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnRateCancel" OnClick="btnRateCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>



<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringFields" runat="server" Style="display: none" DefaultButton="btnMonitoringFieldOk">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Monitoring Print Fields" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="lblMonitoringField" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:MonitoringPrintFieldControl runat="server" ID="monitoringPrintFieldControl" OnDataChanged="monitoringPrintFieldControl_OnDataChanged" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnMonitoringFieldOk" OnClick="btnMonitoringFieldOk_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnMonitoringFieldCancel" OnClick="btnMonitoringFieldCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>



<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeMonitoringFlaw" runat="server" Style="display: none" DefaultButton="btnMonitoringFlawOk">
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Monitoring Flaws" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <ce:Label ID="Label1" runat="server" ForeColor="Red"></ce:Label>
                    </div>
                    <div class="form-group">
                        <local:MonitoringFlawControl runat="server" ID="monitoringFlawControl" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnMonitoringFlawOk" OnClick="btnMonitoringFlawOk_OnClick" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnMonitoringFlawCancel" OnClick="btnMonitoringFlawCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>
