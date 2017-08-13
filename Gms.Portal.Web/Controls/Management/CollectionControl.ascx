<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CollectionControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.CollectionControl" %>

<%@ Register TagPrefix="local" TagName="FieldsControl" Src="~/Controls/Management/FieldsControl.ascx" %>
<%@ Register TagPrefix="local" TagName="NamedControl" Src="~/Controls/Common/NamedControl.ascx" %>
<%@ Register TagPrefix="local" TagName="HiddenFieldValueControl" Src="~/Controls/Common/HiddenFieldValueControl.ascx" %>

<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{CollectionModel.ID=Value}" />
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{CollectionModel.Name=Text}" CssClass="form-control" />
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
</div>
<div class="form-group">
    <local:FieldsControl runat="server" ID="fieldsControl" OnEdit="fieldsControl_OnEdit" OnDelete="fieldsControl_OnDelete" />
</div>
<div>
    <ce:ModalPopup runat="server" ID="mpeField" CssClass="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5>Field</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <asp:Label ID="lblFormField" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <local:NamedControl runat="server" ID="namedControl" OnDataChanged="namedControl_OnDataChanged" />
                </div>
                <div class="modal-footer">
                    <ce:LinkButton runat="server" ID="btFieldOK" ToolTip="Save" OnClick="btnFieldOK_Click" CssClass="btn btn-success fa fa-save" />
                    <ce:LinkButton runat="server" ID="btFieldCancel" ToolTip="Close" OnClick="btFieldCancel_OnClick" CssClass="btn btn-warning fa fa-close" />
                </div>
            </div>
        </div>
    </ce:ModalPopup>
</div>
