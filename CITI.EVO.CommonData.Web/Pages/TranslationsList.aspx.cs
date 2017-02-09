using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.CommonData.DAL.Domain;
using CITI.EVO.Tools.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

public partial class Pages_TranslationsList : System.Web.UI.Page
{
    private const String cacheKey = "translation_dict";
    private const String trnIDKey = "trnID";

    public Guid? TrnID
    {
        get { return ViewState[trnIDKey] as Guid?; }
        set { ViewState[trnIDKey] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        using (var session = Hb8Factory.CreateSession())
        {
            var modules = session.Query<CD_Translation>().Select(n => n.ModuleName).Distinct().ToList();
            var languagePairs = session.Query<CD_Translation>().Select(n => n.LanguagePair).Distinct().ToList();

            modules.Insert(0, "...");
            modules.Sort();

            languagePairs.Insert(0, "...");
            languagePairs.Sort();

            cbxModules.DataSource = modules;
            cbxModules.DataBind();

            cbxLanguagePairs.DataSource = languagePairs;
            cbxLanguagePairs.DataBind();
        }

        FillTranslationsGrid();
    }

    protected void lnkEditTrn_Click(object sender, EventArgs e)
    {
        var control = sender as LinkButton;
        if (control == null)
            return;

        Guid trnID;
        if (!Guid.TryParse(control.CommandArgument, out trnID))
            return;

        TrnID = trnID;

        using (var session = Hb8Factory.CreateSession())
        {
            var dbTrn = (from n in session.Query<CD_Translation>()
                         where n.ID == TrnID
                         select n).FirstOrDefault();

            if (dbTrn == null)
            {
                return;
            }

            tbKey.Text = dbTrn.TrnKey;
            tbModuleName.Text = dbTrn.ModuleName;
            tbLanguagePair.Text = dbTrn.LanguagePair;
            tbDefaultText.Text = dbTrn.DefaultText;
            tbTranslatedText.Text = dbTrn.TranslatedText;

            mpeEdit.Show();
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        if (TrnID == null)
        {
            return;
        }

        using (var session = Hb8Factory.CreateSession())
        {
            var dbTrn = (from n in session.Query<CD_Translation>()
                         where n.ID == TrnID
                         select n).Single();

            //dbTrn.DefaultText = tbDefaultText.Text;
            dbTrn.TranslatedText = tbTranslatedText.Text;
            dbTrn.DateChanged = DateTime.Now;

            session.SubmitChanges(dbTrn);

            var trnsList = Cache[cacheKey] as List<CD_Translation>;
            if (trnsList != null)
            {
                var localTrn = trnsList.FirstOrDefault(n => n.ID == TrnID);
                if (localTrn != null)
                {
                    localTrn.TranslatedText = dbTrn.TranslatedText;
                    localTrn.DateChanged = dbTrn.DateChanged;
                }
            }

            FillTranslationsGrid();

            mpeEdit.Hide();
        }
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        mpeEdit.Hide();
    }

    protected void FillTranslationsGrid()
    {
        using (var session = Hb8Factory.CreateSession())
        {
            var query = from n in session.Query<CD_Translation>()
                        where n.DateDeleted == null
                        select n;

            if (cbxModules.SelectedItem != null && cbxModules.SelectedIndex > 0)
            {
                var selectedModule = Convert.ToString(cbxModules.SelectedItem.Value);

                query = from n in query
                        where n.ModuleName == selectedModule
                        select n;
            }

            if (cbxLanguagePairs.SelectedItem != null && cbxLanguagePairs.SelectedIndex > 0)
            {
                var selectedLangPair = Convert.ToString(cbxLanguagePairs.SelectedItem.Value);

                query = from n in query
                        where n.LanguagePair == selectedLangPair
                        select n;
            }

            gvTrns.DataSource = query;
            gvTrns.DataBind();
        }
    }

    protected void cbxModules_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FillTranslationsGrid();
    }

    protected void cbxLanguagePairs_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FillTranslationsGrid();
    }
}