<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="EventCalendar.aspx.cs" Inherits="Gms.Portal.Web.Pages.User.EventCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="col-lg-8">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>კალენდარი</h5>
                <div class="ibox-tools">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up"></i>
                    </a>
                </div>
            </div>
            <div class="ibox-content">
                <div id="calendar"></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphFooter">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay,listDay,listWeek,listMonth,listYear'
                },
                views: {
                    month: { buttonText: 'Month' },
                    agendaWeek: { buttonText: 'Week' },
                    agendaDay: { buttonText: 'Day' },
                    listDay: { buttonText: 'List day' },
                    listWeek: { buttonText: 'List week' },
                    listMonth: { buttonText: 'List month' },
                    listYear: { buttonText: 'List year' }
                },
                navLinks: true, // can click day/week names to navigate views
                editable: true,
                selectable: true,
                eventLimit: true, // allow "more" link when too many events
                events: {
                    url: '../../Handlers/CalendarData.ashx',
                    error: function () {
                        $('#script-warning').show();
                    }
                },
                loading: function (bool) {
                    $('#loading').toggle(bool);
                },
                dayClick: function (date) {
                    console.log('dayClick', date.format());
                },
                select: function (startDate, endDate) {
                    console.log('select', startDate.format(), endDate.format());
                }
            });
        });
    </script>
</asp:Content>
