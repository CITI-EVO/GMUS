using System;
using System.Collections.Generic;
using System.Linq;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.Others;

namespace Gms.Portal.Web.Helpers
{
    public class SpecialFormDataComparer
    {
        private const String keywordFieldAlias = "keyword";

        private const String workPlaceFieldAlias = "work_place";
        private const String workDepartmentFieldAlias = "work_department";

        private const String primaryPersonnelFieldAlias = "primary_personnel";

        private readonly StringComparer _comparer;

        private readonly FormDataUnit _profileFormData;
        private readonly FormDataUnit _projectFormData;

        private readonly ILookup<Guid?, IDNameExEntity> _directionsLp;

        private readonly ILookup<Guid?, FormDataUnit> _keywordsLp;
        private readonly ILookup<Guid?, FormDataUnit> _employmentsLp;
        private readonly ILookup<Guid?, FormDataUnit> _primaryPersonnelsLp;

        public SpecialFormDataComparer
        (
            FormDataUnit profileFormData,
            FormDataUnit projectFormData,
            ILookup<Guid?, IDNameExEntity> directionsLp,
            ILookup<Guid?, FormDataUnit> keywordsLp,
            ILookup<Guid?, FormDataUnit> employmentsLp,
            ILookup<Guid?, FormDataUnit> primaryPersonnelsLp
        )
        {
            _comparer = StringComparer.OrdinalIgnoreCase;

            _profileFormData = profileFormData;
            _projectFormData = projectFormData;

            _directionsLp = directionsLp;
            _keywordsLp = keywordsLp;
            _employmentsLp = employmentsLp;
            _primaryPersonnelsLp = primaryPersonnelsLp;
        }

        public bool IsAcceptable(FormDataUnit otherFormData)
        {
            if (_projectFormData.UserID == otherFormData.UserID)
                return false;

            var personnel = GetPersonnels(_projectFormData);

            if (personnel.Contains(Convert.ToString(otherFormData.ID)) ||
                personnel.Contains(Convert.ToString(otherFormData.UserID)))
                return false;

            var sameEmployments = GetSameEmployments(_profileFormData, otherFormData);
            if (sameEmployments.Count > 0)
                return false;

            var sameDirections = GetSameDirections(_projectFormData, otherFormData);
            if (sameDirections.Count == 0)
                return false;

            return true;
        }

        public int Compare(FormDataUnit x, FormDataUnit y)
        {
            var xDirections = GetDirections(x);
            var yDirections = GetDirections(y);

            if (xDirections.Count > 0 && yDirections.Count == 0)
                return 1;

            if (xDirections.Count == 0 && yDirections.Count > 0)
                return -1;

            if (xDirections.Count > 0 && yDirections.Count > 0)
            {
                var projectDirections = GetDirections(_projectFormData);
                if (projectDirections.Count > 0)
                {
                    var projectEnumerator = projectDirections.GetEnumerator();

                    var xEnumerator = xDirections.GetEnumerator();
                    var yEnumerator = yDirections.GetEnumerator();

                    while (projectEnumerator.MoveNext() &&
                           xEnumerator.MoveNext() &&
                           yEnumerator.MoveNext())
                    {
                        var projectDirection = projectEnumerator.Current;
                        var xDirection = xEnumerator.Current;
                        var yDirection = yEnumerator.Current;

                        if (_comparer.Equals(projectDirection, xDirection) &&
                            !_comparer.Equals(projectDirection, yDirection))
                            return 1;

                        if (!_comparer.Equals(projectDirection, xDirection) &&
                            _comparer.Equals(projectDirection, yDirection))
                            return -1;
                    }
                }
            }

            var xSameKeywords = GetSameKeywords(_projectFormData, x);
            var ySameKeywords = GetSameKeywords(_projectFormData, y);

            var order = xSameKeywords.Count.CompareTo(ySameKeywords.Count);
            if (order != 0)
                return order;

            return order;
        }

        public ISet<String> GetPersonnels(FormDataUnit fromData)
        {
            var @set = new HashSet<String>(_comparer);

            var primaryPersonnels = _primaryPersonnelsLp[fromData.ID];
            if (!primaryPersonnels.Any())
                return @set;

            var query = (from n in primaryPersonnels
                         let wp = n[primaryPersonnelFieldAlias]
                         select Convert.ToString(wp));

            @set.UnionWith(query);

            return @set;
        }

        public ISet<String> GetDirections(FormDataUnit formData)
        {
            var @set = new HashSet<String>(_comparer);

            var xDirections = _directionsLp[formData.ID];
            if (!xDirections.Any())
                return @set;

            var xQuery = xDirections.Select(n => n.Value);
            @set.UnionWith(xQuery);

            return @set;
        }

        public ISet<String> GetSameKeywords(FormDataUnit x, FormDataUnit y)
        {
            var @set = new HashSet<String>();

            var xKeywords = _keywordsLp[x.ID];
            if (!xKeywords.Any())
                return @set;

            var yKeywords = _keywordsLp[y.ID];
            if (!yKeywords.Any())
                return @set;

            var xQuery = (from n in xKeywords
                          let m = Convert.ToString(n[keywordFieldAlias])
                          where !String.IsNullOrWhiteSpace(m)
                          select m);

            var yQuery = (from n in yKeywords
                          let m = Convert.ToString(n[keywordFieldAlias])
                          where !String.IsNullOrWhiteSpace(m)
                          select m);

            @set.UnionWith(xQuery);
            @set.IntersectWith(yQuery);

            return @set;
        }

        public ISet<String> GetSameDirections(FormDataUnit x, FormDataUnit y)
        {
            var @set = new HashSet<String>(_comparer);

            var xDirections = _directionsLp[x.ID];
            if (!xDirections.Any())
                return @set;

            var yDirections = _directionsLp[y.ID];
            if (!yDirections.Any())
                return @set;

            var xQuery = xDirections.Select(n => n.Value);
            var yQuery = yDirections.Select(n => n.Value);

            @set.UnionWith(xQuery);
            @set.IntersectWith(yQuery);

            return @set;
        }

        public ISet<String> GetSameEmployments(FormDataUnit x, FormDataUnit y)
        {
            var @set = new HashSet<String>();

            var xEmployments = _employmentsLp[x.ID];
            if (!xEmployments.Any())
                return @set;

            var yEmployments = _employmentsLp[y.ID];
            if (!yEmployments.Any())
                return @set;

            var xQuery = (from n in xEmployments
                          let wp = n[workPlaceFieldAlias]
                          let wd = n[workDepartmentFieldAlias]
                          let v = $"{wp} {wd}"
                          select v);

            var yQuery = (from n in yEmployments
                          let wp = n[workPlaceFieldAlias]
                          let wd = n[workDepartmentFieldAlias]
                          let v = $"{wp} {wd}"
                          select v);

            @set.UnionWith(xQuery);
            @set.IntersectWith(yQuery);

            return @set;
        }
    }
}