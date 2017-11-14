<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringPrintFieldControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.MonitoringPrintFieldControl" %>
<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="ce" TagName="HiddenFieldValueControl" %>


<ce:HiddenFieldValueControl runat="server" ID="hdfldID" Property="{MonitoringPrintFieldsModel.ID=Value}" />
<ce:HiddenFieldValueControl runat="server" ID="hdfldFormID" Property="{MonitoringPrintFieldsModel.FormID=Value}" />

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <ce:TextBox runat="server" CssClass="form-control" Property="{MonitoringPrintFieldsModel.Name=Text}"></ce:TextBox>
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Print Type</ce:Label>
    <div class="col-lg-10">
        <asp:DropDownList runat="server" ID="ddlPrintType" CssClass="chosen-select" Property="{MonitoringPrintFieldsModel.PrintType=SelectedValue}" AutoPostBack="True">
            <Items>
                <asp:ListItem Text="PDF" Value="PDF" />
                <asp:ListItem Text="Excel" Value="Excel" />
            </Items>
        </asp:DropDownList>
    </div>
</div>

<asp:Panel runat="server" ID="pnlExcel">

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Budget</ce:Label>
    <div class="col-lg-10">
       <asp:CheckBox runat="server" ID="chBoxBudget" Property="{MonitoringPrintFieldsModel.BudgetForm=Checked}"/>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Summary Budget</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="CheckBox1" Property="{MonitoringPrintFieldsModel.SummaryBudgetForm=Checked}"/>
    </div>
</div>

<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Projects</ce:Label>
    <div class="col-lg-10">
        <asp:CheckBox runat="server" ID="CheckBox2" Property="{MonitoringPrintFieldsModel.ProjectsForm=Checked}"/>
    </div>
</div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlTemplate">
    <div class="form-group">
        <ce:Label runat="server" CssClass="col-lg-2 control-label">Template</ce:Label>
        <div class="col-lg-10">
            <ce:TextBox runat="server" ID="tbxTemplate" TextMode="MultiLine" Rows="6" CssClass="form-control" Property="{MonitoringPrintFieldsModel.Template=Text}"/>
        </div>
    </div>    
    <div class="form-group">
        <ce:Label runat="server" CssClass="col-lg-2 control-label">Landscape</ce:Label>
        <div class="col-lg-10">
            <ce:CheckBox runat="server" ID="chBoxLendscape" Property="{MonitoringPrintFieldsModel.IsLendscape=Checked}"/>
        </div>
    </div>    
</asp:Panel>