using System;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Exceptions;
using CITI.EVO.Rpc.Utils;

namespace CITI.EVO.Rpc.Processing
{
    public class ServerRequestProcessor : RequestProcessorBase
    {
        protected override byte[] ProcessRequest(String peer, byte[] requestBytes)
        {
            try
            {
                return TryTransferToPeer(peer, requestBytes);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(peer, requestBytes, ex);
            }
        }

        private byte[] TryTransferToPeer(String peer, byte[] requestBytes)
        {
			var peerElement = PeersUtil.GetPeer(peer);
            if (peerElement == null)
            {
				throw new PeerNotFoundException(peer);
            }

            Uri peerUrl;
            if (!Uri.TryCreate(peerElement.Url, UriKind.RelativeOrAbsolute, out peerUrl))
            {
				throw new InvalidPeerUrlException(peer);
            }

            var requestTimeout = peerElement.RequestTimeout;
            if (requestTimeout == 0)
            {
                requestTimeout = 100000;
            }

            var transporter = TransporterUtil.CreateClientTransporter(peerUrl.ToString(), requestTimeout);

            var responseBytes = transporter.Send(requestBytes);
			return responseBytes;                
        }

        private byte[] GetErrorResponse(String peer, byte[] requestBytes, Exception ex)
        {
			

            var requestEntity = GetRequestEntity(requestBytes);

            var responseEntity = new ResponseEntity
            {
                RequestID = requestEntity.RequestID,
                ErrorCode = 1,
                ErrorMessage = ex.ToString(),
            };

            var responseBytes = GetResponseBytes(responseEntity);
            return responseBytes;
        }
	}
}
