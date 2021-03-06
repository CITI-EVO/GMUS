﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Common;

namespace CITI.EVO.Tools.Helpers
{
    public class DefaultTranslatable : ITranslatable
    {
        public DefaultTranslatable()
        {
        }

        public DefaultTranslatable(String text) : this(text, true)
        {
        }

        public DefaultTranslatable(String text, bool translate) : this(CryptographyUtil.ComputeMD5(text), text, translate)
        {
        }

        public DefaultTranslatable(String trnKey, String text) : this(trnKey, text, true)
        {
        }

        public DefaultTranslatable(String trnKey, String text, bool translate) : this(null, trnKey, text, translate)
        {
        }

        public DefaultTranslatable(StateBag viewState, String trnKey, String text) : this(viewState, trnKey, text, true)
        {
        }

        public DefaultTranslatable(StateBag viewState, String trnKey, String text, bool translate)
        {
            ViewState = viewState;
            TrnKey = trnKey;
            Text = text;

            Translated = translate;

            if (translate)
                TranslationUtil.ApplyTranslation(this);
        }

        #region Implementation of ITranslatable

        public bool Translated { get; private set; }

        public StateBag ViewState { get; private set; }

        public String TrnKey { get; set; }

        public String Text { get; set; }

        public String Link { get; set; }

        #endregion
    }

}
