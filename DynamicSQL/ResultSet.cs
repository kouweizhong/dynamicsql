using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace SqlHelper
{
    internal class ResultSet : IDisposable, IEnumerator<object>, IEnumerable<object>
    {
        SqlDataReader m_reader;
        SqlCommand m_command;
        SqlConnection m_connection;
        public ResultSet(string connectionString, string sql) : this(connectionString, sql, null, false) { }
        internal ResultSet(string connectionString, string sql, SqlParameter[] parameters, bool proc)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connectionstring can not be empty.");
            }
            m_connection = new SqlConnection(connectionString);
            m_connection.Open();
            m_command = new SqlCommand(sql, m_connection);
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    m_command.Parameters.Add(new SqlParameter(parameter.ParameterName, parameter.Value));
                }
            }
            if (proc)
            {
                m_command.CommandType = CommandType.StoredProcedure;
            }
            m_reader = m_command.ExecuteReader();
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public dynamic Current
        {
            get;
            private set;
        }

        public bool MoveNext()
        {
            if (m_reader.Read())
            {
                dynamic results = new ExpandoObject();

                IDictionary<string, object> resultsDictionary = (IDictionary<string, object>)results;

                for (int i = 0; i < m_reader.FieldCount; i++)
                {
                    string key = m_reader.GetName(i);
                    object value = m_reader.GetValue(i);

                    if (resultsDictionary.ContainsKey(key))
                    {
                        resultsDictionary[key] = value;
                    }
                    else
                    {
                        resultsDictionary.Add(key, value);
                    }
                }
                Current = results;

                return true;
            }
            return false;
        }
        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public IEnumerator<dynamic> GetEnumerator()
        {
            return this;
        }
        public void Dispose()
        {
            m_command.Cancel();
            m_reader.Dispose();
            m_command.Dispose();
            m_connection.Dispose();
        }
    }
}
