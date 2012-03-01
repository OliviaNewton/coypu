﻿using System;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Robustness
{
    internal interface RobustWrapper
    {
        T Robustly<T>(Query<T> query);
        void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan overallTimeout);
        bool ZeroTimeout { get; set; }
    }
}