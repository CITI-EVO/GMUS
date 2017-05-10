using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using DevExpress.Office.Utils;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Utils
{
    public class NVelocityUtil
    {
        public Boolean IsValueList(object value)
        {
            if (value is FormDataListRef || value is FormDataListBase)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<ControlEntity> GetGridColumns(IEnumerable<ControlEntity> controls, string name)
        {
            var control = controls.Where(n => n is GridEntity).FirstOrDefault(n => n.Name == name);
            if (control is GridEntity)
            {
                var grid = control as GridEntity;
                return grid.Controls;
            }
            return Enumerable.Empty<ControlEntity>();
        }

        public FormDataListBase GetGridData(object gridData)
        {
            FormDataListBase formDataList;

            if (gridData is FormDataListRef)
                formDataList = new FormDataLazyList((FormDataListRef)gridData);
            else
                formDataList = (FormDataListBase)gridData;

            return formDataList;
        }

        public String Translate(string value, string language)
        {
            //if (value != null)
            //{
            //    var result = TranslationUtil.GetTranslatedText(Convert.ToString(value));
            //    return result;
            //}
            return value;
        }
    }
}