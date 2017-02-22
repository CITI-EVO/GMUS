<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormControl" %>

<%@ Register TagPrefix="local" TagName="ElementControl" Src="~/Controls/Management/ElementControl.ascx" %>
<%@ Register TagPrefix="local" TagName="ElementsControl" Src="~/Controls/Management/ElementsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{FormModel.ID=Value}" />
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{FormModel.Name=Text}" CssClass="form-control" />
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Number</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxNumber" Property="{FormModel.Number=Text}" CssClass="form-control" />
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
    <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" CssClass="btn btn-primary">
        <asp:Label runat="server" CssClass="fa fa-plus"/>
    </ce:LinkButton>
</div>
<div class="form-group">
    <local:ElementsControl runat="server" ID="elementsControl"
        OnNew="elementsControl_OnNew"
        OnEdit="elementsControl_OnEdit"
        OnDelete="elementsControl_OnDelete" />
</div>
<div>
    <asp:Panel ID="pnlElement" runat="server" Style="display: none" DefaultButton="btElementOK">
        <asp:Button ID="btnElementFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeElement" runat="server" PopupControlID="pnlElement" BackgroundCssClass="modalBackground" TargetControlID="btnElementFake" />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="ibox float-e-margins">
                            <div class="ibox-title">
                                <h5>Element</h5>
                            </div>
                            <div class="ibox-content">
                                <div class="form-group">
                                    <ce:Label ID="lblFormElement" runat="server" ForeColor="Red"></ce:Label>
                                </div>
                                <local:ElementControl runat="server" ID="elementControl" OnDataChanged="elementControl_OnDataChanged" />
                                <div class="form-group">
                                    <asp:LinkButton runat="server" ID="btElementOK" OnClick="btnElementOK_Click" CssClass="btn btn-success">
                                        <asp:Label runat="server" CssClass="fa fa-save"/>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btElementCancel" CssClass="btn btn-warning">
                                        <asp:Label runat="server" CssClass="fa fa-close"/>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
