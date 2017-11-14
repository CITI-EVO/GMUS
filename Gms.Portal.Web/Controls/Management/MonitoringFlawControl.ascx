<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringFlawControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.MonitoringFlawControl" %>
<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="ce" TagName="HiddenFieldValueControl" %>


<ce:HiddenFieldValueControl runat="server" ID="hdfldID" Property="{MonitoringFlawModel.ID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <ce:TextBox runat="server" CssClass="form-control" Property="{MonitoringFlawModel.Name=Text}"></ce:TextBox>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="ddlPrintType" CssClass="chosen-select" Property="{MonitoringFlawModel.Type=SelectedValue}">
            <Items>
                <asp:ListItem Text="Budget" Value="Budget" />
                <asp:ListItem Text="Program" Value="Program" />
            </Items>
        </asp:DropDownList>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Score</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxGroupSize" Property="{MonitoringFlawModel.Score=Text}" CssClass="intSpinEdit" />
    </div>
</div>

