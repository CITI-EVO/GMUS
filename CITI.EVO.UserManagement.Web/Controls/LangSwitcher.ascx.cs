using System;
using System.Drawing;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Svc.Enums;
using DevExpress.Web;
using MenuItem = DevExpress.Web.MenuItem;

public partial class Controls_LangSwitcher : System.Web.UI.UserControl
{
    private const String trnModeKey = "TrnMode";
    private const String resetCacheKey = "ResetCache";

    protected void Page_PreRender(object sender, EventArgs e)
    {
        var currLang = LanguageUtil.GetLanguage();

        var languages = LanguageUtil.GetLanguages();

        var langItem = mnuLanguages.Items[0];
        langItem.Items.Clear();

        var resetCahceItem = InitResetCacheItem();
        langItem.Items.Add(resetCahceItem);

        var trnModeItem = InitTrnModeItem();
        langItem.Items.Add(trnModeItem);

        foreach (var pair in languages)
        {
            var menuItem = InitNewItem(pair.Key, pair.Value, (currLang == pair.Value));
            langItem.Items.Add(menuItem);
        }
    }

    protected void mnuLanguages_OnItemClick(object source, MenuItemEventArgs e)
    {
        var item = e.Item;
        if (item == null || String.IsNullOrEmpty(item.Name))
        {
            return;
        }

        var pair = item.Name;
        if (String.Equals(pair, trnModeKey, StringComparison.OrdinalIgnoreCase))
        {
            if (!HasPesmission())
            {
                return;
            }

            TranslationUtil.TranslationMode = !TranslationUtil.TranslationMode;
            item.Text = $"Translation Mode -> ({(TranslationUtil.TranslationMode ? "ON" : "OFF")})";

            return;
        }

        if (String.Equals(pair, resetCacheKey, StringComparison.OrdinalIgnoreCase))
        {
            if (!HasPesmission())
            {
                return;
            }

            TranslationUtil.ResetCache();
            return;
        }

        LanguageUtil.SetLanguage(pair);
    }

    private bool HasPesmission()
    {
        var umUtil = UmUtil.Instance;

        if (!umUtil.IsLogged)
        {
            return false;
        }

        var resourcePermission = umUtil.GetResourcePermission("TranslationMode");
        if (resourcePermission == null)
        {
            return false;
        }

        switch (resourcePermission.RuleValue)
        {
            case RulePermissionsEnum.None:
            case RulePermissionsEnum.View:
                return false;
        }

        return true;
    }

    protected MenuItem InitResetCacheItem()
    {
        var resetCacheItem = InitNewItem("Reset Dictionary Cache", resetCacheKey, false);
        resetCacheItem.ItemStyle.BackColor = Color.White;
        resetCacheItem.ItemStyle.ForeColor = Color.Red;
        resetCacheItem.Visible = HasPesmission();

        return resetCacheItem;
    }
    protected MenuItem InitTrnModeItem()
    {
        var trnModeItemText = $"Translation Mode ({(TranslationUtil.TranslationMode ? "ON" : "OFF")})";

        var trnModeItem = InitNewItem(trnModeItemText, trnModeKey, false);
        trnModeItem.ItemStyle.BackColor = Color.White;
        trnModeItem.ItemStyle.ForeColor = (TranslationUtil.TranslationMode ? Color.Green : Color.Red);
        trnModeItem.Visible = HasPesmission();

        return trnModeItem;
    }
    protected MenuItem InitNewItem(String text, String name, bool selected)
    {
        var selectedColor = Color.FromArgb(Convert.ToInt32("EDEDEB", 16));
        var defaultColor = Color.White;

        var menuItem = new MenuItem(text, name);
        menuItem.Selected = selected;
        menuItem.ItemStyle.Height = 25;
        menuItem.ItemStyle.BackColor = (menuItem.Selected ? selectedColor : defaultColor);

        return menuItem;
    }
}