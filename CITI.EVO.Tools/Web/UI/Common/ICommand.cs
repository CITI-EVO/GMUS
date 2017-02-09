using System;

namespace CITI.EVO.Tools.Web.UI.Common
{
    public interface ICommand
    {
        String CommandName { get; set; }

        String CommandArgument { get; set; }
    }
}
