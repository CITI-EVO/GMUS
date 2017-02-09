using System;
using System.Dynamic;

namespace CITI.EVO.Tools.Helpers
{
	public class AnonymousObject : DynamicObject
	{
		private readonly Object _object;
		private readonly Type _type;

		public AnonymousObject(Object @object)
		{
			if (@object == null)
			{
				throw new ArgumentNullException("@object");
			}

			_object = @object;
			_type = @object.GetType();
		}

		public override bool TryGetMember(GetMemberBinder binder, out Object result)
		{
			result = null;

			var property = _type.GetProperty(binder.Name);
			if (property == null)
			{
				var field = _type.GetField(binder.Name);
				if (field == null)
				{
					return false;
				}

				result = field.GetValue(_object);
				return true;
			}

			result = property.GetValue(_object);
			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, Object value)
		{
			var property = _type.GetProperty(binder.Name);
			if (property == null)
			{
				var field = _type.GetField(binder.Name);
				if (field == null)
				{
					return false;
				}

				field.SetValue(_object, value);
				return true;
			}

			property.SetValue(_object, value);
			return true;
		}
	}

}
