<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RegEventControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.RegEventControl" %>

<%@ Register TagPrefix="local" TagName="PhaseControl" Src="~/Controls/Management/PhaseControl.ascx" %>
<%@ Register TagPrefix="local" TagName="PhasesControl" Src="~/Controls/Management/PhasesControl.ascx" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{EventModel.ID=Value}" />

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{EventModel.Name=Text}" CssClass="form-control" />
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
</div>
<div class="form-group">
    <local:PhasesControl runat="server" ID="phasesControl" OnEdit="phasesControl_OnEdit" OnDelete="phasesControl_OnDelete" />
</div>
<div>
    <asp:Panel ID="pnlPhase" runat="server" Style="display: none" DefaultButton="btPhaseOK">
        <asp:Button ID="btnPhaseFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpePhase" runat="server" PopupControlID="pnlPhase" BackgroundCssClass="modalBackground" TargetControlID="btnPhaseFake" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="ibox float-e-margins">
                            <div class="ibox-title">
                                <h5>Phase</h5>
                            </div>
                            <div class="ibox-content">
                                <div class="form-group">
                                    <asp:Label ID="lblFormPhase" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                <local:PhaseControl runat="server" ID="phaseControl" OnDataChanged="phaseControl_OnDataChanged" />
                                <div class="form-group">
                                    <ce:LinkButton runat="server" ID="btPhaseOK" ToolTip="Save" OnClick="btnPhaseOK_Click" CssClass="btn btn-success fa fa-save" />
                                    <ce:LinkButton runat="server" ID="btPhaseCancel" ToolTip="Close" OnClick="btPhaseCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
