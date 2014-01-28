using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Mapping;

namespace Common.DA
{
    /// <summary>
    /// Класс предназначенный для отображения свойств доменных объектов типа <c>Guid</c>
    /// </summary>
    public class GuidMapper : MemberMapper
    {
        Guid _nullValue;

        public override bool IsNull(object o) { return false; }
        public override void SetNull(object o) { MemberAccessor.SetGuid(o, _nullValue); }

        public override object GetValue(object o)
        {
            Guid val = (Guid)base.GetValue(o);
            if (val == Guid.Empty)
                return null;
            else
                return val.ToString();
        }

        public override void SetValue(object o, object value)
        {
            MemberAccessor.SetGuid(
                o,
                value is Guid ? (Guid)value :
                value == null ? _nullValue : MappingSchema.ConvertToGuid(value));
        }

        public override void Init(MapMemberInfo mapMemberInfo)
        {
            if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

            if (mapMemberInfo.NullValue != null)
                _nullValue = mapMemberInfo.NullValue is Guid ?
                    (Guid)mapMemberInfo.NullValue : new Guid(mapMemberInfo.NullValue.ToString());

            base.Init(mapMemberInfo);
        }

        public class Nullable : GuidMapper
        {
            public override bool IsNull(object o) { return GetGuid(o) == _nullValue; }

            public override object GetValue(object o)
            {
                var value = MemberAccessor.GetGuid(o);
                return value == _nullValue ? null : (object)value;
            }
        }
    }

    /// <summary>
    /// Класс предназначенный для отображения свойств доменных объектов типа <c>Guid?</c>
    /// </summary>
    public class NullableGuidMapper : MemberMapper
    {
        public override bool IsNull(object o) { return GetNullableGuid(o) == null; }
        public override void SetNull(object o) { MemberAccessor.SetNullableGuid(o, null); }

        public override object GetValue(object o)
        {
            object val = base.GetValue(o);
            return val == null ? null : val.ToString();
        }

        public override void SetValue(object o, object value)
        {
            MemberAccessor.SetNullableGuid(
                o, value == null || value is Guid ? (Guid?)value : MappingSchema.ConvertToNullableGuid(value));
        }
    }
}
