using System;
using System.Drawing;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class ChangeTranslation : BasePage
    {
        private const String trnIDKey = "trnID";
        private const String trnKeyKey = "trnKey";
        private const String moduleNameKey = "moduleName";
        private const String languagePairKey = "languagePair";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var trnKey = Convert.ToString(RequestUrl[trnKeyKey]);
                var moduleName = Convert.ToString(RequestUrl[moduleNameKey]);
                var languagePair = Convert.ToString(RequestUrl[languagePairKey]);

                var translation = CommonProxy.GetTranslation(moduleName, languagePair, trnKey);
                if (translation == null)
                    return;

                var model = new TranslationModel
                {
                    TrnKey = trnKey,
                    ModuleName = moduleName,
                    LanguagePair = languagePair,
                    DefaultText = translation.DefaultText,
                    TranslatedText = translation.TranslatedText
                };

                translationControl.Model = model;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            var model = translationControl.Model;

            //var contract = new TranslationContract
            //{
            //    TrnKey = model.TrnKey,
            //    DefaultText = model.DefaultText,
            //    TranslatedText = model.TranslatedText
            //};

            //var trnKey = Convert.ToString(RequestUrl[trnKeyKey]);
            //var moduleName = Convert.ToString(RequestUrl[moduleNameKey]);
            //var languagePair = Convert.ToString(RequestUrl[languagePairKey]);

            //CommonProxy.SetTranslation(moduleName, languagePair, trnKey, contract);

            TranslationUtil.SetTranslatedText(model.TrnKey, model.DefaultText, model.LanguagePair, model.TranslatedText);

            SetSuccessMessage("Save successfully");
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
}