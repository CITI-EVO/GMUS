using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CITI.EVO.Tools.Web.Services
{
    public class DynamicProxy : DynamicObject
    {
        public DynamicProxy(Type proxyType, Binding binding, EndpointAddress address)
            : base(proxyType)
        {
            var paramTypes = new[] { typeof(Binding), typeof(EndpointAddress) };
            var paramValues = new Object[] { binding, address };

            CallConstructor(paramTypes, paramValues);
        }

        public Type ProxyType
        {
            get { return ObjectType; }
        }

        public Object Proxy
        {
            get { return ObjectInstance; }
        }

        public void Close()
        {
            CallMethod("Close");
        }
    }
}