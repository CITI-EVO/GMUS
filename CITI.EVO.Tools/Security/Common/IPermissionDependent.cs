using System;

namespace CITI.EVO.Tools.Security.Common
{
	public interface IPermissionDependent
	{
		String PermissionKey { get; set; }

		bool DisableIfNoAccess { get; set; }

		bool Enabled { get; set; }

		bool Visible { get; set; }

		bool HasAccess();
	}
}
