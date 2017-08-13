<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecordUsersGird.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.RecordUsersGird" %>

<%@ Register TagPrefix="local" TagName="RecordUsersGridControl" Src="~/Controls/User/RecordUsersGridControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div>
        <div class="ibox float-e-margins">
            <div class="ibox float-e-margins">
                <div class="ibox-content">
                    <div class="form-group">
                        <local:RecordUsersGridControl runat="server" ID="recordUsersGridControl" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
</asp:Content>

