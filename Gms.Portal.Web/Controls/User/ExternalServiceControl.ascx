<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExternalServiceControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.User.ExternalServiceControl" %>


<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label" Text="Caption" />
    <div class="col-lg-10">
        <div class="input-group">
            <asp:TextBox runat="server" ID="tbxValue"></asp:TextBox>
            <ce:LinkButton runat="server" ID="btn" OnClick="btnDisplay_Click" CssClass="btn btn-success fa fa-save" />
        </div>
    </div>
</div>
<div>
    <ce:ModalPopup CssClass="modal fade" role="dialog" ID="mpeSearch" runat="server" Style="display: none" DefaultButton="btnSearchCancel">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>
                        <ce:Label runat="server" Text="Warning" />
                    </h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <asp:Panel runat="server" CssClass="form-group" ID="pnlParameters">
                        </asp:Panel>
                        <div class="form-group">
                            <ce:LinkButton runat="server" ID="btnSearch" OnClick="btnSearch_Click" CssClass="btn btn-success fa fa-save" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:ListBox runat="server" ID="lstResults" DataTextField="Text" DataValueField="Value" EnableViewState="False" />
                    </div>
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btnNotifyMessageOK" OnClick="btnSelectOK_Click" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btnNotifyMessageCancel" OnClick="btnSelectCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>
