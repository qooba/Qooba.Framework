using Qooba.Framework.Configuration.Abstractions;
using Dapper;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qooba.Framework.Db.Abstractions;

namespace Qooba.Framework.Db
{
    public class Db : IDb
    {
        private readonly IConfig config;

        private string connectionString;

        private Lazy<IDbConnection> connection;

        public Db(IConfig config)
        {
            this.config = config;
            this.connectionString = this.config.ConnectionString;
            this.connection = new Lazy<IDbConnection>(() => new System.Data.SqlClient.SqlConnection(this.connectionString));
        }

        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void SetConnection(IDbConnection connection)
        {
            this.connection = new Lazy<IDbConnection>(() => connection);
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public IDataReader ExecuteReader(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public object ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteScalarAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<object> Query(Type type, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(type, sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.Query(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<object>> QueryAsync(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public object QueryFirst(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirst(type, sql, param, transaction, commandTimeout, commandType);
        }

        public T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstAsync(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public object QueryFirstOrDefault(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QueryFirstOrDefaultAsync(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public object QuerySingle(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingle(type, sql, param, transaction, commandTimeout, commandType);
        }

        public T QuerySingle<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingle<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QuerySingleAsync(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public object QuerySingleOrDefault(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleOrDefault(type, sql, param, transaction, commandTimeout, commandType);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleOrDefaultAsync(type, sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return this.connection.Value.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.connection?.Value != null)
                {
                    this.connection.Value.Dispose();
                }
            }
        }
    }
}
