﻿using System;
using System.Diagnostics;
using PodcastUtilities.Common.Platform;

namespace PodcastUtilities.Common.Perfmon
{
    /// <summary>
    /// counter to measure the average time for an event
    /// </summary>
    public class AverageCounter : IAverageCounter
    {
        private readonly IPerfmonCounter _averageTime;
        private readonly IPerfmonCounter _averageTimeBase;
        private readonly IPerfmonCounter _totalCounter;

        /// <summary>
        /// make an average time counter
        /// </summary>
        public AverageCounter(string counterCategory, string averageCounterName) : this(counterCategory,averageCounterName,null)
        {
        }

        /// <summary>
        /// make an average time counter and a total number of events counter
        /// </summary>
        public AverageCounter(string counterCategory, string averageCounterName, string totalCounterName)
        {
            _averageTime = new SystemPerfmonCounter(counterCategory, averageCounterName);
            _averageTimeBase = new SystemPerfmonCounter(counterCategory, averageCounterName + "Base");
            if (!string.IsNullOrEmpty(totalCounterName))
            {
                _totalCounter = new SystemPerfmonCounter(counterCategory, totalCounterName);
            }
        }

        /// <summary>
        /// reset the counter
        /// </summary>
        public void Reset()
        {
            if (_averageTime != null && _averageTimeBase != null)
            {
                _averageTime.Reset();
                _averageTimeBase.Reset();
            }
            if (_totalCounter != null)
            {
                _totalCounter.Reset();
            }
        }

        /// <summary>
        /// register the time for a single event and increments the total counter
        /// </summary>
        public void RegisterTime(Stopwatch timer)
        {
            if (_averageTime != null && _averageTimeBase != null)
            {
                _averageTime.IncrementBy(timer.ElapsedMilliseconds);
                _averageTimeBase.Increment();
            }
            if (_totalCounter != null)
            {
                _totalCounter.Increment();
            }
        }

        /// <summary>
        /// register the value to be recorded against a single event and then increments the total counter by the value as well
        /// </summary>
        public void RegisterValue(long value)
        {
            if (_averageTime != null && _averageTimeBase != null)
            {
                _averageTime.IncrementBy(value);
                _averageTimeBase.Increment();
            }
            if (_totalCounter != null)
            {
                _totalCounter.IncrementBy(value);
            }
        }
    }
}
