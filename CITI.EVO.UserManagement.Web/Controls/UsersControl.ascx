<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsersControl.ascx.cs" Inherits="CITI.EVO.UserManagement.Web.Controls.UsersControl" %>

<div class="col-lg-12">
    <div class="ibox float-e-margins">
        <div class="ibox-title">
            <h5>მომხმარებელთა სია</h5>
            <div class="ibox-tools">
                <a class="collapse-link">
                    <i class="fa fa-chevron-up"></i>
                </a>
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="fa fa-wrench"></i>
                </a>
                <ul class="dropdown-menu dropdown-user">
                    <li><a href="#">Config option 1</a>
                    </li>
                    <li><a href="#">Config option 2</a>
                    </li>
                </ul>
                <a class="close-link">
                    <i class="fa fa-times"></i>
                </a>
            </div>
        </div>
        <div class="ibox-content">

            <div class="table-responsive">
                <ce:ASPxGridView ID="gvUsers" ClientInstanceName="gvUsers" runat="server" KeyFieldName="ID" Width="100%"
                    CssClass="table table-striped table-bordered table-hover" EnableRowsCache="False" Property="{UsersModel.List=DataSource, Mode=Assigne}">

                    <Columns>
                        <dx:GridViewDataColumn VisibleIndex="0" Width="3px" Name="Edit">
                            <DataItemTemplate>
                                    <asp:LinkButton  ID="lnkEdit" runat="server" CssClass="btn btn-primary btn-sm"
                                    ToolTip="რედაქტირება" CommandArgument='<% #Eval("ID")%>' OnCommand="btnEdit_OnCommand" >
                                            <asp:Label runat="server" CssClass="fa fa-edit"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn VisibleIndex="1" Width="3px" Name="View">
                            <DataItemTemplate>
                                   <asp:LinkButton  ID="lnkView" runat="server" CssClass="btn btn-info btn-sm"
                                    ToolTip="ნახვა" CommandArgument='<% #Eval("ID")%>' OnCommand="btnView_OnCommand" >
                                           <asp:Label runat="server" CssClass="fa fa-search"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn VisibleIndex="2" Width="3px" Name="AddMessage">
                            <DataItemTemplate>
                                    <asp:LinkButton  ID="btnNewMessage" runat="server"
                                   CssClass="btn btn-info btn-sm" ToolTip="მესიჯის დამატება"
                                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnNewMessage_OnCommand" >
                                            <asp:Label runat="server" CssClass="fa fa-plus-circle"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn VisibleIndex="3" Width="3px" Name="Delete">
                            <DataItemTemplate>
                                  <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                    ToolTip="წაშლა" OnClientClick="return confirm('გსურთ მომხმარებილის წაშლა?');"
                                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnDelete_OnCommand" >
                                          <asp:Label runat="server" CssClass="fa fa-trash-o"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn VisibleIndex="4" Width="3px" Name="AddAttributes">
                            <DataItemTemplate>
                                    <asp:LinkButton ID="btnSetAttribute" runat="server"
                                    CssClass="btn btn-info btn-sm" ToolTip="ატრიბუტების დამატება"
                                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnSetAttribute_OnCommand" >
                                            <asp:Label runat="server" CssClass="fa fa-plus-circle"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn VisibleIndex="5" Width="3px" Name="ShowAttributes">
                            <DataItemTemplate>
                                <asp:LinkButton ID="btnViewAttributes" runat="server" ToolTip="ატრიბუტების ნახვა" CssClass="btn btn-primary btn-sm"
                                    CommandArgument='<% #Eval("ID")%>' OnCommand="btnViewAttributes_OnCommand">
                                                                <asp:Label runat="server" CssClass="fa fa-search"/>

                                </asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="ID" FieldName="ID" VisibleIndex="6" Visible="false" />
                        <dx:GridViewDataColumn FieldName="LoginName" VisibleIndex="7">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">მომხმარებელი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Password" VisibleIndex="8">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">პაროლი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="სახელი" FieldName="FirstName" VisibleIndex="9">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">სახელი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="LastName" VisibleIndex="10">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">გვარი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="IsActive" VisibleIndex="11">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">აქტივაცია</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Email" VisibleIndex="12">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">მეილი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="Address" VisibleIndex="13">
                            <HeaderCaptionTemplate>
                                <asp:Label runat="server">მისამართი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="PasswordExpire" VisibleIndex="14">
                            <HeaderCaptionTemplate>
                                <asp:Label ID="Label9" runat="server">პაროლის ვალიდურობის თარიღი</asp:Label>
                            </HeaderCaptionTemplate>
                        </dx:GridViewDataColumn>
                    </Columns>
                </ce:ASPxGridView>
            </div>
        </div>
    </div>
</div>



