using System;
using System.Collections.Generic;
using System.Threading;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class ThreadExtensions
    {
        private static Dictionary<int,Guid> _threadIds = new Dictionary<int,Guid>();
        public static void SetRequestId(this Thread t, Guid id)
        {
            var threadId = t.ManagedThreadId;
            _threadIds[threadId] = id;
        }
        public static Guid GetRequestId(this Thread t)
        {
            var threadId = t.ManagedThreadId;
            return _threadIds.ContainsKey(threadId) ? 
                _threadIds[threadId] : Guid.Empty;
        }
    }
}
