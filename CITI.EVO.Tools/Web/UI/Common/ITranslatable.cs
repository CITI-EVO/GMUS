using System;
using System.Web.UI;

namespace CITI.EVO.Tools.Web.UI.Common
{
    public interface ITranslatable
    {
        StateBag ViewState { get; }

        String TrnKey { get; set; }

        String Text { get; set; }
    }
}
