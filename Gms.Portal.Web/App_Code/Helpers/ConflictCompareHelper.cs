using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;
using NHibernate.Linq;

namespace Gms.Portal.Web.Helpers
{
    //public class ConflictCompareHelper
    //{
    //    private StringComparer _comparer;

    //    private Guid? _currentFormID;
    //    private IEnumerable<FormModel> _profileFormModels;
    //    private IDictionary<Guid?, ILookup<String, ControlEntity>> _formControls;

    //    public ConflictCompareHelper(ISession hbSession, Guid? currentFormID, IEnumerable<Guid> profileForms)
    //    {
    //        var converter = new FormEntityModelConverter(hbSession);

    //        var query = (from n in profileForms
    //                     let f = hbSession.Query<GM_Form>().FirstOrDefault(h => h.ID == n)
    //                     let m = converter.Convert(f)
    //                     select m);

    //        _comparer = StringComparer.OrdinalIgnoreCase;
    //        _currentFormID = currentFormID;
    //        _formControls = new Dictionary<Guid?, ILookup<string, ControlEntity>>();
    //        _profileFormModels = query.ToList();

    //        foreach (var formModel in _profileFormModels)
    //        {
    //            var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity).ToList();
    //            var controlsLp = controls.ToLookup(n => n.Alias, _comparer);

    //            _formControls.Add(formModel.ID, controlsLp);
    //        }
    //    }

    //    public bool IsExpertAcceptable(Guid? ownerUserID, Guid? expertUserID)
    //    {
    //        var expertFilter = new Dictionary<String, Object>
    //        {
    //            [FormDataConstants.DateDeletedField] = null,
    //            [FormDataConstants.UserIDField] = expertUserID
    //        };

    //        var expertDoc = MongoDbUtil.FindDocuments(_currentFormID, expertFilter).FirstOrDefault();
    //        if (expertDoc != null)
    //            return false;

    //        var forms = _profileFormModels.ToList();

    //        for (int i = 0; i < forms.Count; i++)
    //        {
    //            for (int j = i; j < forms.Count; j++)
    //            {
    //                var ownerProfileFormID = forms[i].ID;
    //                var expertProfileFormID = forms[j].ID;

    //                if (IsExpertAcceptable(ownerUserID, expertUserID, ownerProfileFormID, expertProfileFormID))
    //                    return true;
    //            }
    //        }

    //        return false;
    //    }

    //    protected bool IsExpertAcceptable(Guid? ownerUserID, Guid? expertUserID, Guid? ownerProfileFormID, Guid? expertProfileFormID)
    //    {
    //        /*
    //          i. არ ემთხვევა ძირითადი პერსონალის სამუშაო ორგანიზაცია და დეპარტამენტი,
    //          ii. არ მონაწილეობს კონკურსში
    //          b. ემთხვევა ძირითადი მიმართულება, დამატებითი მიმართულება, საკვანძო სიტყვები.
    //         */

    //        var expertFilter = new Dictionary<String, Object>
    //        {
    //            [FormDataConstants.DateDeletedField] = null,
    //            [FormDataConstants.UserIDField] = expertUserID
    //        };

    //        var expertDoc = MongoDbUtil.FindDocuments(_currentFormID, expertFilter).FirstOrDefault();
    //        if (expertDoc != null)
    //            return true;

    //        var ownerFilter = new Dictionary<String, Object>
    //        {
    //            [FormDataConstants.DateDeletedField] = null,
    //            [FormDataConstants.UserIDField] = ownerUserID
    //        };

    //        var ownerProfile = MongoDbUtil.FindDocuments(ownerProfileFormID, ownerFilter).FirstOrDefault();
    //        if (ownerProfile == null)
    //            return false;

    //        var expertProfile = MongoDbUtil.FindDocuments(expertProfileFormID, expertFilter).FirstOrDefault();
    //        if (expertProfile == null)
    //            return false;

    //        var ownerFormData = BsonDocumentConverter.ConvertToFormDataUnit(ownerProfile);
    //        var expertFormData = BsonDocumentConverter.ConvertToFormDataUnit(expertProfile);

    //        var ownerProfileControlsLp = _formControls[ownerProfileFormID];
    //        var expertProfileControlsLp = _formControls[expertProfileFormID];

    //        var ownerPrimaryDirectionField = ownerProfileControlsLp["primary_direction"].FirstOrDefault();
    //        var expertPrimaryDirectionField = expertProfileControlsLp["primary_direction"].FirstOrDefault();

    //        if (ownerPrimaryDirectionField == null || expertPrimaryDirectionField == null)
    //            return false;

    //        var ownerPrimaryDirection = ownerFormData[Convert.ToString(ownerPrimaryDirectionField.ID)];
    //        var expertPrimaryDirection = expertFormData[Convert.ToString(expertPrimaryDirectionField.ID)];

    //        if (!_comparer.Equals(ownerPrimaryDirection, expertPrimaryDirection))
    //            return false;

    //        var ownerSubDirectionField = ownerProfileControlsLp["sub_direction"].FirstOrDefault();
    //        var expertSubDirectionField = ownerProfileControlsLp["sub_direction"].FirstOrDefault();

    //        if (ownerSubDirectionField == null || expertSubDirectionField == null)
    //            return false;

    //        var ownerSubDirection = ownerFormData[Convert.ToString(ownerSubDirectionField.ID)];
    //        var expertSubDirection = expertFormData[Convert.ToString(expertSubDirectionField.ID)];

