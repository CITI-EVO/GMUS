using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.EventStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class RegEventControl : BaseUserControlExtend<EventModel>
    {
        public EventEntity EventEntity
        {
            get { return ViewState["Event"] as EventEntity; }
            set { ViewState["Event"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillFieldsGrid();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillFieldsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new PhaseModel();

            phaseControl.Model = model;
            mpePhase.Show();
        }

        protected void phasesControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = EventEntity;
            if (entity == null || entity.Phases == null)
                return;

            var phase = entity.Phases.FirstOrDefault(n => n.ID == e.Value);
            if (phase == null)
                return;

            var model = new PhaseModel
            {
                ID = phase.ID,
                Name = phase.Name,
                Description = phase.Description,
                StartDate = $"{phase.StartDate:dd.MM.yyyy}",
                StartTime = $"{phase.StartDate:HH:mm}",
                EndDate = $"{phase.EndDate:dd.MM.yyyy}",
                EndTime = $"{phase.EndDate:HH:mm}",
                FormID = phase.FormID,
                Color = phase.Color,
            };

            phaseControl.Model = model;
            mpePhase.Show();
        }

        protected void phasesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = EventEntity;
            if (entity == null || entity.Phases == null)
                return;

            var field = entity.Phases.FirstOrDefault(n => n.ID == e.Value);
            if (field == null)
                return;

            EventEntity.Phases.Remove(field);
        }

        protected void phaseControl_OnDataChanged(object sender, EventArgs e)
        {
            mpePhase.Show();
        }

        protected void btnPhaseOK_Click(object sender, EventArgs e)
        {
            var model = phaseControl.Model;

            var entity = EventEntity;
            if (entity == null)
            {
                entity = new EventEntity();
                EventEntity = entity;
            }

            if (entity.Phases == null)
                entity.Phases = new List<PhaseEntity>();

            if (model.ID == null)
            {
                var exists = (from n in entity.Phases
                              where n.Name == model.Name
                              select n).Any();

                if (exists)
                    return;
            }

            var phase = entity.Phases.FirstOrDefault(n => n.ID == model.ID);
            if (phase == null)
            {
                phase = new PhaseEntity
                {
                    ID = Guid.NewGuid(),
                };

                entity.Phases.Add(phase);
            }

            phase.Name = model.Name;
            phase.Color = model.Color;
            phase.FormID = model.FormID;

            var startDate = DataConverter.ToNullableDateTime(model.StartDate);
            startDate = startDate.GetValueOrDefault(DateTime.Now);

            var startTime = DataConverter.ToNullableDateTime(model.StartTime);
            startTime = startTime.GetValueOrDefault(DateTime.Now);

            var endDate = DataConverter.ToNullableDateTime(model.EndDate);
            endDate = endDate.GetValueOrDefault(DateTime.Now);

            var endTime = DataConverter.ToNullableDateTime(model.EndTime);
            endTime = endTime.GetValueOrDefault(DateTime.Now);

            phase.StartDate = GmsCommonUtil.Merge(startDate, startTime);
            phase.EndDate = GmsCommonUtil.Merge(endDate, endTime);

            mpePhase.Hide();
        }

        protected void btPhaseCancel_OnClick(object sender, EventArgs e)
        {
            mpePhase.Hide();
        }

        public override EventModel GetModel()
        {
            var model = base.GetModel();
            model.Entity = EventEntity;

            if (model.Entity != null)
                model.Entity.Name = model.Name;

            return model;
        }

        public override void SetModel(EventModel model)
        {
            EventEntity = model.Entity;
            base.SetModel(model);
        }

        protected void FillFieldsGrid()
        {
            var entity = EventEntity;
            if (entity == null || entity.Phases == null)
                return;

            var units = new PhaseUnitsModel
            {
                List = entity.Phases
            };

            phasesControl.Model = units;
            phasesControl.DataBind();
        }
    }
}