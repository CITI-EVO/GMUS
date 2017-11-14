using System;
using System.Linq;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Helpers
{
    /// <summary>
    /// Summary description for MonitoringHelper
    /// </summary>
    public class MonitoringHelper
    {
        private readonly FormDataUnit _formDataUnit;
        private readonly ILookup<String, ControlEntity> _controlsLp;

        public MonitoringHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected DateTime? GetPeriodStartDate(int? period)
        {
            var periodLength = GetPeriodLength();

            var periodStartDate = GetPeriodStartDate(period, periodLength);
            return periodStartDate;
        }
        protected DateTime? GetPeriodStartDate(int? period, int? periodLength)
        {
            if (period == null)
                return null;

            var periodStartDateAlias = $"ProjectStartDate_{period}";

            var periodStartDateField = _controlsLp[periodStartDateAlias].FirstOrDefault();
            if (periodStartDateField != null)
            {
                var periodStartDateKey = Convert.ToString(periodStartDateField.ID);

                var periodStartDate = DataConverter.ToNullableDateTime(_formDataUnit[periodStartDateKey]);
                return periodStartDate;
            }

            if (period < 1)
                return null;

            var projectStartDateField = _controlsLp["ProjectStartDate"].FirstOrDefault();
            if (projectStartDateField == null)
                return null;

            var projectStartDateKey = Convert.ToString(projectStartDateField.ID);

            var projectStartDate = DataConverter.ToNullableDateTime(_formDataUnit[projectStartDateKey]);
            if (projectStartDate == null)
                return null;

            var start = periodLength.Value * (period.Value - 1);

            var startDate = projectStartDate.Value.AddMonths(start);
            return startDate;
        }

        protected DateTime? GetPeriodEndDate(int? period)
        {
            var periodLength = GetPeriodLength();

            var periodEndDate = GetPeriodEndDate(period, periodLength);
            return periodEndDate;
        }
        protected DateTime? GetPeriodEndDate(int? period, int? periodLength)
        {
            if (period == null)
                return null;

            var periodEndDateAlias = $"ProjectEndDate_{period}";

            var periodEndDateField = _controlsLp[periodEndDateAlias].FirstOrDefault();
            if (periodEndDateField != null)
            {
                var periodEndDateKey = Convert.ToString(periodEndDateField.ID);

                var periodEndDate = DataConverter.ToNullableDateTime(_formDataUnit[periodEndDateKey]);
                return periodEndDate;
            }

            if (periodLength == null)
                return null;

            var startDate = GetPeriodStartDate(period, periodLength);
            if (startDate == null)
                return null;

            var endDate = startDate.Value.AddMonths(periodLength.Value);
            return endDate;
        }

        public int? GetPeriodLength()
        {
            var periodLengthField = _controlsLp["PeriodLength"].FirstOrDefault();
            if (periodLengthField == null)
                return null;

            var periodLengthKey = Convert.ToString(periodLengthField.ID);

            var periodLength = DataConverter.ToNullableInt(_formDataUnit[periodLengthKey]);
            return periodLength;
        }
    }
}