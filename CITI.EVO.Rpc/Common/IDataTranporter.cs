using System;

namespace CITI.EVO.Rpc.Common
{
    public interface IDataTranporter: IDisposable
    {
        Uri Url { get; }

        int Timeout { get; }

        byte[] Send(byte[] bytes);
    }
}
