<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Pages_Report_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>
                        <ce:Label runat="server">Reports</ce:Label>
                    </h5>
                    <div class="ibox-tools">
                    </div>
                </div>
                <div class="ibox-content">
                    <div class="hr-line-dashed"></div>
                    <div class="form-group">
                        <ce:Label runat="server" CssClass="col-lg-2 control-label">Chart Type</ce:Label>
                        <div class="col-lg-10">
                            <ce:DropDownList runat="server" ID="ddlReportTypes"  CssClass="chosen-select" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="lstReportTypes_SelectedIndexChanged"> 
                            </ce:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Chart ID="mainChart" runat="server" IsMapEnabled="True" Width="800" Height="400" AntiAliasing="All" OnDataBound="mainChart_OnDataBound">
                            <Series>
                                <asp:Series Name="Testing" ChartType="Line" YValueType="Int32">
                                    <Points>
                                        <asp:DataPoint AxisLabel="Test 1" YValues="10" />
                                        <asp:DataPoint AxisLabel="Test 2" YValues="20" />

                                        <asp:DataPoint AxisLabel="Test 3" YValues="60" />
                                        <asp:DataPoint AxisLabel="Test 4" YValues="100" />

                                        <asp:DataPoint AxisLabel="Test 5" YValues="15" />
                                        <asp:DataPoint AxisLabel="Test 6" YValues="45" />

                                    </Points>
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="Default">
                                    <AxisX Interval="1" LabelAutoFitStyle="WordWrap">
                                        <MajorGrid LineColor="LightGray"></MajorGrid>
                                    </AxisX>
                                    <AxisY LabelAutoFitStyle="WordWrap">
                                        <MajorGrid LineColor="LightGray"></MajorGrid>
                                    </AxisY>
                                </asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                                <asp:Legend Name="Default" Docking="Bottom" TableStyle="Wide" LegendStyle="Table" TitleAlignment="Near" />
                            </Legends>
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFooter" runat="Server">
</asp:Content>

