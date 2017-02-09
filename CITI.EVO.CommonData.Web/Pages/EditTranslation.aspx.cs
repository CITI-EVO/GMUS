using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.CommonData.DAL.Domain;
using CITI.EVO.Tools.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

public partial class Pages_EditTranslation : System.Web.UI.Page
{
    //moduleName={1}&trnKey={2}&languagePair={3}

    private const String trnIDKey = "trnID";
    private const String trnKeyKey = "trnKey";
    private const String moduleNameKey = "moduleName";
    private const String languagePairKey = "languagePair";

    public String TrnKey
    {
        get { return Convert.ToString(ViewState[trnKeyKey]); }
        set { ViewState[trnKeyKey] = value; }
    }

    public String ModuleName
    {
        get { return Convert.ToString(ViewState[moduleNameKey]); }
        set { ViewState[moduleNameKey] = value; }
    }

    public String LanguagePair
    {
        get { return Convert.ToString(ViewState[languagePairKey]); }
        set { ViewState[languagePairKey] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Guid trnID;
            if (Guid.TryParse(Request[trnIDKey], out trnID))
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    var trn = session.Query<CD_Translation>().First(n => n.ID == trnID);

                    TrnKey = trn.TrnKey;
                    ModuleName = trn.ModuleName;
                    LanguagePair = trn.LanguagePair;
                }
            }
            else
            {
                TrnKey = Request[trnKeyKey];
                ModuleName = Request[moduleNameKey];
                LanguagePair = Request[languagePairKey];
            }
        }

        tbKey.Text = TrnKey;
        tbModuleName.Text = ModuleName;
        tbLanguagePair.Text = LanguagePair;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (String.IsNullOrWhiteSpace(TrnKey))
        {
            SetErrorMessage("Invalid TrnKey");
            return;
        }

        if (String.IsNullOrWhiteSpace(ModuleName))
        {
            SetErrorMessage("Invalid ModuleName");
            return;
        }

        if (String.IsNullOrWhiteSpace(LanguagePair))
        {
            SetErrorMessage("Invalid LanguagePair");
            return;
        }

        using (var session = Hb8Factory.CreateSession())
        {
            var trn = (from n in session.Query<CD_Translation>()
                       where n.DateDeleted == null &&
                             n.TrnKey.ToLower() == TrnKey.ToLower() &&
                             n.LanguagePair.ToLower() == LanguagePair.ToLower() &&
                             n.ModuleName.ToLower() == ModuleName.ToLower()
                       select n).Single();

            tbDefaultText.Text = trn.DefaultText;
            tbTranslatedText.Text = trn.TranslatedText;
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        using (var session = Hb8Factory.CreateSession())
        {
            var dbTrn = (from n in session.Query<CD_Translation>()
                         where n.DateDeleted == null &&
                               n.TrnKey.ToLower() == TrnKey.ToLower() &&
                               n.LanguagePair.ToLower() == LanguagePair.ToLower() &&
                               n.ModuleName.ToLower() == ModuleName.ToLower()
                         select n).Single();

            dbTrn.DefaultText = tbDefaultText.Text;
            dbTrn.TranslatedText = tbTranslatedText.Text;

            session.SubmitChanges(dbTrn);

            SetSuccessMessage("Save successfully");
        }
    }

    protected void SetSuccessMessage(String text)
    {
        lblMessage.ForeColor = Color.Green;
        lblMessage.Text = text;
    }

    protected void SetErrorMessage(String text)
    {
        lblMessage.ForeColor = Color.Red;
        lblMessage.Text = text;
    }
}