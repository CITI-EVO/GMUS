using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class MonitoringProjectFileModel
    {
        public Guid? ID { get; set; }

        public String FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}