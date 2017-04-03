<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TranslationControl.ascx.cs" Inherits="Gms.Portal.Web.Controls.Management.TranslationControl" %>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 font-normal" Text="Module Name:" />
    <div class="col-lg-12">
        <asp:TextBox runat="server" ID="tbModuleName" ReadOnly="True" Property="{TranslationModel.ModuleName=Text}" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 font-normal" Text="Translation Key:" />
    <div class="col-lg-12">
        <asp:TextBox runat="server" ID="tbKey" ReadOnly="True" Property="{TranslationModel.TrnKey=Text}" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 font-normal" Text="Language Pair:" />
    <div class="col-lg-12">
        <asp:TextBox runat="server" ID="tbLanguagePair" ReadOnly="True" Property="{TranslationModel.LanguagePair=Text}" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 font-normal" Text="Default Text:" />
    <div class="col-lg-12">
        <asp:TextBox runat="server" ID="tbDefaultText" TextMode="MultiLine" Property="{TranslationModel.DefaultText=Text}" Rows="50" Columns="150" Height="70px" Width="680px" ReadOnly="True" CssClass="form-control" />
    </div>
</div>
<div class="form-group">
    <ce:Label runat="server" CssClass="col-sm-2 font-normal" Text="Translated Text:" />
    <div class="col-lg-12">
        <asp:TextBox runat="server" ID="tbTranslatedText" TextMode="MultiLine" Property="{TranslationModel.TranslatedText=Text}" Rows="50" Columns="150" Height="70px" Width="680px" CssClass="form-control" />
    </div>
</div>
