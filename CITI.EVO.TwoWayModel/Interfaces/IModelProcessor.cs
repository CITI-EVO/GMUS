using System;

namespace CITI.EVO.TwoWayModel.Interfaces
{
    public interface IModelProcessor
    {
        Object GetModel(Type type);

        void FillModel(Object model);

        void SetModel(Object model);
    }
}
