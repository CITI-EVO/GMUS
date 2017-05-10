using System;
using System.Collections.Generic;
using System.Reflection;

namespace CITI.EVO.Tools.Web.Services
{
    public interface IServiceAssemblyLoader
    {
        byte[] AssemblyBytes { get; }
        Assembly Assembly { get; }


        IList<Type> ServiceContracts { get; }

        IList<Type> DataContracts { get; }

        IList<MethodInfo> Methods { get; }

        DynamicObject CreateProxy(String contractName);
    }
}