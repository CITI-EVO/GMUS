<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonitoringBudgetItemControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.Monitoring.MonitoringBudgetItemControl" %>

<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{MonitoringItemModel.ID=Value}" />

<fieldset>
    <legend></legend>
    <table align="center" style="border: 1px solid #ccc !important;">
        <tr>
            <td style="padding: 5px;">Budget</td>
            <td style="padding: 5px;">Goal</td>
            <td style="padding: 5px;">Type</td>
            <td style="padding: 5px;">Amount</td>
            <td style="padding: 5px;">Date Of Transfer</td>
        </tr>
        <tr>
            <td style="padding: 5px; width: 160px;">
                <ce:DropDownList runat="server" ID="cbxTasks" placeholder="Budget" DataTextField="Name" DataValueField="ID" Property="{MonitoringItemModel.TaskID=SelectedValue}" CssClass="chosen-select" />
            </td>
            <td style="padding: 5px;">
                <asp:TextBox runat="server" ID="tbxGoal" placeholder="Goal" Property="{MonitoringItemModel.Goal=Text}" CssClass="form-control" />
            </td>
            <td style="padding: 5px;">
                <ce:DropDownList runat="server" ID="cbxType" placeholder="Type" Property="{MonitoringItemModel.Type=SelectedValue}" CssClass="chosen-select">
                    <Items>
                        <asp:ListItem Text="Incoming" Value="Incoming" />
                        <asp:ListItem Text="Outgoing" Value="Outgoing" />
                    </Items>
                </ce:DropDownList>
            </td>
            <td style="padding: 5px;">
                <asp:TextBox runat="server" ID="tbxAmount" placeholder="Amount" Property="{MonitoringItemModel.Amount=Text}" CssClass="decSpinEdit" />
            </td>
            <td style="padding: 5px;">
                <div class="input-group date">
                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                    <asp:TextBox runat="server" ID="tbxDateOfTransfer" placeholder="Date Of Transfer" Property="{MonitoringItemModel.DateOfTransfer=Text}" CssClass="form-control"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
