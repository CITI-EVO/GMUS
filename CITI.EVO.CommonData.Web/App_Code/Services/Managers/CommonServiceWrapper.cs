using System;
using System.Collections.Generic;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.CommonData.Svc.Enums;
using CITI.EVO.Rpc.Attributes;

namespace CITI.EVO.CommonData.Web.Services.Managers
{
	public static class CommonServiceWrapper
	{
		[RpcAllowRemoteCall]
		public static List<AreaContract> GetAreas(AreaTypesEnum type, RecordTypesEnum recordType)
		{
			return CommonDataManager.GetAreas(type, recordType);
		}

		[RpcAllowRemoteCall]
		public static List<AreaContract> GetChildAreas(Guid ParentID)
		{
			return CommonDataManager.GetChildAreas(ParentID);
		}

		[RpcAllowRemoteCall]
		public static AreaContract GetAreaByID(Guid Id)
		{
			return CommonDataManager.GetAreaByID(Id);
		}

		[RpcAllowRemoteCall]
		public static AreaContract GetAreaByCode(String code)
		{
			return CommonDataManager.GetAreaByCode(code);
		}

		[RpcAllowRemoteCall]
		public static List<MobileIndexesContract> GetAllMobileIndexes()
		{
			return CommonDataManager.GetAllMobileIndexes();
		}

		[RpcAllowRemoteCall]
		public static MobileIndexesContract GetMobileIndexByID(Guid? ID)
		{
			return CommonDataManager.GetMobileIndexByID(ID);
		}

		[RpcAllowRemoteCall]
		public static List<LanguageContract> GetLanguages()
		{
			return CommonDataManager.GetLanguages();
		}

        [RpcAllowRemoteCall]
        public static TranslationContract GetTranslation(String moduleName, String languagePair, String trnKey)
        {
            return CommonDataManager.GetTranslation(moduleName, languagePair, trnKey);
        }

        [RpcAllowRemoteCall]
        public static void SetTranslation(String moduleName, String languagePair, String trnKey, TranslationContract contract)
        {
            CommonDataManager.SetTranslation(moduleName, languagePair, trnKey, contract);
        }

        [RpcAllowRemoteCall]
		public static String GetTranslatedText(String moduleName, String trnKey, String languagePair, String defaultText)
		{
			return CommonDataManager.GetTranslatedText(moduleName, trnKey, languagePair, defaultText);
		}

		[RpcAllowRemoteCall]
		public static List<TranslationContract> GetTranslations(String moduleName, String languagePair, List<TranslationContract> list)
		{
			return CommonDataManager.GetTranslations(moduleName, languagePair, list);
		}

		[RpcAllowRemoteCall]
		public static void SetTranslatedText(String moduleName, String trnKey, String languagePair, String translatedText)
		{
			CommonDataManager.SetTranslatedText(moduleName, trnKey, languagePair, translatedText);
		}
	}
}