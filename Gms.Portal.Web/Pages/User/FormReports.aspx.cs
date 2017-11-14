using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Wrappers;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormReports : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillLogics();
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            FillGrid();
        }
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            var selectedItem = cbxLogics.SelectedItem;
            if (selectedItem == null)
                return;

            FillGrid();

            Response.ClearContent();
            Response.AddHeader("content-disposition", $"attachment; filename={selectedItem.Text}.xls");
            Response.ContentType = "application/excel";

            var sw = new StringWriter();

            var htw = new HtmlTextWriter(sw);
            gvData.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }

        private void FillLogics()
        {
            var entities = (from n in HbSession.Query<GM_Logic>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            cbxLogics.DataSource = entities;
            cbxLogics.DataBind();
        }
        private void FillGrid()
        {
            var logicID = DataConverter.ToNullableGuid(cbxLogics.SelectedValue);

            var entity = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == logicID);
            if (entity == null)
                return;

            var dataSource = GetDataSourceID(entity);
            if (String.IsNullOrWhiteSpace(dataSource))
                return;

            var contentEntity = GetContant(entity);

            var fieldsList = FormStructureUtil.PreOrderTraversal(contentEntity).OfType<FieldEntity>().ToList();
            var fieldsDict = fieldsList.ToDictionary(n => Convert.ToString(n.ID));

            var converter = new LogicEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            var collection = MongoDbUtil.GetCollection(dataSource);

            if (model.Type == "Query")
            {
                var query = model.Query;

                foreach (var fieldEntity in fieldsList)
                {
                    if (!String.IsNullOrWhiteSpace(fieldEntity.Name))
                        query = query.Replace($"\"{fieldEntity.Name}\"", $"\"{fieldEntity.ID}\"");

                    if (!String.IsNullOrWhiteSpace(fieldEntity.Alias))
                        query = query.Replace($"\"{fieldEntity.Alias}\"", $"\"{fieldEntity.ID}\"");
                }

                var docQuery = BsonSerializer.Deserialize<BsonDocument>(query);

                var queryDoc = new QueryDocument(docQuery);
                var fluent = collection.Find(queryDoc);

                var documents = GetDocuments(fluent);

                var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(documents);

                var formFields = fieldsList.Select(n => Convert.ToString(n.ID)).Union(FormDataBase.DefaultFields);
                var fieldSet = formFields.ToHashSet();

                var formDataView = new DictionaryDataView(formDataUnits, fieldSet);

                gvData.Columns.Clear();

                foreach (var field in fieldSet)
                {
                    var fieldEntity = fieldsDict.GetValueOrDefault(field);
                    if (fieldEntity == null)
                    {
                        var boundField = new GridViewMetaBoundField(field);
                        //gvData.Columns.Add(boundField);
                    }
                    else if (fieldEntity.Alias == "@System")
                    {
                        var boundField = new GridViewMetaBoundField(fieldEntity.Name)
                        {
                            HeaderText = fieldEntity.Name
                        };

                        gvData.Columns.Add(boundField);
                    }
                    else
                    {
                        var boundField = new GridViewMetaBoundField(null, null, fieldEntity, contentEntity);
                        gvData.Columns.Add(boundField);
                    }
                }

                gvData.DataSource = formDataView;
                gvData.DataBind();
            }
            else
            {
                var sortBuilder = Builders<BsonDocument>.Sort;
                var sortDef = (SortDefinition<BsonDocument>)null;

                var filterBuilder = Builders<BsonDocument>.Filter;
                var filterDef = filterBuilder.Empty;

                var projectionBuilder = Builders<BsonDocument>.Projection;
                var projectionDef = (ProjectionDefinition<BsonDocument>)null;

                var fieldsSet = new HashSet<String>();
                fieldsSet.UnionWith(FormDataBase.DefaultFields);

                var expLogic = model.ExpressionsLogic;
                if (expLogic.FilterBy != null && expLogic.FilterBy.Expressions != null)
                {
                    var expressions = (from n in expLogic.FilterBy.Expressions
                                       let c = CorrectExpressionNames(fieldsList, n.Expression)
                                       select c);

                    var allCondidions = String.Join(",", expressions);
                    var jsonText = String.Concat("{", allCondidions, "}");

                    var jsonFilterDef = new JsonFilterDefinition<BsonDocument>(jsonText);
                    filterDef = filterDef & jsonFilterDef;
                }

                var fluent = collection.Find(filterDef);

                if (expLogic.OrderBy != null && expLogic.OrderBy.Expressions != null)
                {
                    foreach (var item in expLogic.OrderBy.Expressions)
                    {
                        if (!RegexUtil.SortingFieldsParserRx.IsMatch(item.Expression))
                            continue;

                        var sortMatch = RegexUtil.SortingFieldsParserRx.Match(item.Expression);

                        var name = sortMatch.Groups["name"].Value;
                        var type = sortMatch.Groups["type"].Value;

                        name = CorrectExpressionNames(fieldsList, name);

                        if (sortDef == null)
                        {
                            if (type == "desc")
                                sortDef = sortBuilder.Descending(name);
                            else
                                sortDef = sortBuilder.Ascending(name);
                        }
                        else
                        {
                            if (type == "desc")
                                sortDef = sortDef.Descending(name);
                            else
                                sortDef = sortDef.Ascending(name);
                        }
                    }
                }

                if (sortDef != null)
                    fluent = fluent.Sort(sortDef);

                if (expLogic.Select != null && expLogic.Select.Expressions != null)
                {
                    foreach (var item in expLogic.Select.Expressions)
                    {
                        var expression = CorrectExpressionNames(fieldsList, item.Expression).Trim('\"');

                        //if (projectionDef == null)
                        //    projectionDef = projectionBuilder.Include(expression);
                        //else
                        //    projectionDef = projectionDef.Include(expression);

                        fieldsSet.Add(expression);
                    }
                }

                if (projectionDef != null)
                    fluent = fluent.Project(projectionDef);
                else
                    fieldsSet.UnionWith(fieldsDict.Keys);

                gvData.Columns.Clear();

                foreach (var field in fieldsSet)
                {
                    var fieldEntity = fieldsDict.GetValueOrDefault(field);
                    if (fieldEntity == null)
                    {
                        var boundField = new GridViewMetaBoundField(field);
                        //gvData.Columns.Add(boundField);
                    }
                    else if (fieldEntity.Alias == "@System")
                    {
                        var boundField = new GridViewMetaBoundField(fieldEntity.Name)
                        {
                            HeaderText = fieldEntity.Name
                        };

                        gvData.Columns.Add(boundField);
                    }
                    else
                    {
                        var boundField = new GridViewMetaBoundField(null, null, fieldEntity, contentEntity);
                        gvData.Columns.Add(boundField);
                    }
                }

                var documents = GetDocuments(fluent);
                var formDataUnits = BsonDocumentConverter.ConvertToFormDataUnit(documents);

                var formDataView = new DictionaryDataView(formDataUnits, fieldsSet);

                gvData.DataSource = formDataView;
                gvData.DataBind();
            }
        }

        private String GetDataSourceID(GM_Logic entity)
        {
            var dsMatch = RegexUtil.DataSourceParserRx.Match(entity.SourceID);
            if (!dsMatch.Success)
                return entity.SourceID;

            var formID = DataConverter.ToNullableGuid(dsMatch.Groups["parentID"]);
            var childID = DataConverter.ToNullableGuid(dsMatch.Groups["childID"]);

            if (formID == null && childID == null)
                return entity.SourceID;

            var ownerID = (childID ?? formID);

            var collName = $"Collection_{ownerID:n}";
            return collName;
        }

        protected String CorrectExpressionNames(IEnumerable<FieldEntity> fields, String text)
        {
            foreach (var entity in fields)
            {
                if (!String.IsNullOrWhiteSpace(entity.Name))
                    text = text.Replace($"\"{entity.Name}\"", $"\"{entity.ID}\"");

                if (!String.IsNullOrWhiteSpace(entity.Alias))
                    text = text.Replace($"\"{entity.Alias}\"", $"\"{entity.ID}\"");
            }

            return text;
        }

        protected ContentEntity GetContant(GM_Logic dbEntity)
        {
            if (dbEntity.SourceID == MongoDbUtil.MonitoringBudgetCollectionName)
            {
                var content = new ContentEntity
                {
                    Name = MongoDbUtil.MonitoringBudgetCollectionName,
                    Controls = new List<ControlEntity>()
                };

                var @set = new HashSet<String>
                {
                    "ID",
                    "OwnerID",
                    "RecordID",
                    "Goal",
                    "ParagraphID",
                    "DateOfTransfer",
                    "Remain" ,
                    "Incoming" ,
                    "Outgoing" ,
                    "CreateUserID",
                    "StatusID",
                    "StatusDate",
                    "StatusUserID",
                    "Comment" ,
                    "PaymentType",
                    "OrganizationID",
                    "OrganizationType",
                    "DateCreated",
                    "DateChanged",
                    "DateDeleted",
                };

                foreach (var name in @set)
                {
                    var field = new FieldEntity
                    {
                        ID = name.ComputeMd5Guid(),
                        Name = name,
                        Alias = "@System",
                    };

                    content.Controls.Add(field);
                }

                return content;
            }

            var dsMatch = RegexUtil.DataSourceParserRx.Match(dbEntity.SourceID);
            if (!dsMatch.Success)
                return null;

            var formID = DataConverter.ToNullableGuid(dsMatch.Groups["parentID"]);
            var childID = DataConverter.ToNullableGuid(dsMatch.Groups["childID"]);

            if (formID == null && childID == null)
                return null;

            var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);

            var converter = new FormEntityModelConverter(HbSession);

            var model = converter.Convert(dbForm);

            if (childID != null)
            {
                var entities = FormStructureUtil.PreOrderTraversal(model.Entity);

                var entity = entities.OfType<ContentEntity>().FirstOrDefault(n => n.ID == childID);
                return entity;
            }

            return model.Entity;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "  
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }

        protected IEnumerable<BsonDocument> GetDocuments(IFindFluent<BsonDocument, BsonDocument> fluent)
        {
            using (var cursor = fluent.ToCursor())
            {
                while (cursor.MoveNext())
                {
                    var batch = cursor.Current;

                    foreach (var document in batch)
                        yield return document;
                }
            }
        }
    }
}