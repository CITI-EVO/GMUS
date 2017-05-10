using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.CommonData.DAL.Domain;
using NHibernate.Linq;

namespace CITI.EVO.CommonData.Web.Caches
{
    public static class TranslationCache
    {
        private const String cacheKey = "@{TranslationCache_Dict}";

        private static IDictionary<String, String> _trnCache;
        public static IDictionary<String, String> TrnCache
        {
            get
            {
                if (_trnCache == null)
                {
                    _trnCache = CommonObjectCache.InitObject(cacheKey, LoadTranslations);
                }

                return _trnCache;
            }
        }

        public static String GetTranslatedText(String moduleName, String languagePair, String trnKey, String defaultText)
        {
            var itemKey = GenCacheItemKey(moduleName, languagePair, trnKey);

            var trnCache = TrnCache;
            lock (trnCache)
            {
                String translatedText;
                if (!trnCache.TryGetValue(itemKey, out translatedText))
                {
                    translatedText = defaultText;

                    trnCache.Add(itemKey, translatedText);

                    AddOrUpdateInDbAsync(moduleName, languagePair, trnKey, defaultText, defaultText);
                }

                return translatedText;
            }


        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetTranslatedText(String moduleName, String languagePair, String trnKey, String translatedText)
        {
            var itemKey = GenCacheItemKey(moduleName, languagePair, trnKey);

            var trnCache = TrnCache;
            lock (trnCache)
            {
                if (trnCache.ContainsKey(itemKey))
                {
                    trnCache[itemKey] = translatedText;
                    AddOrUpdateInDbAsync(moduleName, languagePair, trnKey, null, translatedText);
                }
            }
        }

        public static String GenCacheItemKey(CD_Translation trn)
        {
            return GenCacheItemKey(trn.ModuleName, trn.LanguagePair, trn.TrnKey);
        }
        public static String GenCacheItemKey(String moduleName, String languagePair, String trnKey)
        {
            moduleName = (moduleName ?? String.Empty).ToLower();
            trnKey = (trnKey ?? String.Empty).ToLower();
            languagePair = (languagePair ?? String.Empty).ToLower();

            var keyText = String.Concat(moduleName, trnKey, languagePair);
            return keyText.ComputeMd5();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static IDictionary<String, String> LoadTranslations()
        {
            using (var session = Hb8Factory.CreateSession())
            {
                var translationsList = session.Query<CD_Translation>().ToList();

                var trnList = (from n in translationsList
                               select new
                               {
                                   n.TrnKey,
                                   n.ModuleName,
                                   n.LanguagePair,
                                   n.TranslatedText,
                                   n.DateDeleted,
                               }).ToList();

                var keyedTranList = (from n in trnList.AsParallel()
                                     let key = GenCacheItemKey(n.ModuleName, n.LanguagePair, n.TrnKey)
                                     select new
                                     {
                                         Key = key,
                                         n.TrnKey,
                                         n.ModuleName,
                                         n.LanguagePair,
                                         n.TranslatedText,
                                         n.DateDeleted,
                                     }).ToList();

                var translationsLp = keyedTranList.ToLookup(n => n.Key);

                var translationsDict = (from grp in translationsLp
                                        let trn = grp.First(m => m.DateDeleted == null)
                                        let pair = new KeyValuePair<String, String>(grp.Key, trn.TranslatedText)
                                        select pair).ToDictionary();

                return translationsDict;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void AddOrUpdateInDbAsync(String moduleName, String languagePair, String trnKey, String defaultText, String translatedText)
        {
            var task = new Task(() => AddOrUpdateInDb(moduleName, languagePair, trnKey, defaultText, translatedText));
            task.Start();
        }

        private static void AddOrUpdateInDb(String moduleName, String languagePair, String trnKey, String defaultText, String translatedText)
        {
            try
            {
                using (var session = Hb8Factory.CreateSession())
                {
                    var dbTrn = (from n in session.Query<CD_Translation>()
                                 where n.ModuleName == moduleName &&
                                       n.TrnKey == trnKey &&
                                       n.LanguagePair == languagePair //&&
                                                                      //n.Hashcode == db.GenTrnHashCode(moduleName, trnKey, languagePair)
                                 select n).FirstOrDefault();

                    if (dbTrn == null)
                    {
                        dbTrn = new CD_Translation
                        {
                            ID = Guid.NewGuid(),
                            DateCreated = DateTime.Now
                        };
                    }

                    dbTrn.ModuleName = moduleName;
                    dbTrn.TrnKey = trnKey;
                    dbTrn.LanguagePair = languagePair;
                    dbTrn.DefaultText = defaultText;
                    dbTrn.TranslatedText = translatedText;

                    session.SubmitChanges(dbTrn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}