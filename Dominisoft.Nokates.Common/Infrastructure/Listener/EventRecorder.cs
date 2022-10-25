using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Infrastructure.Repositories;
using Dominisoft.Nokates.Common.Models;

namespace Dominisoft.Nokates.Common.Infrastructure.Listener
{
    public static class EventRecorder
    {
        private static ISqlRepository<LogEntry> _logRepo;
        private static ISqlRepository<RequestMetric> _requestRepo;
        private static ISqlRepository<RepositoryMetric> _repositoryMetricRepo;

        public static void Start()
        {

                
            _logRepo = RepositoryHelper.CreateRepository<LogEntry>();
            _requestRepo = RepositoryHelper.CreateRepository<RequestMetric>();
            _repositoryMetricRepo = RepositoryHelper.CreateRepository<RepositoryMetric>();

            StatusValues.EventLog.CollectionChanged += EventLog_CollectionChanged;
            StatusValues.RequestMetrics.CollectionChanged += RequestMetrics_CollectionChanged;
            StatusValues.RepositoryMetrics.CollectionChanged += RepositoryMetrics_CollectionChanged;


            StatusValues.EventLog.ToList().ForEach(msg =>
            {
                try
                {
                    _logRepo.Create(msg);
                }
                catch (Exception)
                {
                    // ignored
                }
            });
        }

        private static void RepositoryMetrics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newMessages = e.NewItems.Serialize().Deserialize<List<RepositoryMetric>>();
            newMessages.ForEach(msg =>
            {

                _repositoryMetricRepo.Create(msg);

            });
        }

        private static void RequestMetrics_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newMessages = e.NewItems.Serialize().Deserialize<List<RequestMetric>>();
            newMessages.ForEach(msg =>
            {
             
                    _requestRepo.Create(msg);
                
            });
        }

        private static void EventLog_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newMessages =e.NewItems.Serialize().Deserialize<List<LogEntry>>();
            newMessages.ForEach(msg =>
            {
             
                    _logRepo.Create(msg);
               
            });

        }
    }
}
