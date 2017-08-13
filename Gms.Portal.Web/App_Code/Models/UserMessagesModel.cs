using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class UserMessagesModel
    {
        public List<UserMessageModel> List { get; set; }
    }
}