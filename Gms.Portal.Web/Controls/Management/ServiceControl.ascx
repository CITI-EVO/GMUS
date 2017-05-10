<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ServiceControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.ServiceControl" %>

<%@ Register Src="~/Controls/Common/HiddenFieldValueControl.ascx" TagPrefix="local" TagName="HiddenFieldValueControl" %>


<local:HiddenFieldValueControl runat="server" ID="hdID" Property="{ServiceModel.ID=Value}" />

<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Name</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxName" Property="{ServiceModel.Name=Text}" CssClass="form-control" />
    </div>
</div>
<div class="hr-line-dashed"></div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-lg-2 control-label">Url</ce:Label>
    <div class="col-lg-10">
        <asp:TextBox runat="server" ID="tbxUrl" Property="{ServiceModel.Url=Text}" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:ASPxTreeList ID="tlData" runat="server" EnableViewState="False" AutoGenerateColumns="False"
        KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" Width="100%" ViewStateMode="Disabled" >
        <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Both" ShowTreeLines="True" />
        <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="True" AllowFocusedNode="true" />
        <SettingsEditing Mode="EditFormAndDisplayNode" />
        <SettingsPager Position="Bottom" PageSize="25">
            <Summary Text="{0} გვერდი {1}-დან (სულ {2})"></Summary>
            <PageSizeItemSettings Items="25, 50, 100, 200" Visible="True" Caption="ჩანაწერების რაოდენობა" />
        </SettingsPager>
        <SettingsLoadingPanel Text="მიმდინარეობს მონაცემების დამუშავება&amp;hellip;" />
        <Styles>
            <Header ForeColor="#5D5D5D" Wrap="true" HorizontalAlign="Center">
                <border bordercolor="#F7F7F7" borderstyle="Solid"></border>
            </Header>
            <AlternatingNode Enabled="true" />
            <FocusedNode BackColor="#d7d7d7" ForeColor="#003399" />
            <Cell HorizontalAlign="Left" VerticalAlign="Middle" Border-BorderColor="#cfcfcf" Border-BorderWidth="1px">
                <border bordercolor="#CFCFCF" borderwidth="1px" />
            </Cell>
            <Header HorizontalAlign="Center" />
        </Styles>
        <Columns>
            <dx:TreeListTextColumn VisibleIndex="1">
                <HeaderCaptionTemplate>
                    <ce:Label runat="server" Text="Name" />
                </HeaderCaptionTemplate>
                <DataCellTemplate>
                    <div>
                        <span runat="server" class='<%# GetImageClass(Eval("Tag")) %>'></span>
                        <span style="padding: 2px; max-width: 500px;"><%#Eval("Name") %></span>
                    </div>
                </DataCellTemplate>
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn VisibleIndex="2">
                <HeaderCaptionTemplate>
                    <ce:Label runat="server" Text="Type" />
                </HeaderCaptionTemplate>
                <DataCellTemplate>
                    <ce:Label runat="server" Text='<%#Eval("Tag") %>' />
                </DataCellTemplate>
            </dx:TreeListTextColumn>
        </Columns>
    </ce:ASPxTreeList>
</div>