    //        if (!_comparer.Equals(ownerSubDirection, expertSubDirection))
    //            return false;

    //        var ownerEmploymentGrid = ownerProfileControlsLp["current_employment"].FirstOrDefault();
    //        var expertEmploymentGrid = expertProfileControlsLp["current_employment"].FirstOrDefault();

    //        if (ownerEmploymentGrid != null && expertEmploymentGrid != null)
    //        {
    //            var ownerEmploymentVal = ownerFormData[Convert.ToString(ownerEmploymentGrid.ID)];
    //            if (ownerEmploymentVal is FormDataListRef)
    //                ownerEmploymentVal = new FormDataLazyList((FormDataListRef)ownerEmploymentVal);

    //            var expertEmploymentVal = ownerFormData[Convert.ToString(expertEmploymentGrid.ID)];
    //            if (expertEmploymentVal is FormDataListRef)
    //                expertEmploymentVal = new FormDataLazyList((FormDataListRef)expertEmploymentVal);

    //            var ownerEmploymentData = ownerEmploymentVal as FormDataLazyList;
    //            var expertEmploymentData = expertEmploymentVal as FormDataLazyList;

    //            if (ownerEmploymentData != null && expertEmploymentData != null)
    //            {
    //                var ownerGridFields = FormStructureUtil.PreOrderTraversal(ownerEmploymentGrid).ToLookup(n => n.Alias);
    //                var expertGridFields = FormStructureUtil.PreOrderTraversal(expertEmploymentGrid).ToLookup(n => n.Alias);

    //                var ownerWorkPlaceField = ownerGridFields["work_place"].FirstOrDefault();
    //                var ownerWorkDepartmentField = ownerGridFields["work_department"].FirstOrDefault();

    //                var expertWorkPlaceField = expertGridFields["work_place"].FirstOrDefault();
    //                var expertWorkDepartmentField = expertGridFields["work_department"].FirstOrDefault();

    //                if (ownerWorkPlaceField != null && ownerWorkDepartmentField != null &&
    //                    expertWorkPlaceField != null && expertWorkDepartmentField != null)
    //                {

    //                    var ownerWorkInfo = (from n in ownerEmploymentData
    //                                         let wp = n[Convert.ToString(ownerWorkPlaceField.ID)]
    //                                         let wd = n[Convert.ToString(ownerWorkDepartmentField.ID)]
    //                                         let v = $"{wp} {wd}"
    //                                         select v);

    //                    var expertWorkInfo = (from n in ownerEmploymentData
    //                                          let wp = n[Convert.ToString(expertWorkPlaceField.ID)]
    //                                          let wd = n[Convert.ToString(expertWorkDepartmentField.ID)]
    //                                          let v = $"{wp} {wd}"
    //                                          select v);

    //                    var @set = new HashSet<String>(_comparer);
    //                    @set.UnionWith(ownerWorkInfo);
    //                    @set.IntersectWith(expertWorkInfo);

    //                    if (@set.Count > 0)
    //                        return false;
    //                }

    //            }
    //        }

    //        var ownerKeywordsGrid = ownerProfileControlsLp["keywords"].FirstOrDefault();
    //        var expertKeywordsGrid = expertProfileControlsLp["keywords"].FirstOrDefault();

    //        if (ownerKeywordsGrid != null && expertKeywordsGrid != null)
    //        {
    //            var ownerKeywordsVal = ownerFormData[Convert.ToString(ownerKeywordsGrid.ID)];
    //            if (ownerKeywordsVal is FormDataListRef)
    //                ownerKeywordsVal = new FormDataLazyList((FormDataListRef)ownerKeywordsVal);

    //            var expertKeywordsVal = ownerFormData[Convert.ToString(expertKeywordsGrid.ID)];
    //            if (expertKeywordsVal is FormDataListRef)
    //                expertKeywordsVal = new FormDataLazyList((FormDataListRef)expertKeywordsVal);

    //            var ownerKeywordsData = ownerKeywordsVal as FormDataLazyList;
    //            var expertKeywordsData = expertKeywordsVal as FormDataLazyList;

    //            if (ownerKeywordsData != null && expertKeywordsData != null)
    //            {
    //                var ownerGridFields = FormStructureUtil.PreOrderTraversal(ownerKeywordsGrid).ToLookup(n => n.Alias);
    //                var expertGridFields = FormStructureUtil.PreOrderTraversal(expertKeywordsGrid).ToLookup(n => n.Alias);

    //                var ownerKeywordField = ownerGridFields["keyword"].FirstOrDefault();
    //                var expertKeywordField = expertGridFields["keyword"].FirstOrDefault();

    //                if (ownerKeywordField != null && expertKeywordField != null)
    //                {
    //                    var ownerKeywords = (from n in ownerKeywordsData
    //                                         let v = Convert.ToString(n[Convert.ToString(ownerKeywordField.ID)])
    //                                         select v).ToHashSet(_comparer);

    //                    var expertKeywords = (from n in expertKeywordsData
    //                                          let v = Convert.ToString(n[Convert.ToString(expertKeywordField.ID)])
    //                                          select v).ToHashSet(_comparer);

    //                    if (!ownerKeywords.SetEquals(expertKeywords))
    //                        return false;
    //                }
    //            }
    //        }

    //        return true;
    //    }
    //}
}