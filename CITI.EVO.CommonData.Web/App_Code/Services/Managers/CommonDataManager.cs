using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.CommonData.DAL.Domain;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.CommonData.Svc.Enums;
using CITI.EVO.CommonData.Web.Caches;
using CITI.EVO.CommonData.Web.Extensions;
using CITI.EVO.Tools.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.CommonData.Web.Services.Managers
{
    public static class CommonDataManager
    {
        public static List<AreaContract> GetAreas(AreaTypesEnum type, RecordTypesEnum recordType)
        {

            switch (type)
            {
                case AreaTypesEnum.Region:
                    return GetAreasByCode((int)AreaTypesEnum.Region, recordType);

                case AreaTypesEnum.Municipality:
                    return GetAreasByCode((int)AreaTypesEnum.Municipality, recordType);

                case AreaTypesEnum.MunicipalCenter:
                    return GetAreasByCode((int)AreaTypesEnum.MunicipalCenter, recordType);

                case AreaTypesEnum.Town:
                    return GetAreasByCode((int)AreaTypesEnum.Town, recordType);

                case AreaTypesEnum.Village:
                    return GetAreasByCode((int)AreaTypesEnum.Village, recordType);

                case AreaTypesEnum.Country:
                    return GetAreasByCode((int)AreaTypesEnum.Country, recordType);

            }

            return null;
        }

        public static List<AreaContract> GetAreasByCode(int code, RecordTypesEnum recordType)
        {
            var session = Hb8Factory.InitSession();

            var areasQuery = from n in session.Query<CD_Area>()
                             where n.AreaType.Code == code
                             select n;

            switch (recordType)
            {
                case RecordTypesEnum.Active:
                    {
                        areasQuery = from n in areasQuery
                                     where n.DateDeleted == null
                                     select n;
                    }
                    break;
                case RecordTypesEnum.Inactive:
                    {
                        areasQuery = from n in areasQuery
                                     where n.DateDeleted != null
                                     select n;
                    }
                    break;
            }

            var areas = areasQuery.ToList();

            return areas.ToContracts();
        }

        public static AreaContract GetAreaByCode(String code)
        {
            if (String.IsNullOrEmpty(code))
                return null;

            var session = Hb8Factory.InitSession();

            var area = (from n in session.Query<CD_Area>()
                        where n.NewCode == code.Trim()
                        select n).FirstOrDefault();

            if (area == null)
                return null;

            return area.ToContract();
        }

        public static List<AreaContract> GetChildAreas(Guid parentID)
        {
            var session = Hb8Factory.InitSession();

            var childAreas = (from a in session.Query<CD_Area>()
                              where a.ParentID == parentID
                              select a).ToList();

            return childAreas.ToContracts();
        }

        public static AreaContract GetAreaByID(Guid ID)
        {
            var session = Hb8Factory.InitSession();

            var area = (from a in session.Query<CD_Area>()
                        where a.ID == ID
                        select a).FirstOrDefault();

            return area.ToContract();
        }

        public static List<MobileIndexesContract> GetAllMobileIndexes()
        {
            var session = Hb8Factory.InitSession();

            var mobileIndexes = (from n in session.Query<CD_MobileIndex>()
                                 where n.DateDeleted == null
                                 select n).ToList();

            return mobileIndexes.ToContracts();
        }

        public static MobileIndexesContract GetMobileIndexByID(Guid? ID)
        {
            if (ID == null || ID == Guid.Empty)
                return null;

            var session = Hb8Factory.InitSession();

            var mobileIndex = (from n in session.Query<CD_MobileIndex>()
                               where n.ID == ID
                               select n).FirstOrDefault();

            return mobileIndex.ToContract();
        }

        public static List<LanguageContract> GetLanguages()
        {
            return LanguageCache.LangCache.ToContracts();
        }

        public static TranslationContract GetTranslation(String moduleName, String languagePair, String trnKey)
        {
            if (String.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentNullException("moduleName");

            if (String.IsNullOrWhiteSpace(trnKey))
                throw new ArgumentNullException("trnKey");

            if (String.IsNullOrWhiteSpace(languagePair))
                throw new ArgumentNullException("languagePair");

            var session = Hb8Factory.InitSession();

            var entity = (from n in session.Query<CD_Translation>()
                          where n.DateDeleted == null &&
                                n.TrnKey.ToLower() == trnKey.ToLower() &&
                                n.LanguagePair.ToLower() == languagePair.ToLower() &&
                                n.ModuleName.ToLower() == moduleName.ToLower()
                          select n).SingleOrDefault();

            if (entity == null)
                return null;

            var contract = new TranslationContract
            {
                TrnKey = trnKey,
                DefaultText = entity.DefaultText,
                TranslatedText = entity.TranslatedText
            };

            return contract;
        }

        public static void SetTranslation(String moduleName, String languagePair, String trnKey, TranslationContract contract)
        {
            if (String.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentNullException("moduleName");

            if (String.IsNullOrWhiteSpace(trnKey))
                throw new ArgumentNullException("trnKey");

            if (String.IsNullOrWhiteSpace(languagePair))
                throw new ArgumentNullException("languagePair");

            if (contract == null)
                throw new ArgumentNullException("contract");

            var session = Hb8Factory.InitSession();

            var entity = (from n in session.Query<CD_Translation>()
                          where n.DateDeleted == null &&
                                n.TrnKey.ToLower() == trnKey.ToLower() &&
                                n.LanguagePair.ToLower() == languagePair.ToLower() &&
                                n.ModuleName.ToLower() == moduleName.ToLower()
                          select n).SingleOrDefault();

            if (entity == null)
            {
                entity = new CD_Translation
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now
                };

                entity.TrnKey = trnKey;
                entity.ModuleName = moduleName;
                entity.LanguagePair = languagePair;
                entity.DefaultText = contract.DefaultText;
                entity.TranslatedText = contract.TranslatedText;

                session.SubmitChanges(entity);
            }
            else
            {
                TranslationCache.SetTranslatedText(moduleName, languagePair, trnKey, contract.TranslatedText);
            }
        }

        public static String GetTranslatedText(String moduleName, String languagePair, String trnKey, String defaultText)
        {
            if (String.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentNullException("moduleName");

            if (String.IsNullOrWhiteSpace(trnKey))
                throw new ArgumentNullException("trnKey");

            if (String.IsNullOrWhiteSpace(languagePair))
                throw new ArgumentNullException("languagePair");

            var translatedText = TranslationCache.GetTranslatedText(moduleName, languagePair, trnKey, defaultText);
            return translatedText;

        }

        public static void SetTranslatedText(String moduleName, String languagePair, String trnKey, String translatedText)
        {
            if (String.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentNullException("moduleName");

            if (String.IsNullOrWhiteSpace(trnKey))
                throw new ArgumentNullException("trnKey");

            if (String.IsNullOrWhiteSpace(languagePair))
                throw new ArgumentNullException("languagePair");

            TranslationCache.SetTranslatedText(moduleName, languagePair, trnKey, translatedText);
        }

        public static List<TranslationContract> GetTranslations(String moduleName, String languagePair, List<TranslationContract> list)
        {
            var @set = new HashSet<String>();
            var result = new List<TranslationContract>();

            foreach (var contract in list)
            {
                if (String.IsNullOrWhiteSpace(contract.TrnKey))
                    continue;

                if (!@set.Add(contract.TrnKey))
                    continue;

                contract.TranslatedText = TranslationCache.GetTranslatedText(moduleName, languagePair, contract.TrnKey, contract.DefaultText);
                result.Add(contract);
            }

            return result;
        }
    }
}