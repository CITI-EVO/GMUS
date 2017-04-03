<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ElementControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ElementControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ElementModel.ID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentID" Property="{ElementModel.ParentID=Value}" />
<local:HiddenFieldValueControl runat="server" ID="hdParentType" Property="{ElementModel.ParentType=Value}" />
<div class="col-sm-6">
    <asp:Panel runat="server" ID="pnlElementType" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Element Type</ce:Label>
        <div class="col-lg-6">
            <ce:DropDownList runat="server" ID="cbxElementType" AppendDataBoundItems="True" Property="{ElementModel.ElementType=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
            </ce:DropDownList>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlVisible" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Visible</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="tbxVisible" Property="{ElementModel.Visible=Checked}"></asp:CheckBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlName" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Name</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxName" CssClass="form-control" Property="{ElementModel.Name=Text}"></asp:TextBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlOrderIndex" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-lg-6 font-normal">Order Index</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="seOrderIndex" Property="{ElementModel.OrderIndex=Text}" CssClass="intSpinEdit" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlMandatory" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Mandatory</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="chkMandatory" Property="{ElementModel.Mandatory=Checked}" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrivacy" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Privacy</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="chkPrivacy" Property="{ElementModel.Privacy=Checked}" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDescription" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Description</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxDescription" CssClass="form-control" Property="{ElementModel.Description=Text}" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlGroupAlign" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Group Size</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxGroupSize" Property="{ElementModel.GroupSize=Text}" CssClass="elementSizeSpinEdit" />
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
                    <asp:ListItem Text="Date" Value="Date" />
                    <asp:ListItem Text="Time" Value="Time" />
                    <asp:ListItem Text="Number" Value="Number" />
                    <asp:ListItem Text="CheckBox" Value="CheckBox" />
                    <asp:ListItem Text="ComboBox" Value="ComboBox" />
                    <asp:ListItem Text="RagioButton" Value="RagioButton" />
                    <asp:ListItem Text="FileUpload" Value="FileUpload" />
                </Items>
            </ce:DropDownList>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCaptionSize" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Caption size</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxCaptionSize" Property="{ElementModel.CaptionSize=Text}" CssClass="elementSizeSpinEdit" />
        </div>
    </asp:Panel>
</div>
<div class="col-lg-6">
    <asp:Panel runat="server" ID="pnlControlSize" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Control size</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxControlSize" Property="{ElementModel.ControlSize=Text}" CssClass="elementSizeSpinEdit" />
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInversion" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Inversion</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="chkInversion" Property="{ElementModel.Inversion=Checked}"></asp:CheckBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlMask" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Mask</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxMask" CssClass="form-control" Property="{ElementModel.Mask=Text}"></asp:TextBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEnabled" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Enabled</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="chkEnabled" Property="{ElementModel.Enabled=Checked}"></asp:CheckBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDisplayOnGrid" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Display On Grid</ce:Label>
        <div class="col-lg-6">
            <asp:CheckBox runat="server" ID="chkDisplayOnGrid" Property="{ElementModel.DisplayOnGrid=Checked}"></asp:CheckBox>
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
    <asp:Panel runat="server" ID="pnlTag" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Tag</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxTag" CssClass="form-control" Property="{ElementModel.Tag=Text}"></asp:TextBox>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDataSource" CssClass="form-group">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Data Source</ce:Label>
        <div class="col-lg-6">
            <ce:DropDownList runat="server" ID="cbxDataSource" AppendDataBoundItems="True" DataTextField="Name" DataValueField="ID" Property="{ElementModel.DataSourceID=SelectedValue}" CssClass="chosen-select" AutoPostBack="True" OnSelectedIndexChanged="comboBox_OnSelectedIndexChanged">
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
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Filter Expression</ce:Label>
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
    <asp:Panel runat="server" ID="pnlDependentFillExp" CssClass="form-group" Visible="False">
        <ce:Label runat="server" CssClass="col-sm-6 font-normal">Dependent Fill Expression</ce:Label>
        <div class="col-lg-6">
            <asp:TextBox runat="server" ID="tbxDependentFillExp" CssClass="form-control" Property="{ElementModel.DependentFillExp=Text}"></asp:TextBox>
        </div>
    </asp:Panel>
</div>

