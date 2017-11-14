using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class SummaryBudgetDataControl : BaseUserControlExtend<SummaryBudgetDataModel>
    {
        private SummaryBudgetEntity _summaryEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var dataGridRow = e.Row;
            if (dataGridRow.RowType == DataControlRowType.DataRow)
            {
                var item = dataGridRow.DataItem as SummaryBudgetEntity;
                if (item != null)
                {
                    _summaryEntity = (_summaryEntity ?? new SummaryBudgetEntity());

                    _summaryEntity.PlanGrossing = _summaryEntity.PlanGrossing.GetValueOrDefault() + item.PlanGrossing;
                    _summaryEntity.PlanCurrent = _summaryEntity.PlanCurrent.GetValueOrDefault() + item.PlanCurrent;

                    _summaryEntity.EnrollGrossing = _summaryEntity.EnrollGrossing.GetValueOrDefault() + item.EnrollGrossing;
                    _summaryEntity.EnrollCurrent = _summaryEntity.EnrollCurrent.GetValueOrDefault() + item.EnrollCurrent;

                    _summaryEntity.RemainPrevious = _summaryEntity.RemainPrevious.GetValueOrDefault() + item.RemainPrevious;
                    _summaryEntity.RemainCurrent = _summaryEntity.RemainCurrent.GetValueOrDefault() + item.RemainCurrent;

                    _summaryEntity.CashExpenseGrossing = _summaryEntity.CashExpenseGrossing.GetValueOrDefault() + item.CashExpenseGrossing;
                    _summaryEntity.CashExpenseCurrent = _summaryEntity.CashExpenseCurrent.GetValueOrDefault() + item.CashExpenseCurrent;

                    _summaryEntity.ConfirmedExpenseGrossing = _summaryEntity.ConfirmedExpenseGrossing.GetValueOrDefault() + item.ConfirmedExpenseGrossing;
                    _summaryEntity.ConfirmedExpenseCurrent = _summaryEntity.ConfirmedExpenseCurrent.GetValueOrDefault() + item.ConfirmedExpenseCurrent;

                    _summaryEntity.UnconfirmedExpenseGrossing = _summaryEntity.UnconfirmedExpenseGrossing.GetValueOrDefault() + item.UnconfirmedExpenseGrossing;
                    _summaryEntity.UnconfirmedExpenseCurrent = _summaryEntity.UnconfirmedExpenseCurrent.GetValueOrDefault() + item.UnconfirmedExpenseCurrent;
                }
            }
            else if (dataGridRow.RowType == DataControlRowType.Footer)
            {
                if (_summaryEntity != null)
                {
                    var controls = UserInterfaceUtil.TraverseControls(dataGridRow);
                    var controlsLp = controls.ToLookup(n => n.ID);

                    var lblPlanGrossing = controlsLp["lblPlanGrossing"].FirstOrDefault() as Label;
                    if (lblPlanGrossing != null)
                        lblPlanGrossing.Text = Convert.ToString(_summaryEntity.PlanGrossing);

                    var lblPlanCurrent = controlsLp["lblPlanCurrent"].FirstOrDefault() as Label;
                    if (lblPlanCurrent != null)
                        lblPlanCurrent.Text = Convert.ToString(_summaryEntity.PlanCurrent);

                    var lblEnrollGrossing = controlsLp["lblEnrollGrossing"].FirstOrDefault() as Label;
                    if (lblEnrollGrossing != null)
                        lblEnrollGrossing.Text = Convert.ToString(_summaryEntity.EnrollGrossing);

                    var lblEnrollCurrent = controlsLp["lblEnrollCurrent"].FirstOrDefault() as Label;
                    if (lblEnrollCurrent != null)
                        lblEnrollCurrent.Text = Convert.ToString(_summaryEntity.EnrollCurrent);

                    var lblRemainPrevious = controlsLp["lblRemainPrevious"].FirstOrDefault() as Label;
                    if (lblRemainPrevious != null)
                        lblRemainPrevious.Text = Convert.ToString(_summaryEntity.RemainPrevious);

                    var lblRemainCurrent = controlsLp["lblRemainCurrent"].FirstOrDefault() as Label;
                    if (lblRemainCurrent != null)
                        lblRemainCurrent.Text = Convert.ToString(_summaryEntity.RemainCurrent);

                    var lblCashExpenseGrossing = controlsLp["lblCashExpenseGrossing"].FirstOrDefault() as Label;
                    if (lblCashExpenseGrossing != null)
                        lblCashExpenseGrossing.Text = Convert.ToString(_summaryEntity.CashExpenseGrossing);

                    var lblCashExpenseCurrent = controlsLp["lblCashExpenseCurrent"].FirstOrDefault() as Label;
                    if (lblCashExpenseCurrent != null)
                        lblCashExpenseCurrent.Text = Convert.ToString(_summaryEntity.CashExpenseCurrent);

                    var lblConfirmedExpenseGrossing = controlsLp["lblConfirmedExpenseGrossing"].FirstOrDefault() as Label;
                    if (lblConfirmedExpenseGrossing != null)
                        lblConfirmedExpenseGrossing.Text = Convert.ToString(_summaryEntity.ConfirmedExpenseGrossing);

                    var lblConfirmedExpenseCurrent = controlsLp["lblConfirmedExpenseCurrent"].FirstOrDefault() as Label;
                    if (lblConfirmedExpenseCurrent != null)
                        lblConfirmedExpenseCurrent.Text = Convert.ToString(_summaryEntity.ConfirmedExpenseCurrent);

                    var lblUnconfirmedExpenseGrossing = controlsLp["lblUnconfirmedExpenseGrossing"].FirstOrDefault() as Label;
                    if (lblUnconfirmedExpenseGrossing != null)
                        lblUnconfirmedExpenseGrossing.Text = Convert.ToString(_summaryEntity.UnconfirmedExpenseGrossing);

                    var lblUnconfirmedExpenseCurrent = controlsLp["lblUnconfirmedExpenseCurrent"].FirstOrDefault() as Label;
                    if (lblUnconfirmedExpenseCurrent != null)
                        lblUnconfirmedExpenseCurrent.Text = Convert.ToString(_summaryEntity.UnconfirmedExpenseCurrent);
                }
            }
        }

        public void BindData(DateTime? startDate, DateTime? endDate, String reqPayment, FormEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<IDictionary<String, Object>> transfers)
        {
            var entities = transfers.Select(n => new MoneyTransferEntity(n));
            BindData(startDate, endDate, reqPayment, formEntity, formData, paragraphs, entities);
        }
        public void BindData(DateTime? startDate, DateTime? endDate, String reqPayment, ContentEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MoneyTransferEntity> transfers)
        {
            var entities = GetSummaryGridData(startDate, endDate, reqPayment, formEntity, formData, paragraphs, transfers);

            gvData.DataSource = entities;
            gvData.DataBind();
        }

        public DataTable GetDataTable(DateTime? startDate, DateTime? endDate, String reqPayment, ContentEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<IDictionary<String, Object>> transfers)
        {
            var entities = transfers.Select(n => new MoneyTransferEntity(n));
            return GetDataTable(startDate, endDate, reqPayment, formEntity, formData, paragraphs, entities);
        }
        public DataTable GetDataTable(DateTime? startDate, DateTime? endDate, String reqPayment, ContentEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MoneyTransferEntity> transfers)
        {
            var entities = GetSummaryGridData(startDate, endDate, reqPayment, formEntity, formData, paragraphs, transfers);

            var dataTable = entities.ToDataTable();
            return dataTable;
        }

        public IEnumerable<SummaryBudgetEntity> GetSummaryGridData(DateTime? startDate, DateTime? endDate, String reqPayment, ContentEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<IDictionary<String, Object>> transfers)
        {
            var entities = transfers.Select(n => new MoneyTransferEntity(n));
            return GetSummaryGridData(startDate, endDate, reqPayment, formEntity, formData, paragraphs, entities);
        }
        public IEnumerable<SummaryBudgetEntity> GetSummaryGridData(DateTime? startDate, DateTime? endDate, String reqPayment, ContentEntity formEntity, FormDataUnit formData, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MoneyTransferEntity> transfers)
        {
            if (formEntity == null)
                return null;

            var comparer = StringLogicalComparer.OrdinalIgnoreCase;
            var controls = FormStructureUtil.PreOrderTraversal(formEntity);

            var controlsLp = controls.ToLookup(n => n.Alias, comparer);

            var budgetControlsQuery = GetBudgetControls(controls, paragraphs);

            var budgetControlsList = budgetControlsQuery.ToList();
            if (budgetControlsList.Count == 0)
                lblError.Text = "Unable to find any ProjectBudget";

            var budgetFullDataQuery = (from n in budgetControlsList
                                       let data = GetBudgetDataGrid(n, formData)
                                       let fields = GetBudgetAmountFields(n.Content)
                                       select new SummaryBudgetDataEntity
                                       {
                                           Key = n.Key,
                                           Name = n.Name,
                                           Data = data,
                                           Fields = fields,
                                       });

            var budgetFullDataDict = budgetFullDataQuery.ToDictionary(n => n.Key);

            var transfersLp = transfers.ToLookup(n => n.ParagraphID);

            var projectStart = GetProjectStartDate(controlsLp, formData);
            if (startDate == null)
                startDate = projectStart;

            var currentPeriod = GetPeriodOfDate(controlsLp, formData, projectStart, endDate);

            var acceptedStatus = MonitoringItemStatuses.Accepted;

            var query = (from n in paragraphs
                         where n.Container

                         let entity = budgetFullDataDict.GetValueOrDefault(n.ID)

                         let grossingPlan = GetBudgetDataSum(entity, reqPayment, currentPeriod, true)
                         let currentPlan = GetBudgetDataSum(entity, reqPayment, currentPeriod, false)

                         let paragraphTransfers = transfersLp[n.ID]

                         let grossingEnroll = GetEnrollSum(paragraphTransfers, projectStart, endDate)
                         let currentEnroll = GetEnrollSum(paragraphTransfers, startDate, endDate)

                         let grossingCashExpense = GetExpenseSum(paragraphTransfers, projectStart, endDate, null, null)
                         let currentCashExpense = GetExpenseSum(paragraphTransfers, startDate, endDate, null, null)

                         let grossingConfirmedExpense = GetExpenseSum(paragraphTransfers, projectStart, endDate, acceptedStatus, true)
                         let currentConfirmedExpense = GetExpenseSum(paragraphTransfers, startDate, endDate, acceptedStatus, true)

                         let grossingUnconfirmedExpense = GetExpenseSum(paragraphTransfers, projectStart, endDate, acceptedStatus, false)
                         let currentUnconfirmedExpense = GetExpenseSum(paragraphTransfers, startDate, endDate, acceptedStatus, false)

                         let previousEnroll = GetEnrollSum(paragraphTransfers, projectStart, startDate)
                         let previousCashExpense = GetExpenseSum(paragraphTransfers, projectStart, startDate, null, null)

                         let remainPrevious = (previousEnroll - previousCashExpense)
                         let remainCurrent = (grossingEnroll - grossingCashExpense)

                         select new SummaryBudgetEntity
                         {
                             Paragraph = n.Name,

                             RemainPrevious = remainPrevious,
                             RemainCurrent = remainCurrent,

                             PlanGrossing = grossingPlan,
                             PlanCurrent = currentPlan,

                             EnrollGrossing = grossingEnroll,
                             EnrollCurrent = currentEnroll,

                             CashExpenseGrossing = grossingCashExpense,
                             CashExpenseCurrent = currentCashExpense,

                             ConfirmedExpenseGrossing = grossingConfirmedExpense,
                             ConfirmedExpenseCurrent = currentConfirmedExpense,

                             UnconfirmedExpenseGrossing = grossingUnconfirmedExpense,
                             UnconfirmedExpenseCurrent = currentUnconfirmedExpense,
                         });

            return query;
        }

        protected IEnumerable<BudgetControlEntity> GetBudgetControls(IEnumerable<ControlEntity> controls, IEnumerable<BudgetParagraphEntity> paragraphs)
        {
            var controlsDict = controls.ToDictionary(n => (Guid?)n.ID);

            foreach (var item in paragraphs)
            {
                ControlEntity control;
                if (item.ID == null || !controlsDict.TryGetValue(item.ID, out control))
                {
                    if (item.ContentID == null || !controlsDict.TryGetValue(item.ContentID, out control))
                        continue;
                }

                var content = control as ContentEntity;
                if (content == null)
                    continue;

                if (IsBudgetTreeOrGrid(content))
                {
                    var field = controlsDict.GetValueOrDefault(item.FieldID) as FieldEntity;

                    var result = new BudgetControlEntity
                    {
                        Key = item.ID,
                        Name = item.Name,
                        Field = field,
                        Content = content,
                        DependOnRow = true,
                        DependOnValue = item.DependOnValue
                    };

                    yield return result;
                }
                else
                {
                    foreach (var entity in content.Controls)
                    {
                        if (!IsBudgetTreeOrGrid(entity))
                            continue;

                        var result = new BudgetControlEntity
                        {
                            Key = item.ID,
                            Name = entity.Name,
                            Content = (ContentEntity)entity,
                        };

                        yield return result;
                    }
                }
            }
        }

        protected double? GetExpenseSum(IEnumerable<MoneyTransferEntity> transfers, DateTime? startDate, DateTime? endDate, String status, bool? match)
        {
            var query = (from n in transfers
                         where n.Outgoing != null &&
                               n.Incoming == null
                         select n);

            if (match != null)
            {
                if (match.GetValueOrDefault())
                {
                    query = (from n in query
                             where n.Status == status
                             select n);
                }
                else
                {
                    query = (from n in query
                             where n.Status != status
                             select n);
                }
            }

            if (startDate == null && endDate != null)
            {
                query = (from n in query
                         where GmsCommonUtil.DateLessOrEquals(n.DateOfTransfer, endDate)
                         select n);
            }
            else if (startDate != null && endDate != null)
            {
                query = (from n in query
                         where GmsCommonUtil.DateBetween(n.DateOfTransfer, startDate, endDate)
                         select n);
            }

            var value = query.Sum(n => n.Outgoing);
            return value;
        }

        protected double? GetExpenseSum(IEnumerable<MoneyTransferEntity> transfers, DateTime? endDate)
        {
            if (endDate == null)
                return 0D;

            var query = (from n in transfers
                         where n.Outgoing != null &&
                               n.Incoming == null &&
                               GmsCommonUtil.DateLessOrEquals(n.DateOfTransfer, endDate)
                         select n);

            var value = query.Sum(n => n.Outgoing);
            return value;
        }

        protected double? GetEnrollSum(IEnumerable<MoneyTransferEntity> transfers, DateTime? startDate, DateTime? endDate)
        {
            var query = (from n in transfers
                         where n.Incoming != null &&
                               n.Outgoing == null
                         select n);

            if (startDate == null && endDate != null)
            {
                query = (from n in query
                         where GmsCommonUtil.DateLessOrEquals(n.DateOfTransfer, endDate)
                         select n);
            }
            else if (startDate != null && endDate != null)
            {
                query = (from n in query
                         where GmsCommonUtil.DateBetween(n.DateOfTransfer, startDate, endDate)
                         select n);

            }

            var value = query.Sum(n => n.Incoming);
            return value;
        }

        protected double? GetEnrollSum(IEnumerable<MoneyTransferEntity> transfers, DateTime? endDate)
        {
            if (endDate == null)
                return 0D;

            var query = (from n in transfers
                         where n.Incoming != null &&
                               n.Outgoing == null &&
                               GmsCommonUtil.DateLessOrEquals(n.DateOfTransfer, endDate)
                         select n);

            var value = query.Sum(n => n.Incoming);
            return value;
        }

        protected int? GetPeriodOfDate(ILookup<String, ControlEntity> controlsLp, FormDataUnit formData, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
                return null;

            var periodLengthField = controlsLp["PeriodLength"].FirstOrDefault();
            if (periodLengthField == null)
            {
                lblError.Text = "Unable to find PeriodLength";
                return null;
            }

            var periodLengthKey = Convert.ToString(periodLengthField.ID);

            var periodLength = DataConverter.ToNullableInt(formData[periodLengthKey]);
            if (periodLength == null)
                return null;

            var periodStartDateFields = (from n in controlsLp
                                         where n.Key != null && RegexUtil.ProjectPeriodStartDateRx.IsMatch(n.Key)
                                         orderby n.Key
                                         select n);

            foreach (var periodStartDateFieldsGrp in periodStartDateFields)
            {
                var periodStartDateField = periodStartDateFieldsGrp.FirstOrDefault();
                if (periodStartDateField == null)
                    continue;

                var periodStartDateKey = Convert.ToString(periodStartDateField.ID);

                var periodStartDate = DataConverter.ToNullableDateTime(formData[periodStartDateKey]);
                if (periodStartDate == null)
                    continue;

                var periodEndDate = periodStartDate.Value.AddMonths(periodLength.Value);
                if (GmsCommonUtil.DateBetween(startDate, periodStartDate, periodEndDate) &&
                    GmsCommonUtil.DateBetween(endDate, periodStartDate, periodEndDate))
                {
                    var match = RegexUtil.ProjectPeriodStartDateRx.Match(periodStartDateFieldsGrp.Key);

                    var index = DataConverter.ToNullableInt(match.Groups["index"].Value);
                    return index;
                }
            }

            var difference = MonthDifference(startDate, endDate);
            if (difference < 1)
                return difference;

            int rem;
            var period = Math.DivRem(difference, periodLength.Value, out rem);
            if (rem > 0)
                period += 1;

            return period;
        }

        protected DateTime? GetProjectStartDate(ILookup<String, ControlEntity> controlsLp, FormDataUnit formData)
        {
            var projectStartDateField = controlsLp["ProjectStartDate"].FirstOrDefault();
            if (projectStartDateField == null)
            {
                lblError.Text = "Unable to find ProjectStartDate";
                return null;
            }

            var projectStartDateKey = Convert.ToString(projectStartDateField.ID);

            var projectStartDate = DataConverter.ToNullableDateTime(formData[projectStartDateKey]);
            return projectStartDate;
        }

        protected FormDataListBase GetBudgetDataGrid(BudgetControlEntity entity, FormDataUnit formData)
        {
            var key = Convert.ToString(entity.Content.ID);

            var dataList = GetBudgetDataGrid(formData[key]);
            if (dataList == null)
                return null;

            if (entity.DependOnRow && entity.DependOnValue != null)
            {
                var fieldKey = Convert.ToString(entity.Field.ID);

                var query = (from n in dataList
                             where Equals(n[fieldKey], entity.DependOnValue)
                             select n);

                dataList = new FormDataListBase(Guid.Empty);

                foreach (var item in query)
                    dataList.Add(item);
            }

            return dataList;
        }

        protected double? GetBudgetDataSum(SummaryBudgetDataEntity entity, String reqPayment, int? period, bool gross)
        {
            if (entity == null || entity.Data == null)
                return null;

            var amountFields = entity.Fields.AsEnumerable();
            if (entity.Fields.Count == 0)
            {
                lblError.Text = "Unable to find any ProjectBudget_Amount";
                return null;
            }

            if (!String.IsNullOrWhiteSpace(reqPayment))
            {
                var comparer = StringComparer.OrdinalIgnoreCase;

                amountFields = (from n in entity.Fields
                                where comparer.Equals(n.Category, reqPayment)
                                select n);
            }

            if (period != null)
            {
                if (gross)
                {
                    amountFields = (from n in amountFields
                                    where n.OrderIndex <= period
                                    select n);
                }
                else
                {
                    amountFields = (from n in amountFields
                                    where n.OrderIndex == period
                                    select n);
                }
            }

            var query = (from n in entity.Data
                         from m in amountFields
                         let k = Convert.ToString(m.Control.ID)
                         let v = DataConverter.ToNullableDouble(n[k])
                         select v);

            return query.Sum();
        }

        protected IList<BudgetAmountFieldEntity> GetBudgetAmountFields(ContentEntity entity)
        {
            var regex = RegexUtil.BudgetAmountParserRx;

            var query = from c in entity.Controls
                        let alias = (c.Alias ?? String.Empty)
                        where regex.IsMatch(alias)
                        let match = regex.Match(alias)
                        let type = match.Groups["type"].Value
                        let index = DataConverter.ToNullableInt(match.Groups["index"].Value)
                        select new BudgetAmountFieldEntity
                        {
                            Category = type,
                            ParentID = entity.ID,
                            OrderIndex = index,
                            Control = c
                        };

            return query.ToList();
        }

        protected FormDataListBase GetBudgetDataGrid(Object value)
        {
            if (value == null)
                return null;

            var dataList = value as FormDataListBase;
            if (dataList != null)
                return (FormDataListBase)value;

            var listRef = value as FormDataListRef;
            if (listRef != null)
                return new FormDataLazyList(listRef);

            return null;
        }

        protected bool IsBudgetTreeOrGrid(ControlEntity control)
        {
            if ((control is GridEntity || control is TreeEntity) &&
                !String.IsNullOrWhiteSpace(control.Alias) &&
                control.Alias.StartsWith("ProjectBudget", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        protected int MonthDifference(DateTime? x, DateTime? y)
        {
            var xVal = x.GetValueOrDefault();
            var yVal = y.GetValueOrDefault();

            var xTotal = xVal.Year * 12 + xVal.Month;
            var yTotal = yVal.Year * 12 + yVal.Month;

            var diff = xTotal - yTotal;

            if (xVal.Day < yVal.Day)
                diff += 1;

            return Math.Abs(diff);
        }
    }
}