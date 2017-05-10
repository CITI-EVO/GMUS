using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.EventStructure;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class PhaseUnitsModel
    {
        public List<PhaseEntity> List { get; set; }
    }
}