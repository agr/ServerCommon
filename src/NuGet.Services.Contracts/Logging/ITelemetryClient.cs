﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NuGet.Services.Logging
{
    public interface ITelemetryClient
    {
        void TrackTrace(string message, LogLevel logLevel, EventId eventId);

        void TrackMetric(
            string metricName,
            double value,
            IDictionary<string, string> properties = null);

        void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);
    }
}