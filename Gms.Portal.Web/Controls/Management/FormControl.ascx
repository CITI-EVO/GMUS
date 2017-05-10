<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FormControl" %>

<%@ Register Src="~/Controls/Management/ElementControl.ascx" TagPrefix="local" TagName="ElementControl" %>
<%@ Register Src="~/Controls/Management/ElementsControl.ascx" TagPrefix="local" TagName="ElementsControl" %>
<%@ Register Src="~/Controls/Management/ElementMoveControl.ascx" TagPrefix="local" TagName="ElementMoveControl" %>
<%@ Register Src="~/Controls/Management/ElementPasteControl.ascx" TagPrefix="local" TagName="ElementPasteControl" %>
<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>


<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{FormModel.ID=Value}" />
   

<div class="ibox float-e-margins">
    <div class="ibox-title" style="background-color: #4d608a;">
        <h5 style="color: white;">
            <ce:Label runat="server">Form Fields</ce:Label>
        </h5>
        <div class="ibox-tools">
            <a class="collapse-link">
                <i class="fa fa-chevron-up" style="color: white;"></i>
            </a>
        </div>
    </div>
    <div class="ibox-content" style="display: none;">
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
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:LinkButton runat="server" ID="btnNew" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
</div>

<div class="form-group">
    <local:ElementsControl runat="server" ID="elementsControl"
        OnNew="elementsControl_OnNew"
        OnEdit="elementsControl_OnEdit"
        OnDelete="elementsControl_OnDelete" 
        OnCopy="elementsControl_Copy"
        OnPaste="elementsControl_Paste"
        OnMove="elementsControl_OnMove" />
</div>

<div>
    <asp:Panel ID="pnlElement" runat="server" Style="display: none" DefaultButton="btElementOK">
        <asp:Button ID="btnElementFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeElement" runat="server" PopupControlID="pnlElement" BackgroundCssClass="modalBackground" TargetControlID="btnElementFake" />
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <h5>
                            <ce:Label runat="server" Text="Element" />
                        </h5>
                    </div>
                    <div class="ibox-content">
                        <div class="form-group">
                            <ce:Label ID="lblFormElement" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div class="form-group">
                            <local:ElementControl runat="server" ID="elementControl" OnDataChanged="elementControl_OnDataChanged" />
                        </div>
                        <div class="form-group">
                            <ce:LinkButton runat="server" ID="btElementOK" OnClick="btnElementOK_Click" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ID="btElementCancel" OnClick="btElementCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

<div>
    <asp:Panel ID="pnlElementMove" runat="server" Style="display: none" DefaultButton="btElementOK">
        <asp:Button ID="btnElementMoveFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeElementMove" runat="server" PopupControlID="pnlElementMove" BackgroundCssClass="modalBackground" TargetControlID="btnElementMoveFake"  CancelControlID="btnElementMoveCancel"/>
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <h5>
                            <ce:Label runat="server" Text="Element" />
                        </h5>
                    </div>
                    <div class="ibox-content">
                        <div class="form-group">
                            <ce:Label ID="lblElementMoveElement" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div class="form-group">
                            <local:ElementMoveControl runat="server" ID="elementMoveControl" />
                        </div>
                        <div class="form-group">
                            <ce:LinkButton runat="server" ID="btnElementMoveOK" OnClick="btnElementMoveOK_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ID="btnElementMoveCancel" OnClick="btnElementMoveCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

<div>
    <asp:Panel ID="pnlElementPaste" runat="server" Style="display: none" DefaultButton="btElementOK">
        <asp:Button ID="btnElementPasteFake" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpeElementPaste" runat="server" PopupControlID="pnlElementPaste" BackgroundCssClass="modalBackground" TargetControlID="btnElementPasteFake"  CancelControlID="btnElementPasteCancel"/>
        <div class="modal-dialog" style="width: 900px;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <h5>
                            <ce:Label runat="server" Text="Element" />
                        </h5>
                    </div>
                    <div class="ibox-content">
                        <div class="form-group">
                            <ce:Label ID="lblPasteResult" runat="server" ForeColor="Red"></ce:Label>
                        </div>
                        <div class="form-group">
                            <local:ElementPasteControl runat="server" ID="elementPasteControl" OnDataChanged="elementPasteControl_OnDataChanged" />
                        </div>
                        <div class="form-group">
                            <ce:LinkButton runat="server" ID="btnElementPasteOK" OnClick="btnElementPasteOK_Click" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ID="btnElementPasteCancel" OnClick="btnElementPasteCancel_Click" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
