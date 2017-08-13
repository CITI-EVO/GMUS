using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Helpers
{
    public interface ICustomMetaControl
    {
        Object Value { get; set; }

        IDictionary<String, Object> GetFullContent();
    }
}