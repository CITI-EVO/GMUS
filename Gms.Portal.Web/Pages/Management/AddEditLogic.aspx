<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditLogic.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.AddEditLogic" %>

<%@ Register Src="~/Controls/Management/LogicControl.ascx" TagPrefix="local" TagName="LogicControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="col-lg-12">
                            <ce:LinkButton runat="server" ID="btnSave" ToolTip="Save" OnClick="btnSave_OnClick" CssClass="btn btn-success fa fa-save" />
                            <ce:LinkButton runat="server" ID="btnCancel" ToolTip="Close" OnClick="btnCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                        </div>
                    </div>
                    <local:LogicControl runat="server" ID="logicControl" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <ce:ModalPopup CssClass="modal fade" role="dialog" runat="server" ID="mpePreview">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h5>
                            <ce:Label runat="server" Text="Data" />
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <asp:Label ID="lblFormElement" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="form-group">
                            <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="True" Width="100%" EnableViewState="False">
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <ce:LinkButton runat="server" ID="btnClose" OnClick="btnClose_OnClick" CssClass="btn btn-warning fa fa-close" />
                    </div>
                </div>
            </div>
        </ce:ModalPopup>
    </div>
</asp:Content>

