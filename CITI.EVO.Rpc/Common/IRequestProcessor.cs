using CITI.EVO.Rpc.Processing;

namespace CITI.EVO.Rpc.Common
{
    public interface IRequestProcessor
    {
        RequestProcessorBase RequestProcessor { get; }
    }
}