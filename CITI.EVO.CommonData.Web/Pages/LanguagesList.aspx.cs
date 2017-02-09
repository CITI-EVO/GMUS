using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.CommonData.DAL.Domain;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using NHibernate.Linq;

public partial class Pages_LanguagesList : System.Web.UI.Page
{
    private const String langIDKey = "langID";

    public Guid? LangID
    {
        get { return ViewState[langIDKey] as Guid?; }
        set { ViewState[langIDKey] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        FillLanguagesGrid();

        if (!IsPostBack)
        {
            FillSystemLanguages();
        }
    }

    protected void FillLanguagesGrid()
    {
        using (var session = Hb8Factory.CreateSession())
        {
            var languages = (from n in session.Query<CD_Language>()
                             where n.DateDeleted == null
                             select n).ToList();

            gvLanguages.DataSource = languages;
            gvLanguages.DataBind();
        }
    }

    protected void FillSystemLanguages()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        ddlSystemLanguages.DataSource = cultures;
        ddlSystemLanguages.DataBind();
    }

    protected void lnkEditLang_Click(object sender, EventArgs e)
    {
        lblMessage.Text = String.Empty;

        var control = sender as LinkButton;
        if (control == null)
            return;

        Guid langID;
        if (!Guid.TryParse(control.CommandArgument, out langID))
            return;

        using (var session = Hb8Factory.CreateSession())
        {
            var language = (from n in session.Query<CD_Language>()
                            where n.ID == langID
                            select n).FirstOrDefault();

            if (language == null)
            {
                return;
            }

            LangID = langID;

            tbDisplayName.Text = language.DisplayName;
            tbEngName.Text = language.EngName;
            tbNativeName.Text = language.NativeName;
            tbPair.Text = language.Pair;

            ddlSystemLanguages.SelectedValue = language.Pair;

            mpeAddEdit.Show();
        }
    }

    protected void lnkDeleteLang_Click(object sender, EventArgs e)
    {
        var control = sender as LinkButton;
        if (control == null)
            return;

        Guid langID;
        if (!Guid.TryParse(control.CommandArgument, out langID))
            return;

        using (var session = Hb8Factory.CreateSession())
        {
            var language = (from n in session.Query<CD_Language>()
                            where n.ID == langID
                            select n).FirstOrDefault();

            if (language == null)
                return;

            language.DateDeleted = DateTime.Now;

            session.SubmitChanges(language);
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        lblMessage.Text = String.Empty;

        if (String.IsNullOrWhiteSpace(tbDisplayName.Text))
        {
            lblMessage.Text = "Please enter display name";

            mpeAddEdit.Show();
            return;
        }

        if (String.IsNullOrWhiteSpace(tbPair.Text))
        {
            lblMessage.Text = "Please enter pair";

            mpeAddEdit.Show();
            return;
        }

        using (var session = Hb8Factory.CreateSession())
        {
            CD_Language language;

            if (LangID == null)
            {
                language = new CD_Language();
                language.ID = Guid.NewGuid();
                language.DateCreated = DateTime.Now;
            }
            else
            {
                language = (from n in session.Query<CD_Language>()
                            where n.ID == LangID
                            select n).FirstOrDefault();

                if (language == null)
                    return;
            }

            language.DisplayName = tbDisplayName.Text;
            language.EngName = tbEngName.Text;
            language.NativeName = tbNativeName.Text;
            language.Pair = tbPair.Text;

            session.SubmitChanges(language);

            mpeAddEdit.Hide();
        }

        FillLanguagesGrid();
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        mpeAddEdit.Hide();
    }

    protected void btAddLanguage_OnClick(object sender, EventArgs e)
    {
        LangID = null;

        lblMessage.Text = String.Empty;

        tbDisplayName.Text = String.Empty;
        tbEngName.Text = String.Empty;
        tbNativeName.Text = String.Empty;
        tbPair.Text = String.Empty;

        mpeAddEdit.Show();
    }

    protected void ddlSystemLanguages_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        var culture = cultures.FirstOrDefault(n => n.Name == ddlSystemLanguages.SelectedValue);
        if (culture != null)
        {
            tbDisplayName.Text = culture.DisplayName;
            tbEngName.Text = culture.EnglishName;
            tbNativeName.Text = culture.NativeName;
            tbPair.Text = culture.Name;
        }

        mpeAddEdit.Show();
    }
}