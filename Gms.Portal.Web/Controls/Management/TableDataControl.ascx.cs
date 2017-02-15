using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using DevExpress.Web;
using DevExpress.Web.Data;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class TableDataControl : BaseUserControlExtend<TableDataModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(object model)
        {
            //var dataModel = model as TableDataModel;
            //if (dataModel == null)
            //    return;

            //var logicModel = dataModel.Logic;
            //var tableModel = dataModel.Table;

            //var queryGen = new QueryGenerator(DataContext, logicModel);

            //sqlDs.ConnectionString = GetConnectionString();

            //sqlDs.InsertCommand = queryGen.InsertQuery();
            //sqlDs.UpdateCommand = queryGen.UpdateQuery();
            //sqlDs.DeleteCommand = queryGen.DeleteQuery();
            //sqlDs.SelectCommand = queryGen.SelectQuery();

            //foreach (var pair in queryGen.PrimaryColumnsParams)
            //    sqlDs.UpdateParameters.Add(pair.Key, pair.Value);

            //foreach (var pair in queryGen.PrimaryColumnsParams)
            //    sqlDs.DeleteParameters.Add(pair.Key, pair.Value);

            //sqlDs.CacheKeyDependency = sqlDs.SelectCommand.ComputeMd5();
            //sqlDs.CacheExpirationPolicy = DataSourceCacheExpiry.Sliding;
            //sqlDs.CacheDuration = 900;

            //var primaryColumns = tableModel.Columns.Where(n => n.IsPrimary).ToList();
            //if (primaryColumns.Count == 0)
            //    primaryColumns = tableModel.Columns;

            //var keyFields = String.Join(";", primaryColumns.Select(n => n.Name));
            //gvData.KeyFieldName = keyFields;
        }

        protected void gvData_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
        }

        protected void gvData_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var newValues = e.NewValues;
            var tableModel = Model.Table;

            foreach (var column in tableModel.Columns)
            {
                var name = String.Format("v{0}", column.Name.ComputeCrc16());
                var value = Convert.ToString(newValues[column.Name]);

                sqlDs.InsertParameters.Add(name, value);
            }
        }

        protected void gvData_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var newValues = e.NewValues;
            var oldValues = e.OldValues;

            var tableModel = Model.Table;

            foreach (var column in tableModel.Columns)
            {
                var name = String.Format("v{0}", column.Name.ComputeCrc16());
                var value = Convert.ToString(newValues[column.Name]);

                sqlDs.UpdateParameters.Add(name, value);
            }

            var primaryColumns = tableModel.Columns.Where(n => n.IsPrimary).ToList();
            if (primaryColumns.Count == 0)
                primaryColumns = tableModel.Columns;

            foreach (var column in primaryColumns)
            {
                var name = String.Format("w{0}", column.Name.ComputeCrc16());
                var value = Convert.ToString(oldValues[column.Name]);

                sqlDs.UpdateParameters.Add(name, value);
            }
        }

        protected void gvData_OnRowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            var values = e.Keys;

            var tableModel = Model.Table;

            var primaryColumns = tableModel.Columns.Where(n => n.IsPrimary).ToList();
            if (primaryColumns.Count == 0)
                primaryColumns = tableModel.Columns;

            foreach (var column in primaryColumns)
            {
                var name = String.Format("w{0}", column.Name.ComputeCrc16());
                var value = Convert.ToString(values[column.Name]);

                sqlDs.DeleteParameters.Add(name, value);
            }
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            gvData.AddNewRow();
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            ExportExcel();
        }

        protected void btnImport_OnClick(object sender, EventArgs e)
        {
            ImportExcel();

            Response.Redirect(Request.Url.ToString());
        }

        protected void btnTemplate_OnClick(object sender, EventArgs e)
        {
            var tableModel = Model.Table;
            var logicModel = Model.Logic;

            var dataTable = new DataTable(tableModel.Name);
            foreach (var columnModel in tableModel.Columns)
            {
                dataTable.Columns.Add(columnModel.Name);
            }

            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            var excelBytes = ExcelUtil.ConvertToExcel(dataSet);

            var period = String.Format("{0:dd.MM.yyyy_HH.mm.ss}", DateTime.Now);

            var fileName = String.Format("Template_{0}.xlsx", period);

            fileName = HttpUtility.UrlPathEncode(fileName);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", String.Format("inline;filename={0}", fileName));

            Response.BinaryWrite(excelBytes);
            Response.End();
        }

        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            ClearTable();

            Response.Redirect(Request.Url.ToString());
        }

        protected Object GetCorrectValue(Object value, SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.NVarChar:
                    return DataConverter.ToString(value);
                case SqlDbType.BigInt:
                    return DataConverter.ToNullableLong(value);
                case SqlDbType.Float:
                    return DataConverter.ToNullableDouble(value);
                case SqlDbType.DateTime:
                    return DataConverter.ToNullableDateTime(value);
                default:
                    return value;
            }
        }

        protected String GetConnectionString()
        {
            var connString = ConfigurationManager.ConnectionStrings["RepositoryConnectionString"];
            return connString.ConnectionString;
        }

        protected void ClearTable()
        {
            //var tableModel = Model.Table;
            //var logicModel = Model.Logic;

            //var queryGen = new QueryGenerator(DataContext, logicModel);

            //var connectionString = GetConnectionString();

            //using (var connection = new SqlConnection(connectionString))
            //{
            //    if (connection.State != ConnectionState.Open)
            //        connection.Open();

            //    using (var transaction = connection.BeginTransaction())
            //    {
            //        var truncateQuery = queryGen.TruncateQuery();

            //        using (var command = new SqlCommand(truncateQuery, connection, transaction))
            //        {
            //            command.ExecuteNonQuery();

            //            try
            //            {
            //                transaction.Commit();
            //            }
            //            catch (Exception)
            //            {
            //                transaction.Rollback();
            //            }
            //        }
            //    }
            //}
        }

        protected void ImportExcel()
        {
            //var tableModel = Model.Table;
            //var logicModel = Model.Logic;

            //var tableName = String.Format("#{0}", tableModel.Name.Trim());

            //var dataSet = ExcelUtil.ConvertToDataSet(fuImport.FileBytes);
            //var dataTable = dataSet.Tables[tableName];

            //var queryGen = new QueryGenerator(DataContext, logicModel);

            //var connectionString = GetConnectionString();

            //using (var connection = new SqlConnection(connectionString))
            //{
            //    if (connection.State != ConnectionState.Open)
            //        connection.Open();

            //    using (var transaction = connection.BeginTransaction())
            //    {
            //        var insertQuery = queryGen.InsertQuery();

            //        using (var command = new SqlCommand(insertQuery, connection, transaction))
            //        {
            //            foreach (var columnModel in tableModel.Columns)
            //            {
            //                var columnName = queryGen.AllColumns[columnModel.Name.Trim()];
            //                var paramName = queryGen.AllColumnsParams[columnName];
            //                var dbType = queryGen.DbTypes[columnName];

            //                var param = command.CreateParameter();
            //                param.ParameterName = paramName;
            //                param.SqlDbType = dbType;
            //                param.IsNullable = true;

            //                command.Parameters.Add(param);
            //            }

            //            foreach (DataRow dataRow in dataTable.Rows)
            //            {
            //                foreach (var columnModel in tableModel.Columns)
            //                {
            //                    var dataColumnName = String.Format("#{0}", columnModel.Name.Trim());

            //                    var columnName = queryGen.AllColumns[columnModel.Name.Trim()];
            //                    var columnValue = dataRow[dataColumnName];

            //                    var paramName = queryGen.AllColumnsParams[columnName];
            //                    var sqlParam = command.Parameters[paramName];

            //                    sqlParam.Value = GetCorrectValue(columnValue, sqlParam.SqlDbType);
            //                }

            //                command.ExecuteNonQuery();
            //            }

            //            try
            //            {
            //                transaction.Commit();
            //            }
            //            catch (Exception)
            //            {
            //                transaction.Rollback();
            //            }
            //        }
            //    }
            //}
        }

        protected void ExportExcel()
        {
            var dataView = (DataView)sqlDs.Select(new DataSourceSelectArguments());
            if (dataView == null)
                return;

            var tableModel = Model.Table;
            var logicModel = Model.Logic;

            var dataTable = dataView.ToTable();
            dataTable.TableName = tableModel.Name;

            var excelBytes = ExcelUtil.ConvertToExcel(dataTable);

            var period = String.Format("{0:dd.MM.yyyy_HH.mm.ss}", DateTime.Now);

            var fileName = String.Format("Data_{0}.xlsx", period);
            fileName = HttpUtility.UrlPathEncode(fileName);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", String.Format("inline;filename={0}", fileName));

            Response.BinaryWrite(excelBytes);
            Response.End();
        }

    }
}