using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.CommonData.Svc.Enums;
using CITI.EVO.Rpc;
using CITI.EVO.Rpc.Attributes;

namespace CITI.EVO.Proxies
{
	public static class CommonProxy
	{
		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetAreas")]
		public static List<AreaContract> GetAreas(AreaTypesEnum type, RecordTypesEnum recordType)
		{
			return RpcInvoker.InvokeMethod<List<AreaContract>>(type, recordType);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetChildAreas")]
		public static List<AreaContract> GetChildAreas(Guid parentID)
		{
			return RpcInvoker.InvokeMethod<List<AreaContract>>(parentID);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetAreaByID")]
		public static AreaContract GetAreaByID(Guid ID)
		{
			return RpcInvoker.InvokeMethod<AreaContract>(ID);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetAreaByCode")]
		public static AreaContract GetAreaByCode(String code)
		{
			return RpcInvoker.InvokeMethod<AreaContract>(code);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetAllMobileIndexes")]
		public static List<MobileIndexesContract> GetAllMobileIndexes()
		{
			return RpcInvoker.InvokeMethod<List<MobileIndexesContract>>();
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetMobileIndexByID")]
		public static MobileIndexesContract GetMobileIndexByID(Guid? ID)
		{
			return RpcInvoker.InvokeMethod<MobileIndexesContract>(ID);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetLanguages")]
		public static List<LanguageContract> GetLanguages()
		{
			return RpcInvoker.InvokeMethod<List<LanguageContract>>();
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetTranslatedText")]
		public static String GetTranslatedText(String moduleName, String languagePair, String trnKey, String defaultText)
		{
			return RpcInvoker.InvokeMethod<String>(moduleName, languagePair, trnKey, defaultText);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.GetTranslations")]
		public static List<TranslationContract> GetTranslations(String moduleName, String languagePair, List<TranslationContract> list)
		{
			return RpcInvoker.InvokeMethod<List<TranslationContract>>(moduleName, languagePair, list);
		}

		[RpcRemoteMethod("Common.CITI.EVO.CommonData.Web.Services.Managers.CommonServiceWrapper.SetTranslatedText")]
		public static void SetTranslatedText(String moduleName, String languagePair, String trnKey, String translatedText)
		{
			RpcInvoker.InvokeMethod(moduleName, languagePair, trnKey, translatedText);
		}


	}

}
