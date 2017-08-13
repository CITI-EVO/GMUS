using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class LogicControl : BaseUserControlExtend<LogicModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var model = Model;

            FillLists(model);
            ApplyViewMode(model);
        }

        private void FillLists(LogicModel model)
        {
            if (model.SourceType == "Database")
            {
                var dataSources = LoadDataSources().OrderBy(n => n.Key);
                cbxSource.BindData(dataSources);
            }

            if (model.SourceType == "Logic")
            {
                var query = (from n in HbSession.Query<GM_Logic>()
                             where n.DateDeleted == null
                             orderby n.Name
                             select new KeyValueEntity
                             {
                                 Key = n.ID,
                                 Value = n.Name,
                             });

                var list = query.ToList();

                cbxSource.BindData(list);
            }

            cbxSource.TrySetSelectedValue(model.SourceID);
        }

        private void ApplyViewMode(LogicModel model)
        {
            if (model.Type == "Logic")
            {
                pnlLogic.Visible = true;
                pnlQuery.Visible = false;
            }
            else if (model.Type == "Query")
            {
                pnlLogic.Visible = false;
                pnlQuery.Visible = true;
            }
        }

        public override void SetModel(object model)
        {
            var logicModel = (LogicModel)model;

            FillLists(logicModel);
            ApplyViewMode(logicModel);
        }

        protected IEnumerable<KeyValueEntity> LoadDataSources()
        {
            var collectionsDataSource = (from n in HbSession.Query<GM_Collection>()
                                         where n.DateDeleted == null
                                         select n);

            foreach (var item in collectionsDataSource)
            {
                var entity = new KeyValueEntity
                {
                    Key = item.Name,
                    Value = $"{item.ID}/@"
                };

                yield return entity;
            }

            var formsDataSource = (from n in HbSession.Query<GM_Form>()
                                   where n.DateDeleted == null
                                   select n);

            var formConverter = new FormEntityModelConverter(HbSession);

            foreach (var item in formsDataSource)
            {
                var entity = new KeyValueEntity
                {
                    Key = item.Name,
                    Value = $"{item.ID}/@"
                };

                yield return entity;

                var model = formConverter.Convert(item);
                if (model.Entity != null)
                {
                    var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(model.Entity);

                    var treesAndGrids = (from n in allControls
                                         where n is GridEntity || n is TreeEntity
                                         select n);

                    foreach (var control in treesAndGrids)
                    {
                        var subEntity = new KeyValueEntity
                        {
                            Key = $"{item.Name}/{control.Name} - {control.Alias}",
                            Value = $"{item.ID}/{control.ID}"
                        };

                        yield return subEntity;
                    }
                }
            }

            var servicesDataSource = (from n in HbSession.Query<GM_Service>()
                                      where n.DateDeleted == null
                                      select n);

            var serviceConverter = new ServiceEntityModelConverter(HbSession);

            foreach (var item in servicesDataSource)
            {
                var model = serviceConverter.Convert(item);
                if (model.Entity != null)
                {
                    foreach (var method in model.Entity.Methods)
                    {
                        var subEntity = new KeyValueEntity
                        {
                            Key = $"{item.Name}/{method.Name}",
                            Value = $"{item.ID}/{method.ID}"
                        };

                        yield return subEntity;
                    }
                }
            }
        }

    }
}