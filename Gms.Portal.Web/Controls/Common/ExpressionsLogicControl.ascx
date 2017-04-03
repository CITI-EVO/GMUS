<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ExpressionsLogicControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Common.ExpressionsLogicControl" %>

<%@ Register Src="~/Controls/Common/NamedExpressionsListControl.ascx" TagPrefix="local" TagName="NamedExpressionsListControl" %>
<%@ Register Src="~/Controls/Common/ExpressionsListControl.ascx" TagPrefix="local" TagName="ExpressionsListControl" %>

<div class="row">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>
                    <ce:Label runat="server" Text="Filters" />
                </h5>
            </div>
            <div class="ibox-content">
                <local:ExpressionsListControl runat="server" ID="filterByControl" Property="{ExpressionsLogicModel.FilterBy=Model}" />
            </div>
        </div>

    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>
                    <ce:Label runat="server" Text="Group By" />
                </h5>
            </div>
            <div class="ibox-content">
                <local:ExpressionsListControl runat="server" ID="groupByControl" Property="{ExpressionsLogicModel.GroupBy=Model}" />
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>
                    <ce:Label runat="server" Text="Order By" />
                </h5>
            </div>
            <div class="ibox-content">
                <local:ExpressionsListControl runat="server" ID="orderByControl" Property="{ExpressionsLogicModel.OrderBy=Model}" />
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>
                    <ce:Label runat="server" Text="Select" />
                </h5>
            </div>
            <div class="ibox-content">
                <local:NamedExpressionsListControl runat="server" ID="selectControl" Property="{ExpressionsLogicModel.Select=Model}" />
            </div>
        </div>
    </div>
</div>

