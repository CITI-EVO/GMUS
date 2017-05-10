<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EventsList.aspx.cs" Inherits="Gms.Portal.Web.Pages.Management.EventsList" %>

<%@ Register Src="~/Controls/Management/EventsControl.ascx" TagPrefix="local" TagName="EventsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Events</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:LinkButton runat="server" ID="btnNew" ToolTip="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary fa fa-plus" />
                    </div>
                    <div class="form-group">
                        <local:EventsControl runat="server" ID="eventsControl" OnEdit="eventsControl_OnEdit" OnDelete="eventsControl_OnDelete" OnView="eventsControl_OnView" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" Runat="Server">
</asp:Content>

