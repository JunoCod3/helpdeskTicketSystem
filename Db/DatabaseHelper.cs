using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FullstackDevTS.Db;

public static class DatabaseHelper
{
    private static SqlParameter CreateSqlParameter(string name, object value)
    {
        return value switch
        {
            Guid guid => new SqlParameter(name, SqlDbType.UniqueIdentifier) { Value = guid },
            int i => new SqlParameter(name, SqlDbType.Int) { Value = i },
            string s => new SqlParameter(name, SqlDbType.NVarChar) { Value = s },
            bool b => new SqlParameter(name, SqlDbType.Bit) { Value = b },
            DateTime dt => new SqlParameter(name, SqlDbType.DateTime) { Value = dt },
            null => new SqlParameter(name, SqlDbType.NVarChar) { Value = DBNull.Value},
            _ => new SqlParameter(name,value)
        };
    }

    public static async Task<List<T>> ExecuteQuery<T>(
        this DbContext context,
        string sql,
        Func<SqlDataReader, T> createObject,
        Dictionary<string, object>? parameters = null,
        bool isProcedure = false)
    {
        
        var result = new List<T>();
        var connection = context.Database.GetDbConnection() as SqlConnection;

        await using (connection)
        {
            await connection?.OpenAsync()!;

            await using var command = new SqlCommand(sql,connection);

            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(CreateSqlParameter(parameter.Key, parameter.Value)); 
                }
            }

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(createObject(reader));
            }
        }
        
        return result; 

    }

    public static async Task ExecuteNonQuery<T>(
        this DbContext context,
        string sql,
        Func<SqlDataReader, T> createObject,
        Dictionary<string, object>? parameters = null,
        bool isProcedure = false)
    {
        
        var connection = context.Database.GetDbConnection() as SqlConnection;

        await using (connection)
        {
            await connection?.OpenAsync()!;

            await using var command = new SqlCommand(sql, connection);

            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(CreateSqlParameter(parameter.Key, parameter.Value));
                }
            }

            await command.ExecuteNonQueryAsync();
        }
    }
    
    //Scalar : Return first column result
    public static async Task<object?> ExecuteScalar<T>(
        this DbContext context,
        string sql,
        Func<SqlDataReader, T> createObject,
        Dictionary<string, object>? parameters = null,
        bool isProcedure = false)
    {
        
        var connection = context.Database.GetDbConnection() as SqlConnection;

        await using (connection)
        {
            await connection?.OpenAsync()!;

            await using var command = new SqlCommand(sql, connection);

            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(CreateSqlParameter(parameter.Key, parameter.Value));
                }
            }

            return await command.ExecuteScalarAsync();
        }
    }
    
    
    //Scalar : Return first column result
    public static async Task<Dictionary<string,object?>?> CallProcedure(
        this DbContext context,
        string procedureName,
        Dictionary<string, (object? value,ParameterDirection direction )> parameters)
    {
        
        var connection = context.Database.GetDbConnection() as SqlConnection;
        
        var outputValues = new Dictionary<string, object?>();
        
        await using (connection)
        {
            await connection?.OpenAsync()!;

            await using var command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            if (parameters.Any())
            {
                foreach (var parameter in parameters)
                {
                    var sqlParameter = CreateSqlParameter(parameter.Key, parameter.Value);
                    sqlParameter.Direction = parameter.Value.direction;
                    command.Parameters.Add(sqlParameter);
                }
            }

            await command.ExecuteNonQueryAsync();

            foreach (SqlParameter param in command.Parameters)
            {
                if (param.Direction == ParameterDirection.Input ||
                    param.Direction == ParameterDirection.Output ||
                    param.Direction == ParameterDirection.ReturnValue)
                {
                    outputValues.Add(param.ParameterName, param.Value);
                }
            }
        }
        
        return outputValues;
    }
}