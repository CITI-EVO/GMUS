using System;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Transporters;

namespace CITI.EVO.Rpc.Utils
{
    public static class TransporterUtil
    {
        private const String clientHubName = "RpcClientSignalR";
        private const String serverHubName = "RpcServerSignalR";

        public static IDataTranporter CreateClientTransporter(String url, int timeout)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                throw new Exception();
            }

            return new HttpDataTransporter(uri, timeout);
        }

        public static IDataTranporter CreateServerTransporter(String url, int timeout)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                throw new Exception();
            }

            return new HttpDataTransporter(uri, timeout);
        }

        private static Uri GetCorrectUri(Uri uri)
        {
            if (String.Equals(uri.Scheme, "SignalR", StringComparison.OrdinalIgnoreCase))
            {
                var uriBuilder = new UriBuilder(uri)
                {
                    Scheme = Uri.UriSchemeHttp,
                };

                return uriBuilder.Uri;
            }

            if (String.Equals(uri.Scheme, "SignalRS", StringComparison.OrdinalIgnoreCase))
            {
                var uriBuilder = new UriBuilder(uri)
                {
                    Scheme = Uri.UriSchemeHttps,
                };

                return uriBuilder.Uri;
            }

            return uri;
        }
    }
}
