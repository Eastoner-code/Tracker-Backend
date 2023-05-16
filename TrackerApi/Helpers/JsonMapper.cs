using System;
using System.Data.Common;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;
using NPoco;

namespace TrackerApi.Helpers
{
    public class JsonMapper : DefaultMapper
    {
        public override Func<object, object> GetToDbConverter(Type destType, MemberInfo sourceMemberInfo)
        {
            if (sourceMemberInfo.GetMemberInfoType() == typeof(string) && destType == typeof(JsonB))
                return x => new JsonB {Value = (string) x};

            return base.GetToDbConverter(destType, sourceMemberInfo);
        }

        public override Func<object, object> GetParameterConverter(DbCommand dbCommand, Type sourceType)
        {
            if (sourceType == typeof(JsonB))
                return x => new NpgsqlParameter
                {
                    NpgsqlDbType = NpgsqlDbType.Jsonb,
                    Value = ((JsonB) x).Value
                };

            return base.GetParameterConverter(dbCommand, sourceType);
        }
    }
}