using System;

namespace CITI.EVO.Tools.Helpers.Hibern8
{
    [Flags]
    public enum DataTableTransformerMode
    {
        None = 0,
        InitDataTable = 1,
        FillDataTable = 2,
        RecheckColumns = 4,
    }
}