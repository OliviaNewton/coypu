using System;
using Coypu.Actions;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class ImmediateSingleExecutionFakeRobustWrapper : RobustWrapper
    {
        public T Robustly<T>(Query<T> query)
        {
            query.Run();
            return query.Result;
        }

        public void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan overallTimeout)
        {
            tryThis.Act();
        }

        public bool ZeroTimeout{get; set; }
    }
}