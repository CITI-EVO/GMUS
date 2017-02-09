using System.Collections.Generic;
using System.Linq;

using CITI.EVO.CommonData.Svc.Contracts;
using CITI.EVO.CommonData.Svc.Enums;
using CITI.EVO.CommonData.DAL.Domain;

namespace CITI.EVO.CommonData.Web.Extensions
{
	public static class ContractsExtensions
	{
		#region ToContract
		public static AreaContract ToContract(this CD_Area entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new AreaContract();
			contract.ID = entity.ID;
			contract.OLD_ID = entity.OLD_ID;
			contract.ParentID = entity.ParentID;
			contract.Code = entity.Code;
			contract.CraCode = entity.CraCode;
			contract.GeoName = entity.GeoName;
			contract.EngName = entity.EngName;
			contract.TypeID = entity.TypeID;
			contract.RecordType = (entity.DateDeleted == null) ? RecordTypesEnum.Active : RecordTypesEnum.Inactive;
			contract.NewCode = entity.NewCode;
			contract.DateChanged = entity.DateChanged;
			contract.DateCreated = entity.DateCreated;
			contract.DateDeleted = entity.DateDeleted;
			contract.AreaType = entity.AreaType.ToContract();
			contract.PhoneIndexes = entity.PhoneIndexes.ToContracts();

			return contract;
		}


		public static AreaTypeContract ToContract(this CD_AreaType entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new AreaTypeContract();
			contract.ID = entity.ID;
			contract.GeoName = entity.GeoName;
			contract.EngName = entity.EngName;
			contract.Code = entity.Code;
			contract.Level = entity.Level;
			contract.DateChanged = entity.DateChanged;
			contract.DateCreated = entity.DateCreated;
			contract.DateDeleted = entity.DateDeleted;

			return contract;
		}


		public static PhoneIndexContract ToContract(this CD_PhoneIndex entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new PhoneIndexContract();
			contract.ID = entity.ID;
			contract.PhoneIndexType = entity.PhoneIndexType.ToContract();
			contract.Value = entity.Value;
			contract.DateChanged = entity.DateChanged;
			contract.DateCreated = entity.DateCreated;
			contract.DateDeleted = entity.DateDeleted;

			return contract;
		}


		public static PhoneIndexTypeContract ToContract(this CD_PhoneIndexType entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new PhoneIndexTypeContract();
			contract.ID = entity.ID;
			contract.Name = entity.Name;
			contract.DateChanged = entity.DateChanged;
			contract.DateCreated = entity.DateCreated;
			contract.DateDeleted = entity.DateDeleted;

			return contract;
		}

		public static MobileIndexesContract ToContract(this CD_MobileIndex entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new MobileIndexesContract();
			contract.ID = entity.ID;
			contract.Value = entity.Value;
			contract.GeoOperatorName = entity.GeoOperatorName;
			contract.EngOperatorName = entity.EngOperatorName;
			contract.DateCreated = entity.DateCreated;
			contract.DateChanged = entity.DateChanged;
			contract.DateDeleted = entity.DateDeleted;

			return contract;
		}

		public static LanguageContract ToContract(this CD_Language entity)
		{
			if (entity == null)
			{
				return null;
			}

			var contract = new LanguageContract();

			contract.ID = entity.ID;
			contract.DisplayName = entity.DisplayName;
			contract.EngName = entity.EngName;
			contract.NativeName = entity.NativeName;
			contract.Pair = entity.Pair;
			contract.DateCreated = entity.DateCreated;
			contract.DateChanged = entity.DateChanged;
			contract.DateDeleted = entity.DateDeleted;

			return contract;
		}

		#endregion

		#region ToContracts
		public static List<AreaContract> ToContracts(this IEnumerable<CD_Area> list)
		{
			if (list == null)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
			return result;
		}

		public static List<PhoneIndexContract> ToContracts(this IEnumerable<CD_PhoneIndex> list)
		{
			if (list == null)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
			return result;
		}

		public static List<MobileIndexesContract> ToContracts(this IEnumerable<CD_MobileIndex> list)
		{
			if (list == null)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
			return result;
		}

		public static List<LanguageContract> ToContracts(this IEnumerable<CD_Language> list)
		{
			if (list == null)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToContract()).ToList();
			return result;
		}

		#endregion

		#region ToEntity
		public static CD_Area ToEntity(this AreaContract contract)
		{
			if (contract == null)
			{
				return null;
			}

			var entity = new CD_Area();
			entity.ID = contract.ID;
			entity.OLD_ID = contract.OLD_ID;
			entity.ParentID = contract.ParentID;
			entity.Code = contract.Code;
			entity.CraCode = contract.CraCode;
			entity.GeoName = contract.GeoName;
			entity.EngName = contract.EngName;
			entity.TypeID = contract.TypeID;
			entity.NewCode = contract.NewCode;
			entity.DateChanged = contract.DateChanged;
			entity.DateCreated = contract.DateCreated;
			entity.DateDeleted = contract.DateDeleted;

			return entity;
		}

		public static CD_MobileIndex ToEntity(this MobileIndexesContract contract)
		{
			if (contract == null)
			{
				return null;
			}

			var entity = new CD_MobileIndex();

			entity.ID = contract.ID;
			entity.Value = contract.Value.GetValueOrDefault();
			entity.GeoOperatorName = contract.GeoOperatorName;
			entity.EngOperatorName = contract.EngOperatorName;
			entity.DateCreated = contract.DateCreated;
			entity.DateChanged = contract.DateChanged;
			entity.DateDeleted = contract.DateDeleted;

			return entity;
		}

		public static CD_Language ToEntity(this LanguageContract contract)
		{
			if (contract == null)
			{
				return null;
			}

			var entity = new CD_Language();

			entity.ID = contract.ID;
			entity.DisplayName = contract.DisplayName;
			entity.EngName = contract.EngName;
			entity.NativeName = contract.NativeName;
			entity.Pair = contract.Pair;
			entity.DateCreated = contract.DateCreated;
			entity.DateChanged = contract.DateChanged;
			entity.DateDeleted = contract.DateDeleted;

			return entity;
		}

		#endregion

		#region ToEntities
		public static List<CD_Area> ToEntities(this List<AreaContract> list)
		{
			if (list == null || list.Count == 0)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
			return result;
		}

		public static List<CD_MobileIndex> ToEntities(this List<MobileIndexesContract> list)
		{
			if (list == null || list.Count == 0)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
			return result;
		}

		public static List<CD_Language> ToEntities(this List<LanguageContract> list)
		{
			if (list == null || list.Count == 0)
			{
				return null;
			}

			var result = list.Where(n => n != null).Select(n => n.ToEntity()).ToList();
			return result;
		}

		#endregion
	}
}