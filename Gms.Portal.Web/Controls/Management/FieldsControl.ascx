﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FieldsControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.FieldsControl" %>
<div class="table-responsive">
    <div class="dataTables_wrapper form-inline dt-bootstrap">
        <ce:GridView runat="server" ID="gvData" ShowHeaderWhenEmpty="True" UseAccessibleHeader="True" TableSectionHeader="True" AutoGenerateColumns="False" EnableViewState="False"
            CssClass="table table-striped table-bordered table-hover" data-page-size="8" data-filter="#filter" Property="{FieldUnitsModel.List=DataSource, Mode=Assigne}">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnEdit" ToolTip="Edit" CommandArgument='<%# Eval("ID") %>' OnCommand="btnEdit_OnCommand" CssClass="btn btn-primary btn-sm">
                            <asp:Label runat="server" CssClass="fa fa-edit"/>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnDelete" ToolTip="Delete" CommandArgument='<%# Eval("ID") %>' OnCommand="btnDelete_OnCommand" CssClass="btn btn-danger btn-sm">
                            <asp:Label runat="server" CssClass="fa fa-trash-o"/>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <ce:Label runat="server" Text="Name" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </ce:GridView>
    </div>
</div>
