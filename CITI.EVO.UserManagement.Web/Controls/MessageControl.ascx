<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageControl.ascx.cs" Inherits="Controls_MessageControl" %>
<%@ Register TagPrefix="mis" Namespace="CITI.EVO.Tools.Web.UI.Controls" Assembly="CITI.EVO.Tools" %>

<asp:Panel ID="pnlMessage" runat="server" Style="display: none;" CssClass="modalWindow"
    DefaultButton="btMessageOK">
    <asp:UpdatePanel ID="upnlMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btMessagePopup" runat="server" Style="display: none;" />
            <act:ModalPopupExtender ID="mpeMessage" TargetControlID="btMessagePopup" PopupControlID="pnlMessage"
                runat="server" Enabled="True" BackgroundCssClass="modalBackground" />
            <div class="popup">
                <div class="popup_fieldset">
                    <h2>
                        <asp:Label Text="მესიჯის დამატება" runat="server"></asp:Label></h2>
                    <div class="title_separator"></div>
                    <asp:Label ID="lbMessageError" runat="server" ForeColor="Red"></asp:Label>
                    <div class="box">
                        <h3>
                            <asp:Label ID="Label1" runat="server">სათაური</asp:Label></h3>
                        <div class="box_body_short">
                            <dx:ASPxComboBox ID="ddlSubjects" runat="server"
                                TextField="Subject" ValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged" />

                        </div>
                    </div>
                    <div class="box">
                        <h3>
                            <asp:Label ID="Label2" runat="server">სათაური</asp:Label>
                        </h3>
                        <div class="box_body">
                            <asp:TextBox ID="tbxSubject" runat="server" />
                        </div>
                    </div>
                    <div class="box">
                        <h3>
                            <asp:Label ID="Label5" runat="server">მესიჯი</asp:Label>
                        </h3>
                        <div class="box_body">
                            <asp:TextBox ID="tbxMessage" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="box">
                        <h3>
                            <asp:Label ID="Label6" runat="server">მესიჯის ტიპი</asp:Label>
                        </h3>
                        <div class="box_body_short">
                            <ce:ASPxComboBox runat="server" ID="ddlMessageType">
                                <Items>
                                    <dx:ListEditItem Text="სტანდარტული" Value="0" Selected="True" />
                                    <dx:ListEditItem Text="ყველა" Value="1" />
                                </Items>
                            </ce:ASPxComboBox>
                        </div>
                    </div>
                </div>
                <div class="fieldsetforicons">
                    <div class="left">
                        <asp:LinkButton ID="btMessageOK" runat="server" Text="შენახვა" CssClass="icon" ToolTip="შენახვა"
                            OnClick="btMessageOK_Click" />


                    </div>
                    <div class="left">
                        <asp:LinkButton ID="btDelete" CssClass="icon" runat="server" Text="წაშლა" ToolTip="წაშლა" Enabled="False"
                            OnClick="btDelete_Click" />
                    </div>
                    <div class="right">


                        <asp:LinkButton ID="btClose" CssClass="icon" runat="server" Text="დახურვა" ToolTip="დახურვა" />
                    </div>
                </div>
            </div>




















        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
