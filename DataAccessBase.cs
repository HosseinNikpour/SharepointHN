using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointHN
{
    public class DataAccessBase
{
    // Fields
    private SqlCommand Cmd;
    private SqlConnection Cn;
    private string Cs;

    // Methods
    public DataAccessBase()
    {
       // this.Cs = "Data Source=172.29.0.161;Initial Catalog=wss_content;user id=sa;password=P@ssw0rd";
        this.Cs = "Data Source=172.16.33.252;Initial Catalog=wss_content;user id=sa;password=P@ssw0rd";
        this.Cn = null;
        this.Cmd = null;
    }

    public DataAccessBase(string ConnectionString)
    {
       // this.Cs = "Data Source=172.29.0.161;Initial Catalog=wss_content;user id=sa;password=P@ssw0rd";
        this.Cs = "Data Source=172.16.33.252;Initial Catalog=wss_content;user id=sa;password=P@ssw0rd";
        this.Cn = null;
        this.Cmd = null;
        this.Cs = ConnectionString;
    }

    private int GetStoredProcidureParameters(string storedProsedureName, ref SqlCommand comm)
    {
        int count;
        DataTable dataTable = new DataTable();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlParameter parameter = null;
        try
        {
            command.Connection = new SqlConnection(this.Cs);
            command.CommandText = "    SELECT    SCHEMA_NAME(o.schema_id) AS schema_name, o.name AS object_name, o.type_desc, p.parameter_id, p.name AS parameter_name, TYPE_NAME(p.user_type_id) \r\n                                AS parameter_type, p.max_length, p.precision, p.scale, p.is_output\r\n                                FROM    sys.objects AS o INNER JOIN\r\n                                sys.parameters AS p ON o.object_id = p.object_id\r\n                                WHERE (o.name LIKE '" + storedProsedureName + "')\r\n                                ORDER BY  schema_name, object_name, p.parameter_id";
            if (command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = row["parameter_name"].ToString();
                parameter.Size = int.Parse(row["max_length"].ToString());
                comm.Parameters.Add(parameter);
            }
            count = comm.Parameters.Count;
        }
        catch (Exception)
        {
            throw;
        }
        return count;
    }

    public int NonQuerySp(string SPName)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteNonQuery();
    }

    public int NonQuerySp(string SPName, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(SPName, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
            return this.Cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
        }
        return 0;
    }

    public int NonQueryTxt(string CommandText)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteNonQuery();
    }

    public int NonQueryTxt(string CommandText, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(CommandText, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteNonQuery();
    }

    public DataTable ReaderSp(string SPName)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        SqlDataReader reader = this.Cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);
        reader.Close();
        this.Cn.Close();
        return table;
    }

    public DataTable ReaderSp(string SPName, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(SPName, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        SqlDataReader reader = this.Cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);
        reader.Close();
        this.Cn.Close();
        return table;
    }

    public DataTable ReaderText(string CommandText)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        SqlDataReader reader = this.Cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);
        reader.Close();
        this.Cn.Close();
        return table;
    }

    public DataTable ReaderText(string CommandText, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(CommandText, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        SqlDataReader reader = this.Cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);
        reader.Close();
        this.Cn.Close();
        return table;
    }

    public object ScalarSp(string SPName)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteScalar();
    }

    public object ScalarSp(string SPName, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.StoredProcedure;
        this.Cmd.CommandText = SPName;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(SPName, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteScalar();
    }

    public object ScalarTxt(string CommandText)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteScalar();
    }

    public object ScalarTxt(string CommandText, params object[] ParameterValues)
    {
        this.Cn = new SqlConnection(this.Cs);
        this.Cmd = new SqlCommand();
        this.Cmd.CommandType = CommandType.Text;
        this.Cmd.CommandText = CommandText;
        this.Cmd.Connection = this.Cn;
        this.GetStoredProcidureParameters(CommandText, ref this.Cmd);
        for (int i = 0; i < ParameterValues.Length; i++)
        {
            this.Cmd.Parameters[i].Value = ParameterValues[i];
        }
        try
        {
            this.Cn.Open();
        }
        catch (Exception)
        {
        }
        return this.Cmd.ExecuteScalar();
    }
}

 

 

}
