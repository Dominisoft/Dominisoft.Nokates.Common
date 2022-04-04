using System;
using System.Collections.Generic;
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
        private static SqlRepository<LogEntry> _logRepo;
        private static SqlRepository<RequestMetric> _requestRepo;

        public static void Start()
        {

                
            _logRepo = RepositoryHelper.CreateRepository<LogEntry>();
            _requestRepo = RepositoryHelper.CreateRepository<RequestMetric>();

            StatusValues.EventLog.CollectionChanged += EventLog_CollectionChanged;
            StatusValues.RequestMetrics.CollectionChanged += RequestMetrics_CollectionChanged;

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

        private static void RequestMetrics_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newMessages = e.NewItems.Serialize().Deserialize<List<RequestMetric>>();
            newMessages.ForEach(msg =>
            {
             
                    _requestRepo.Create(msg);
                
            });
        }

        private static void EventLog_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newMessages =e.NewItems.Serialize().Deserialize<List<LogEntry>>();
            newMessages.ForEach(msg =>
            {
             
                    _logRepo.Create(msg);
               
            });

        }
    }
}
