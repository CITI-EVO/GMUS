<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="cphHead">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cphContent">
    <div class="wrapper wrapper-content animated fadeInRight">
        <div class="row">
            <div class="col-lg-6">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5><span>Personal Details</span></h5>
                    </div>
                    <div class="ibox-content">
                        <div class="row">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">
                                    <span style="color: Red;">*</span>
                                    <span title="This field is mandatory">First name</span>
                                </label>
                                <div class="col-sm-10">
                                    <div class="input-group m-b">

                                        <div class="input-group date">
                                            <span class="input-group-addon"><span class="fa fa-calendar"></span></span>
                                            <input name="ctl00$cphContent$formDataControl$field_934672af94ed49e28caa81975e295a6d" id="field_934672af94ed49e28caa81975e295a6d" class="form-control" type="text">
                                        </div>
                                        <span class="input-group-addon">
                                            <input id="sec_61555fdbdab044fe9f4a4d7667752c38" name="ctl00$cphContent$formDataControl$sec_61555fdbdab044fe9f4a4d7667752c38" type="checkbox">
                                        </span>
                                    </div>
                                    <span id="cphContent_formDataControl_ctl59" style="color: Red; visibility: hidden;">This is a required field</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
