﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace NuGet.Services.Logging
{
    public class TelemetryClientWrapper : ITelemetryClient
    {
        private const string TelemetryPropertyEventId = "EventId";
        private const string TelemetryPropertyEventName = "EventName";
        private readonly TelemetryClient _telemetryClient;

        public TelemetryClientWrapper(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            try
            {
                _telemetryClient.TrackException(exception, properties, metrics);
            }
            catch
            {
                // logging failed, don't allow exception to escape
            }
        }

        public void TrackMetric(
            string metricName,
            double value,
            IDictionary<string, string> properties = null)
        {
            try
            {
                _telemetryClient.TrackMetric(metricName, value, properties);
            }
            catch
            {
                // logging failed, don't allow exception to escape
            }
        }

        public void TrackTrace(string message, LogLevel logLevel, EventId eventId)
        {
            try
            {
                var telemetry = new TraceTelemetry(
                    message,
                    LogLevelToSeverityLevel(logLevel));

                telemetry.Properties[TelemetryPropertyEventId] = eventId.Id.ToString();

                if (!string.IsNullOrWhiteSpace(eventId.Name))
                {
                    telemetry.Properties[TelemetryPropertyEventName] = eventId.Name;
                }

                _telemetryClient.TrackTrace(telemetry);
            }
            catch
            {
                // logging failed, don't allow exception to escape
            }
        }

        private static SeverityLevel LogLevelToSeverityLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical: return SeverityLevel.Critical;
                case LogLevel.Error: return SeverityLevel.Error;
                case LogLevel.Warning: return SeverityLevel.Warning;
                case LogLevel.Information: return SeverityLevel.Information;
                case LogLevel.Trace:
                default: return SeverityLevel.Verbose;
            }
        }
    }
}