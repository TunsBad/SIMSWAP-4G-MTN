using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace sim_swap.Extensions
{
    public static class DbExtensions
    {
       
            public static NpgsqlCommand WithSqlParam(this NpgsqlCommand cmd, string paramName, object paramValue, NpgsqlDbType dbType = NpgsqlDbType.Text)
            {
                if (string.IsNullOrEmpty(cmd.CommandText))
                    throw new InvalidOperationException("Call LoadStoredProc before using this method");

                var param = cmd.CreateParameter();
                param.ParameterName = paramName;
                param.Value = paramValue;
                param.NpgsqlDbType = dbType;
                cmd.Parameters.Add(param);
                return cmd;
            }

            private static IList<T> MapToList<T>(this NpgsqlDataReader dr)
            {
                var objList = new List<T>();
                var props = typeof(T).GetRuntimeProperties();


                var colMapping = dr.GetColumnSchema()
                    .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                    .ToDictionary(key => key.ColumnName.ToLower());

                if (dr.HasRows)
                {

                    if (colMapping.Any())
                    {
                        while (dr.Read())
                        {
                            T obj = Activator.CreateInstance<T>();
                            foreach (var prop in props)
                            {
                                var propKey = prop.Name.ToLower();
                                if (colMapping.Keys.Any(c => c == propKey))
                                {
                                    var columnOrdinal = colMapping[prop.Name.ToLower()].ColumnOrdinal;
                                    if (columnOrdinal != null)
                                    {
                                        var val = dr.GetValue(columnOrdinal.Value);
                                        prop.SetValue(obj, val == DBNull.Value ? null : val);
                                    }
                                }
                            }
                            objList.Add(obj);
                        }
                    }
                }
                return objList;
            }

            public static IList<T> ExecuteStoredProc<T>(this NpgsqlCommand command)
            {
                using (command)
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            return reader.MapToList<T>();
                        }
                    }

                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }

            public static void ExecuteNonReturning(this NpgsqlCommand command)
            {
                using (command)
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    try
                    {
                        var data = command.ExecuteNonQuery();
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }

            public static async Task<T> ExecuteReturningScalarAsync<T>(this NpgsqlCommand command) where T : struct
            {
                using (command)
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    try
                    {
                        var data = (T)(await command.ExecuteScalarAsync());
                        return data;
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }

            public static async Task<IList<T>> ExecuteStoredProcAsync<T>(this NpgsqlCommand command)
            {
                return await Task.FromResult(ExecuteStoredProc<T>(command));
            }
        }
    
}
