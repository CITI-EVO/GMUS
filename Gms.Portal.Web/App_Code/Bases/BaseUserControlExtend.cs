using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Bases
{
    public class BaseUserControlExtend<TModel> : BaseUserControl<TModel> where TModel : class, new()
    {
        public BaseUserControlExtend()
        {
        }

        public event EventHandler<GenericEventArgs<Guid>> New;
        protected virtual void OnNew(GenericEventArgs<Guid> e)
        {
            if (New != null)
                New(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> View;
        protected virtual void OnView(GenericEventArgs<Guid> e)
        {
            if (View != null)
                View(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Edit;
        protected virtual void OnEdit(GenericEventArgs<Guid> e)
        {
            if (Edit != null)
                Edit(this, e);
        }

        public event EventHandler<GenericEventArgs<Guid>> Delete;
        protected virtual void OnDelete(GenericEventArgs<Guid> e)
        {
            if (Delete != null)
                Delete(this, e);
        }

        protected virtual void btnNew_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnNew(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected virtual void btnView_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnView(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected virtual void btnEdit_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnEdit(new GenericEventArgs<Guid>(itemID.Value));
        }

        protected virtual void btnDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID != null)
                OnDelete(new GenericEventArgs<Guid>(itemID.Value));
        }
    }
}