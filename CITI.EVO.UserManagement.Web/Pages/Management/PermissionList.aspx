<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="PermissionList.aspx.cs" Inherits="CITI.EVO.UserManagement.Web.Pages.Management.PermissionList" %>

<%@ Register TagPrefix="mis" Namespace="CITI.EVO.Tools.Web.UI.Controls" Assembly="CITI.EVO.Tools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>პროექტი/მოდული</h5>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <ce:DropDownList ID="cmbProject" DataTextField="Name" DataValueField="ID" CssClass="chosen-select" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbProject_SelectedIndexChanged">
                        </ce:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-6">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>მომხმარებლის ჯგუფები</h5>
                </div>
                <div class="ibox-content">
                    <div class="row">
                        <ce:ASPxTreeList ID="tlGroups" runat="server" AutoGenerateColumns="False" Width="100%"
                            KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID"
                            ViewStateMode="Disabled" OnFocusedNodeChanged="tlGroups_FocusedNodeChanged"
                            EnableCallbacks="False" DataCacheMode="Disabled">
                            <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Horizontal" ShowTreeLines="false" />
                            <%--<SettingsSelection Enabled="True" />--%>
                            <%--<SettingsBehavior ProcessSelectionChangedOnServer="True" />--%>
                            <SettingsBehavior ProcessFocusedNodeChangedOnServer="True" />
                            <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowSort="false" AllowFocusedNode="true" />
                            <SettingsEditing Mode="EditFormAndDisplayNode" />
                            <Styles>
                                <AlternatingNode Enabled="true" />
                                <Cell HorizontalAlign="Left" VerticalAlign="Middle" />
                                <Header HorizontalAlign="Center">
                                    <border bordercolor="#D7D7D7" />
                                </Header>
                            </Styles>
                            <Columns>
                                <dx:TreeListTextColumn VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <asp:Label runat="server" Text="სახელი" />
                                    </HeaderCaptionTemplate>
                                    <DataCellTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Name") %>' />
                                    </DataCellTemplate>
                                </dx:TreeListTextColumn>
                            </Columns>
                            <Styles>
                                <Header BackColor="#E9E9E9">
                                </Header>
                            </Styles>
                            <Border BorderColor="#D7D7D7" />
                        </ce:ASPxTreeList>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>რესურსები</h5>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:TextBox runat="server" ID="tbxKeyword" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">
                                <asp:LinkButton ID="btnSearch" runat="server" ToolTip="ძებნა" CssClass="btn btn-primary btn-sm fa fa-search" />
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <ce:ASPxTreeList ID="tlResources" runat="server" AutoGenerateColumns="False" Width="100%"
                            KeyFieldName="ID" ParentFieldName="ParentID" ClientIDMode="AutoID" ViewStateMode="Disabled"
                            OnFocusedNodeChanged="tlResources_FocusedNodeChanged" EnableCallbacks="false"
                            DataCacheMode="Disabled">
                            <Settings ShowGroupFooter="false" ShowFooter="false" GridLines="Horizontal" ShowTreeLines="false" />
                            <SettingsBehavior ExpandCollapseAction="NodeDblClick"
                                AllowSort="false"
                                AllowFocusedNode="true"
                                ProcessFocusedNodeChangedOnServer="true" />
                            <SettingsEditing Mode="EditFormAndDisplayNode" />
                            <Styles>
                                <AlternatingNode Enabled="true" />
                                <Cell HorizontalAlign="Left" VerticalAlign="Middle" />
                                <Header HorizontalAlign="Center">
                                    <border bordercolor="#D7D7D7" />
                                </Header>
                            </Styles>
                            <Columns>
                                <dx:TreeListTextColumn VisibleIndex="3">
                                    <HeaderCaptionTemplate>
                                        <asp:Label runat="server" Text="სახელი" />
                                    </HeaderCaptionTemplate>
                                    <DataCellTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Name") %>' />
                                    </DataCellTemplate>
                                </dx:TreeListTextColumn>
                                <dx:TreeListTextColumn VisibleIndex="4">
                                    <HeaderCaptionTemplate>
                                        <asp:Label runat="server" Text="მნიშვნელობა" />
                                    </HeaderCaptionTemplate>
                                    <DataCellTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Value") %>' />
                                    </DataCellTemplate>
                                </dx:TreeListTextColumn>
                                <dx:TreeListTextColumn VisibleIndex="5">
                                    <HeaderCaptionTemplate>
                                        <asp:Label runat="server" Text="აღწერილობა" />
                                    </HeaderCaptionTemplate>
                                    <DataCellTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Description") %>' />
                                    </DataCellTemplate>
                                </dx:TreeListTextColumn>
                                <dx:TreeListTextColumn VisibleIndex="6">
                                    <HeaderCaptionTemplate>
                                        <asp:Label runat="server" Text="ტიპი" />
                                    </HeaderCaptionTemplate>
                                    <DataCellTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Type") %>' />
                                    </DataCellTemplate>
                                </dx:TreeListTextColumn>
                            </Columns>
                            <Styles>
                                <Header BackColor="#E9E9E9">
                                </Header>
                            </Styles>
                            <Border BorderColor="#D7D7D7" />
                        </ce:ASPxTreeList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>უფლებების დაწესება</h5>
                </div>
                <div class="ibox-content">
                    <div class="form-group">
                        <div>
                            <ce:CheckBoxList ID="chklRules" Font-Size="12px" runat="server" TextAlign="Left" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">ნახვა</asp:ListItem>
                                <asp:ListItem Value="1">დამატება</asp:ListItem>
                                <asp:ListItem Value="2">რედაქტირება</asp:ListItem>
                                <asp:ListItem Value="3">წაშლა</asp:ListItem>
                            </ce:CheckBoxList>
                            <asp:Label runat="server" ID="lbPermissionInfo" />
                            <ce:LinkButton ID="btnSet" runat="server" CssClass="btn btn-info btn-sm fa fa-check" ToolTip="დაწესება" OnClick="btnSet_Click" />
                            <ce:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-danger btn-sm fa fa-trash-o" ToolTip="მოხსნა" OnClick="btnRemove_Click" />
                            <ce:LinkButton ID="btPermissionParameter" runat="server" CssClass="btn btn-info btn-sm fa fa-plus" Text="დამატებითი ატრიბუტები" ToolTip="დაწესება" OnClick="btPermissionParameter_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="PanelPermissionParameters" runat="server" Style="display: none" DefaultButton="btPermissionParameter">
        <asp:Button ID="btPermissionParameterPopup" runat="server" Style="display: none;" />
        <act:ModalPopupExtender ID="mpePermissionParametersForm" runat="server" PopupControlID="PanelPermissionParameters"
            BackgroundCssClass="modalBackground" TargetControlID="btPermissionParameterPopup" />

        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row ">
                        <p>დამატებითი ატრიბუტები</p>
                        <div class="ibox-content form-horizontal">
                            <div class="col-sm-12">

                                <asp:HiddenField runat="server" ID="hdPermissionID" />
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">სახელი</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbName" runat="server" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">მნიშვნელობა</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="tbValue" runat="server" />

                                    </div>
                                </div>




                                <div style="width: 100%; padding: 0px; margin: 0px; float: left;">
                                    <ce:ASPxGridView ID="gvPermissionParameters" Width="100%" runat="server">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="Name" VisibleIndex="0">
                                                <HeaderCaptionTemplate>
                                                    <asp:Label runat="server">სახელი</asp:Label>
                                                </HeaderCaptionTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Value" VisibleIndex="1">
                                                <HeaderCaptionTemplate>
                                                    <asp:Label runat="server">მნიშვნელიბა</asp:Label>
                                                </HeaderCaptionTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn VisibleIndex="1" Width="3px">
                                                <DataItemTemplate>
                                                    <ce:LinkButton ID="lnkDelete" runat="server" DefaultImageUrl="~/App_Themes/default/images/delete.png"
                                                        ToolTip="წაშლა" OnClientClick="return confirm('წავშალოთ?');" CommandArgument='<% #Eval("ID")%>'
                                                        OnClick="lnkDelete_Click" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                        </Columns>
                                    </ce:ASPxGridView>
                                </div>
                                <div style="text-align: left; padding: 13px 0 2px 3px; float: left;">
                                    <ce:LinkButton ID="btPermissionParameterOK" CssClass="btn btn-success fa fa-floppy-o" ToolTip="შენახვა" runat="server" OnClick="btPermissionParameterOK_Click" />
                                </div>
                                <div style="text-align: left; padding: 13px 0 2px 15px; float: left;">
                                    <ce:LinkButton ID="btProjectCancel" CssClass="btn btn-warning  fa fa-times" ToolTip="დახურვა" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
</asp:Content>
