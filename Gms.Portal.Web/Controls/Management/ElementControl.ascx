<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ElementModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ElementModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentType" Property="{ElementModel.ParentType=Value}" />

<div class="tabs-container">
    <ul class="nav nav-tabs">
        <li>
            <a data-toggle="tab" href="#tab-properties" aria-expanded="true">Base</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-datasource" aria-expanded="false">Data Source</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-expressions" aria-expanded="false">Expressions</a>
        </li>
        <li>
            <a data-toggle="tab" href="#tab-dependency" aria-expanded="false">Dependency</a>
        </li>
    </ul>
    <div class="tab-content">
        <div id="tab-properties" class="tab-pane active">
            <div class="panel-body">
                <asp:Panel runat="server" ID="pnlElementType" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Element Type</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxElementType" AppendDataBoundItems="True" Property="{ElementModel.ElementType=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlName" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Name</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{ElementModel.Name=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlAlias" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Alias</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxAlias" CssClass="form-control" Property="{ElementModel.Alias=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDescription" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Description</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxDescription" CssClass="form-control" Property="{ElementModel.Description=Text}" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlOrderIndex" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-lg-6 font-normal">Order Index</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="seOrderIndex" Property="{ElementModel.OrderIndex=Text}" CssClass="intSpinEdit" />
                    </div>
                </asp:Panel>
               
                <asp:Panel runat="server" ID="pnlGroupSize" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Group Size</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxGroupSize" Property="{ElementModel.GroupSize=Text}" CssClass="elementSizeSpinEdit" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlGroupBgColor" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Background Color</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxGroupBgColor" Property="{ElementModel.GroupBgColor=Text}" CssClass="form-control colorpicker colorpicker-element" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlGroupTextColor" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Title Color</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxGroupTextColor" Property="{ElementModel.GroupTextColor=Text}" CssClass="form-control colorpicker colorpicker-element" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlType" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Control Type</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxType" AppendDataBoundItems="True" Property="{ElementModel.ControlType=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
                            <Items>
                                <asp:ListItem Text="Select an Option" Value="" />
                                <asp:ListItem Text="TextBox" Value="TextBox" />
                                <asp:ListItem Text="TextArea" Value="TextArea" />
                                <asp:ListItem Text="Lookup" Value="Lookup" />
                                <asp:ListItem Text="Label" Value="Label" />
                                <asp:ListItem Text="Date" Value="Date" />
                                <asp:ListItem Text="Time" Value="Time" />
                                <asp:ListItem Text="Number (float)" Value="Number" />
                                <asp:ListItem Text="Number (int)" Value="Number_Int" />
                                <asp:ListItem Text="CheckBox" Value="CheckBox" />
                                <asp:ListItem Text="ComboBox" Value="ComboBox" />
                                <asp:ListItem Text="CheckBoxList" Value="CheckBoxList" />
                                <asp:ListItem Text="RagioButton" Value="RagioButton" />
                                <asp:ListItem Text="FileUpload" Value="FileUpload" />
                                <asp:ListItem Text="PersonLookup" Value="PersonLookup" />
                            </Items>
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTotalSize" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Total Size</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxTotalSize" Property="{ElementModel.TotalSize=Text}" CssClass="elementSizeSpinEdit" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlCaptionSize" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Caption size</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxCaptionSize" Property="{ElementModel.CaptionSize=Text}" CssClass="elementSizeSpinEdit" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlControlSize" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Control size</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxControlSize" Property="{ElementModel.ControlSize=Text}" CssClass="elementSizeSpinEdit" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlGridFieldSummary" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Grid Field Summary</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxGridFieldSummary" AppendDataBoundItems="True" Property="{ElementModel.GridFieldSummary=SelectedValue}" CssClass="chosen-select">
                            <Items>
                                <asp:ListItem Text="Select an Option" Value="" />
                                <asp:ListItem Text="Sum" Value="Sum" />
                                <asp:ListItem Text="Avg" Value="Avg" />
                                <asp:ListItem Text="Max" Value="Max" />
                                <asp:ListItem Text="Min" Value="Min" />
                            </Items>
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTreeMaxLevel" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Tree max level</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxTreeMaxLevel" Property="{ElementModel.TreeMaxLevel=Text}" CssClass="elementSizeSpinEdit" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlProperties" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Properties</ce:Label>
                    <div class="col-lg-6">
                        <ce:ListBox runat="server" ID="lstProperties" SelectionMode="Multiple" AppendDataBoundItems="True" CssClass="chosen-select">
                            <Items>
                                <asp:ListItem Text="Unique" Value="Unique" />
                                <asp:ListItem Text="Privacy" Value="Privacy" />
                                <asp:ListItem Text="Visible" Value="Visible" />
                                <asp:ListItem Text="ReadOnly" Value="ReadOnly" />
                                <asp:ListItem Text="Mandatory" Value="Mandatory" />
                                <asp:ListItem Text="Inversion" Value="Inversion" />
                                <asp:ListItem Text="FilterByUser" Value="FilterByUser" />
                                <asp:ListItem Text="NotPrintable" Value="NotPrintable" />
                                <asp:ListItem Text="FirstTimeFill" Value="FirstTimeFill" />
                                <asp:ListItem Text="AllowBulkFill" Value="AllowBulkFill" />
                                <asp:ListItem Text="ResetDataOnHide" Value="ResetDataOnHide" />
                                <asp:ListItem Text="DisplayOnFilter" Value="DisplayOnFilter" />
                                <asp:ListItem Text="RequiresApproval" Value="RequiresApproval" />
                            </Items>
                        </ce:ListBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDisplayOnGrid" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Display On Grid</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxDisplayOnGrid" AppendDataBoundItems="True" Property="{ElementModel.DisplayOnGrid=SelectedValue}" CssClass="chosen-select">
                            <Items>
                                <asp:ListItem Text="Select an Option" Value="" />
                                <asp:ListItem Text="None" Value="None" />
                                <asp:ListItem Text="Always" Value="Always" />
                                <asp:ListItem Text="Conditional" Value="Conditional" />
                            </Items>
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlMask" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Mask</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxMask" CssClass="form-control" Property="{ElementModel.Mask=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTag" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Tag</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxTag" CssClass="form-control" Property="{ElementModel.Tag=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div id="tab-datasource" class="tab-pane">
            <div class="panel-body">
                <asp:Panel runat="server" ID="pnlDataSource" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Data Source</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxDataSource" AppendDataBoundItems="True" DataTextField="Key" DataValueField="Value" Property="{ElementModel.DataSourceID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDataSourceFilterExp" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Filter Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxDataSourceFilterExp" CssClass="form-control" Property="{ElementModel.DataSourceFilterExp=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDataSourceSortExp" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Sort Fields</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxDataSourceSortExp" CssClass="form-control" Property="{ElementModel.DataSourceSortExp=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlTextExpression" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Text Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxTextExpression" CssClass="form-control" Property="{ElementModel.TextExpression=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlValueExpression" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Value Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxValueExpression" CssClass="form-control" Property="{ElementModel.ValueExpression=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div id="tab-expressions" class="tab-pane">
            <div class="panel-body">
                <asp:Panel runat="server" ID="pnlVisibleExpression" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Visible Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxVisibleExpression" CssClass="form-control" Property="{ElementModel.VisibleExpression=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlFieldValueExpression" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Field Value Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxFieldValueExpression" Property="{ElementModel.FieldValueExpression=Text}" CssClass="form-control" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlValidationExp" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Validation Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxValidationExp" CssClass="form-control" Property="{ElementModel.ValidationExp=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlErrorMessage" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Error Message</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxErrorMessage" CssClass="form-control" Property="{ElementModel.ErrorMessage=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
        </div>
        <div id="tab-dependency" class="tab-pane">
            <div class="panel-body">
                <asp:Panel runat="server" ID="pnlDependentField" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Dependent field</ce:Label>
                    <div class="col-lg-6">
                        <ce:DropDownList runat="server" ID="cbxDependentField" AppendDataBoundItems="True" DataTextField="Name" DataValueField="ID" Property="{ElementModel.DependentFieldID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
                        </ce:DropDownList>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDependentExp" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Dependent expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxDependentExp" CssClass="form-control" Property="{ElementModel.DependentExp=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlDependentFillExp" CssClass="form-group">
                    <ce:Label runat="server" CssClass="col-sm-6 font-normal">Dependent Fill Expression</ce:Label>
                    <div class="col-lg-6">
                        <asp:TextBox runat="server" ID="tbxDependentFillExp" CssClass="form-control" Property="{ElementModel.DependentFillExp=Text}"></asp:TextBox>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>
<asp:Panel runat="server" ID="pnlServiceParameters" Visible="False" CssClass="form-group">
    <div class="col-lg-12">
        <div class="col-lg-4">
            <ce:DropDownList runat="server" ID="cbxParameters" DataTextField="Name" DataValueField="Name" AppendDataBoundItems="True" CssClass="chosen-select">
            </ce:DropDownList>
        </div>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxParameterExp" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-lg-2">
            <ce:LinkButton runat="server" ID="btnSaveParam" ToolTip="Save" OnClick="btnSaveParam_OnClick" CssClass="btn btn-primary fa fa-plus" />
        </div>
        <div class="col-lg-12">
            <ce:GridView runat="server" ID="gvParameters" UseAccessibleHeader="True" TableSectionHeader="True" TableSectionFooter="True" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
                CssClass="table table-bordered" data-page-size="8">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <ce:LinkButton runat="server" ID="btnDeleteParam" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDeleteParam_OnCommand" CssClass="btn btn-danger btn-sm fa fa-trash-o" OnClientClick="return confirm('დარწმუნებული ხართ?/Are you sure?')" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text="Name" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <ce:Label runat="server" Text="Expression" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Expression") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </ce:GridView>
        </div>
    </div>
</asp:Panel>


