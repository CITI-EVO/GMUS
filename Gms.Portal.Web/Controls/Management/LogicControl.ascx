<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogicControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.LogicControl" %>

<%@ Register Src="~/Controls/Common/ExpressionsLogicControl.ascx" TagPrefix="local" TagName="ExpressionsLogicControl" %>

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Source Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="lstSourceType" Property="{LogicModel.SourceType=SelectedValue}" AutoPostBack="True" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Form" Value="Form" Selected="True" />
                <asp:ListItem Text="Logic" Value="Logic" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Source</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="cbxSource" DataTextField="Name" DataValueField="ID" ValueType="System.Guid" Property="{LogicModel.SourceID=SelectedValue}" CssClass="chosen-select">
        </asp:DropDownList>
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Logic Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="lstType" Property="{LogicModel.Type=SelectedValue}" AutoPostBack="True" CssClass="chosen-select">
            <Items>
                <asp:ListItem Text="Logic" Value="Logic" Selected="True" />
                <asp:ListItem Text="Query" Value="Query" />
            </Items>
        </asp:DropDownList>
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" Property="{LogicModel.Name=Text}" CssClass="form-control"></asp:TextBox>
    </div>
</div>
<asp:Panel runat="server" ID="pnlQuery" CssClass="form-group">
    <asp:TextBox runat="server" TextMode="MultiLine" Property="{LogicModel.Query=Text}"></asp:TextBox>
</asp:Panel>
<asp:Panel runat="server" ID="pnlLogic">
    <local:ExpressionsLogicControl runat="server" ID="expressionsLogicControl" Property="{LogicModel.ExpressionsLogic=Model}"></local:ExpressionsLogicControl>
</asp:Panel>
