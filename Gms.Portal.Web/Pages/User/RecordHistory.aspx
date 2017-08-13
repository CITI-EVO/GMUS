<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecordHistory.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.RecordHistory" %>

<%@ Register TagPrefix="local" TagName="RecordHistoryControl" Src="~/Controls/User/RecordHistoryControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:RecordHistoryControl runat="server" ID="recordHistoryControl" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
</asp:Content>

