using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace SqlHelper
{
    /// <summary>
    /// DynamicSQL MSSQL Database Helper By NJ.Wu(QQ:test)
    /// </summary>
    public class DynamicSQL : IDisposable
    {
        Dictionary<int, SqlParameter[]> m_parameterCache = new Dictionary<int, SqlParameter[]>();

        public string ConnectionString
        {
            get;
            set;
        }
        public DynamicSQL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DynamicSQL()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        public IEnumerable<dynamic> Exec(string sql)
        {
            return Exec(sql, ConnectionString);
        }
        public IEnumerable<dynamic> Exec(string sql, string connectionString)
        {
            return Exec(sql, null, connectionString);
        }
        public IEnumerable<dynamic> Exec(string sql, object parameters)
        {
            return Exec(sql, parameters, false, ConnectionString);
        }
        public IEnumerable<dynamic> Exec(string sql, object parameters, string connectionString)
        {
            return Exec(sql, parameters, false, connectionString);
        }


        public IEnumerable<dynamic> ExecProc(string procduceName)
        {
            return ExecProc(procduceName, ConnectionString);
        }

        public IEnumerable<dynamic> ExecProc(string procduceName, string connectionString)
        {
            return Exec(procduceName, null, true, connectionString);
        }

        public IEnumerable<dynamic> ExecProc(string procduceName, object parameters)
        {
            return ExecProc(procduceName, null, ConnectionString);
        }

        public IEnumerable<dynamic> ExecProc(string procduceName, object parameters, string connectionString)
        {
            return Exec(procduceName, parameters, true, connectionString);
        }


        public IEnumerable<dynamic> Exec(string sql, object parameters, bool proc, string connectionString)
        {
            SqlParameter[] sqlParameters = null;

            if (parameters != null)
            {
                int parametersHashCode = parameters.GetHashCode();
                lock (m_parameterCache)
                {
                    if (m_parameterCache.ContainsKey(parametersHashCode))
                    {
                        sqlParameters = m_parameterCache[parametersHashCode];
                    }
                    else
                    {
                        sqlParameters = ObjectToSqlParameterList(parameters).ToArray();
                        m_parameterCache.Add(parametersHashCode, sqlParameters);
                    }
                }
            }
            Result = new ResultSet(connectionString, sql, sqlParameters, proc);
            return Result;
        }
        private ResultSet Result
        {
            get;
            set;
        }



        private static List<SqlParameter> ObjectToSqlParameterList(object obj)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                object value = property.GetValue(obj, null);
                parameters.Add(new SqlParameter(name, value));
            }
            return parameters;
        }

        public void Dispose()
        {
            if (Result != null)
            {
                Result.Dispose();
            }
        }
    }
}
