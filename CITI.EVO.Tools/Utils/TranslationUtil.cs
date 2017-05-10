using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Web.UI.Common;

namespace CITI.EVO.Tools.Utils
{
    public class TranslationUtil
    {

        private const String TrnKeyKey = "TrnKey";
        private const String DefaultTextKey = "DefaultText";
        private const String TrnEditPageKey = "TrnEditPage";

        private const String TrnCacheKey = "$[TranslationUtil_Translations]";

        public const String RequestTranslationModeKey = "translationMode";

        private const String SessionTranslationModeKey = "TranslationUtil_translationMode";

        private const String ContextEnableTranslationKey = "TranslationUtil_enableTranslation";

        private const String EditLinkFormat = @"[<a target='_blank' href='{0}?moduleName={1}&trnKey={2}&languagePair={3}'>TRN</a>]";

        public static bool TranslationMode
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var urlHelper = HttpServerUtil.RequestUrl;
                    return (context.Session[SessionTranslationModeKey] != null || urlHelper[RequestTranslationModeKey] != null);
                }

                return false;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    if (value)
                        context.Session[SessionTranslationModeKey] = "ON";
                    else
                        context.Session[SessionTranslationModeKey] = null;
                }
            }
        }

        public static bool EnableTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                var context = HttpContext.Current;

                var value = DataConverter.ToNullableBool(context.Items[ContextEnableTranslationKey]);
                if (value == null)
                {
                    value = DataConverter.ToNullableBool(ConfigurationManager.AppSettings["EnableTranslations"]);
                    value = value.GetValueOrDefault(true);

                    context.Items[ContextEnableTranslationKey] = value;
                }

                return value.GetValueOrDefault();
            }
        }

        public static void ResetCache()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var cache = context.Cache;
                cache.Remove(TrnCacheKey);
            }
        }

        public static String GetTranslatedText(String defaultText)
        {
            var trnKey = CryptographyUtil.ComputeMD5(defaultText);
            return GetTranslatedText(trnKey, defaultText);
        }

        public static String GetTranslatedText(String trnKey, String defaultText)
        {
            var languagePair = Thread.CurrentThread.CurrentCulture.Name;
            return GetTranslatedText(trnKey, defaultText, languagePair);
        }

        public static String GetTranslatedText(String trnKey, String defaultText, String languagePair)
        {
            if (!EnableTranslation)
            {
                return defaultText;
            }

            if (!String.IsNullOrWhiteSpace(trnKey))
            {
                var moduleName = PermissionUtil.ModuleName;

                String trnText;
                if (!TranslationMode)
                {
                    var cacheKey = $"{moduleName}|{trnKey}|{languagePair}";

                    trnText = GetCacheTranslation(cacheKey);

                    if (String.IsNullOrWhiteSpace(trnText))
                    {
                        trnText = CommonProxy.GetTranslatedText(moduleName, languagePair, trnKey, defaultText);
                        if (!String.IsNullOrWhiteSpace(trnText))
                        {
                            SetCacheTranslation(cacheKey, trnText);
                        }
                    }
                }
                else
                {
                    var editLink = GetTranslationLink(moduleName, trnKey, languagePair);
                    var cleanText = CommonProxy.GetTranslatedText(moduleName, languagePair, trnKey, defaultText);

                    trnText = String.Concat(cleanText, editLink);
                }

                defaultText = trnText;
            }

            return defaultText;
        }


        public static void SetTranslatedText(String trnKey, String defaultText, String languagePair, String translatedText)
        {
            if (!EnableTranslation)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(trnKey))
            {
                return;
            }

            var moduleName = PermissionUtil.ModuleName;
            var cacheKey = $"{moduleName}|{trnKey}|{languagePair}";

            var trnText = GetCacheTranslation(cacheKey);
            if (!String.IsNullOrWhiteSpace(trnText))
            {
                SetCacheTranslation(cacheKey, translatedText);
            }

            trnText = CommonProxy.GetTranslatedText(moduleName, languagePair, trnKey, defaultText);
            if (!String.IsNullOrWhiteSpace(trnText))
            {
                CommonProxy.SetTranslatedText(moduleName, languagePair, trnKey, translatedText);
            }
        }

        private static void SetCacheTranslation(String cacheKey, String translateText)
        {
            var cache = CommonObjectCache.InitObject(TrnCacheKey, () => new Dictionary<String, String>());
            SetCacheTranslation(cache, cacheKey, translateText);
        }
        private static void SetCacheTranslation(IDictionary<String, String> cache, String cacheKey, String translateText)
        {
            if (cache != null)
            {
                lock (cache)
                {
                    cache[cacheKey] = translateText;
                }
            }
        }

        private static String GetCacheTranslation(String cacheKey)
        {
            var cache = CommonObjectCache.InitObject(TrnCacheKey, () => new Dictionary<String, String>());
            return GetCacheTranslation(cache, cacheKey);
        }
        private static String GetCacheTranslation(IDictionary<String, String> cache, String cacheKey)
        {
            lock (cache)
            {
                String tranlatedText;
                if (!cache.TryGetValue(cacheKey, out tranlatedText))
                {
                    return null;
                }

                return tranlatedText;
            }
        }

        private static bool ContainsCacheTranslation(String cacheKey)
        {
            var cache = CommonObjectCache.InitObject(TrnCacheKey, () => new Dictionary<String, String>());
            return ContainsCacheTranslation(cache, cacheKey);
        }
        private static bool ContainsCacheTranslation(IDictionary<String, String> cache, String cacheKey)
        {
            lock (cache)
            {
                return cache.ContainsKey(cacheKey);
            }
        }

        public static void PreloadTranslations(Control control)
        {
            PreloadTranslations(control, false);
        }
        public static void PreloadTranslations(Control control, bool applyTranslations)
        {
            var moduleName = PermissionUtil.ModuleName;
            var languagePair = Thread.CurrentThread.CurrentCulture.Name;


            var cache = CommonObjectCache.InitObject(TrnCacheKey, () => new Dictionary<String, String>());
            var controls = UserInterfaceUtil.TraverseChildren(control);

            var query = from ctl in controls
                        let trn = ctl as ITranslatable
                        where trn != null &&
                              !String.IsNullOrWhiteSpace(trn.Text)
                        let defText = GetClearText(trn.TrnKey, languagePair, trn.Text)
                        let trnKey = (String.IsNullOrWhiteSpace(trn.TrnKey) ? CryptographyUtil.ComputeMD5(defText) : trn.TrnKey)
                        let cacheKey = $"{moduleName}|{trnKey}|{languagePair}"
                select new
                        {
                            TrnKey = trnKey,
                            CacheKey = cacheKey,
                            DefaultText = defText,
                            Translatable = trn,
                        };

            var list = query.ToList();
            var lookup = list.ToLookup(n => n.CacheKey);

            var notInCacheControls = (from n in lookup
                                      where !ContainsCacheTranslation(cache, n.Key)
                                      select n).ToList();

            var notInCacheContracts = (from n in notInCacheControls
                                       let p = n.FirstOrDefault()
                                       let m = new TranslationContract { TrnKey = p.TrnKey, DefaultText = p.DefaultText }
                                       select m).ToList();

            var contracts = CommonProxy.GetTranslations(moduleName, languagePair, notInCacheContracts);

            foreach (var contract in contracts)
            {
                var cacheKey = $"{moduleName}|{contract.TrnKey}|{languagePair}";
                SetCacheTranslation(cache, cacheKey, contract.TranslatedText);

                if (applyTranslations)
                {
                    foreach (var item in lookup[cacheKey])
                    {
                        var translatable = item.Translatable;

                        translatable.TrnKey = item.TrnKey;
                        translatable.Text = contract.TranslatedText;
                    }
                }
            }
        }

        public static void ApplyTranslation(ITranslatable translatable)
        {
            if (!EnableTranslation)
                return;

            if (String.IsNullOrEmpty(translatable.Text))
                return;

            var defaultText = GetClearText(translatable);
            if (String.IsNullOrEmpty(translatable.TrnKey))
                translatable.TrnKey = CryptographyUtil.ComputeMD5(defaultText);

            translatable.Text = GetTranslatedText(translatable.TrnKey, defaultText);

            var defTranslatable = translatable as DefaultTranslatable;
            if (defTranslatable != null)
            {
                defTranslatable.Text = GetClearText(translatable);

                if (TranslationMode)
                    defTranslatable.Link = GetTranslationLink(translatable);
            }

        }

        public static String GetClearText(ITranslatable translatable)
        {
            var languagePair = Thread.CurrentThread.CurrentCulture.Name;
            return GetClearText(translatable.TrnKey, languagePair, translatable.Text);
        }

        public static String GetClearText(String trnKey, String languagePair, String text)
        {
            if (!TranslationMode)
                return text;

            if (String.IsNullOrWhiteSpace(text))
                return text;

            var editLink = GetTranslationLink(trnKey, languagePair);

            var patternIndex = text.IndexOf(editLink, StringComparison.OrdinalIgnoreCase);
            if (patternIndex > -1)
            {
                var clearText = text.Substring(0, patternIndex);
                return clearText;
            }

            return text;
        }

        public static String GetTranslationLink(ITranslatable translatable)
        {
            var moduleName = PermissionUtil.ModuleName;
            var languagePair = Thread.CurrentThread.CurrentCulture.Name;

            return GetTranslationLink(moduleName, translatable.TrnKey, languagePair);
        }
        public static String GetTranslationLink(String trnKey, String languagePair)
        {
            var moduleName = PermissionUtil.ModuleName;
            return GetTranslationLink(moduleName, trnKey, languagePair);
        }
        public static String GetTranslationLink(String moduleName, String trnKey, String languagePair)
        {
            var trnEditPage = ConfigurationManager.AppSettings[TrnEditPageKey];

            var editLink = String.Format(EditLinkFormat, trnEditPage, moduleName, trnKey, languagePair);
            return editLink;
        }
    }

}
