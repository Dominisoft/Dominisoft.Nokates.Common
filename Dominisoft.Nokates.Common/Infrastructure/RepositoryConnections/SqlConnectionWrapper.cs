using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Infrastructure.Repositories;
using Dominisoft.Nokates.Common.Models;
using Dominisoft.SqlBuilder;

namespace Dominisoft.Nokates.Common.Infrastructure.RepositoryConnections
{
    public interface ISqlConnectionWrapper
    {
        int Insert<TEntity>(TEntity entity) where TEntity : Entity, new();
        TEntity Get<TEntity>(TEntity entity) where TEntity : Entity, new();
        List<TEntity> GetAll<TEntity>() where TEntity : Entity, new();
        List<TEntity> GetAllFilter<TEntity>(object filter) where TEntity : Entity, new();
        List<TEntity> GetAnyFilter<TEntity>(object filter) where TEntity : Entity, new();
        int Update<TEntity>(TEntity entity) where TEntity : Entity, new();
        int Delete<TEntity>(TEntity entity) where TEntity : Entity, new();
    }
    public class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private readonly SqlConnection _connection;
        private readonly bool _logMetrics;
        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _logMetrics = GetLogRepository();
        }

        private string GetAllCacheTag<TEntity>() where TEntity:Entity,new()
        {
            var entityTable = new TEntity().GetTableName();
            return $"GetAll - {entityTable}";

        }
        public List<TEntity> Query<TEntity>(string procedure, object param)
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = _connection.Query<TEntity>(procedure, param, commandType: CommandType.StoredProcedure)?.ToList() ?? new List<TEntity>();
            watch.Stop();
            LogTransactionResult(_connection, start, procedure, param, result, watch.ElapsedMilliseconds);
            return result;
        }


        public TEntity QueryFirstOrDefault<TEntity>(string procedure, object param)
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result =
                _connection.QueryFirstOrDefault<TEntity>(procedure, param, commandType: CommandType.StoredProcedure);
            watch.Stop();
            LogTransactionResult(_connection, start, procedure, param, result, watch.ElapsedMilliseconds);
            return result;
        }

        public void Execute(string procedure, object param)
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            _connection.Execute(procedure, param, commandType: CommandType.StoredProcedure);
            watch.Stop();
            LogTransactionResult(_connection, start, procedure, param, null, watch.ElapsedMilliseconds);
        }

        public int Insert<TEntity>(TEntity entity) where TEntity : Entity, new()
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var entityTable = entity.GetTableName();
            var result = _connection.Insert(entity);
            watch.Stop();
            UpdateCache<TEntity>();
            LogTransactionResult(_connection, start, $"Insert - {entityTable}", entity, result, watch.ElapsedMilliseconds);
            return result;

        }

        public TEntity Get<TEntity>(TEntity entity) where TEntity : Entity, new()
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var entityTable = entity.GetTableName();
            var result = _connection.Get(entity);
            watch.Stop();
            LogTransactionResult(_connection, start, $"Get - {entityTable}", entity, result, watch.ElapsedMilliseconds);
            return result;
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : Entity, new()
        {
            var query = GetAllCacheTag<TEntity>();
            var disableCache = ConfigurationValues.GetBoolValueOrDefault("DisableRepositoryCache");


            if (Cache.HasValidValue(query) && !disableCache)
                return Cache.GetValue<List<TEntity>>(query);

            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = _connection.GetAll<TEntity>();
            watch.Stop();
            LogTransactionResult(_connection, start, query, null, result, watch.ElapsedMilliseconds);

            var hasTime = ConfigurationValues.TryGetValue<int>(out var cacheTime, "CacheTime");
            if (!hasTime) cacheTime = 60;
            if (!disableCache)
                Cache.SetValue(query, result, cacheTime);
            return result;
        }

        public List<TEntity> GetAllFilter<TEntity>(object filter) where TEntity : Entity, new()
        {

            if (Cache.HasValidValue(GetAllCacheTag<TEntity>()))
                return GetAllFilterFromCache<TEntity>(filter);
            var entity = new TEntity();
            var entityTable = entity.GetTableName();
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = _connection.GetAllFilterAdHoc(entity,filter);
            watch.Stop();
            LogTransactionResult(_connection, start, $"{nameof(GetAllFilter)} - {entityTable}", null, result, watch.ElapsedMilliseconds);
            return result;
        }

        private List<TEntity> GetAllFilterFromCache<TEntity>(object filter) where TEntity : Entity, new()
        {
            var query = GetAllCacheTag<TEntity>();
            LoggingHelper.LogDebug($"Reading From Cache - {query}");
            var all = Cache.GetValue<List<TEntity>>(query);
            var filters = filter.Serialize().Deserialize<Dictionary<string, string>>();
            var t = typeof(TEntity);
            var matches = all.Where(e => filters.All(f => f.Value.Serialize() == t.GetProperty(f.Key)?.GetValue(e)?.Serialize()))
                .ToList();
            return matches;

        }

        public List<TEntity> GetAnyFilter<TEntity>(object filter) where TEntity : Entity, new()
        {
            var entity = new TEntity();
            var entityTable = entity.GetTableName();
            if (Cache.HasValidValue(GetAllCacheTag<TEntity>()))
                return GetAnyFilterFromCache<TEntity>(filter);

            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = _connection.GetAnyFilterAdHoc(entity, filter);
            watch.Stop();
            LogTransactionResult(_connection, start, $"{nameof(GetAnyFilter)} - {entityTable}", null, result, watch.ElapsedMilliseconds);
            return result;
        }

        private List<TEntity> GetAnyFilterFromCache<TEntity>(object filter) where TEntity : Entity, new()
        {
            var query = GetAllCacheTag<TEntity>();
            var all = Cache.GetValue<List<TEntity>>(query);
            var filters = filter.Serialize().Deserialize<Dictionary<string, string>>();
            var t = typeof(TEntity);
            var matches = all.Where(e => filters.Any(f => f.Value.Serialize() == t.GetProperty(f.Key)?.GetValue(e)?.Serialize()))
                .ToList();

            return matches;
        }


        public int Update<TEntity>(TEntity entity) where TEntity : Entity, new()
        {

            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var entityTable = entity.GetTableName();
            var result = _connection.Update(entity);
            watch.Stop();
            LogTransactionResult(_connection, start, $"Update - {entityTable}", entity, result, watch.ElapsedMilliseconds);
            UpdateCache<TEntity>();
            return result;
        }

      
        public int Delete<TEntity>(TEntity entity) where TEntity : Entity, new()
        {
            var start = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var entityTable = entity.GetTableName();
            var result = _connection.Delete(entity);
            watch.Stop();
            UpdateCache<TEntity>();
            LogTransactionResult(_connection, start, $"Delete - {entityTable}", entity, result, watch.ElapsedMilliseconds);
            return result;
        }
        private void UpdateCache<TEntity>() where TEntity : Entity, new()
        {
            var cacheTag = GetAllCacheTag<TEntity>();
            Cache.Invalidate(cacheTag);
        }

        private void LogTransactionResult(SqlConnection connection,DateTime start,string query,object param, object result, long elapsedMilliseconds)
        {
            if (param?.GetType() == typeof(RepositoryMetric)) return;
            if (!_logMetrics) return;
            var trackingId = System.Threading.Thread.CurrentThread.GetRequestId();
            var metric = new RepositoryMetric
            {
                Query = query,
                Request = param?.Serialize()??string.Empty,
                RequestStart = start,
                RequestTrackingId = trackingId,
                Response = result?.Serialize()??string.Empty,
                ResponseTime = elapsedMilliseconds,
                ServiceName = AppHelper.GetAppName()


            };
            StatusValues.LogRepositoryMetric(metric);
        }

        private bool GetLogRepository()
        {
            var LogDatabaseMetrics = ConfigurationValues.GetBoolValueOrDefault("LogDatabaseMetrics");
            return LogDatabaseMetrics;
        }
    }
}
