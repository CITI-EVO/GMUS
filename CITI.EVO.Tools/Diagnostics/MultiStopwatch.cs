using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CITI.EVO.Tools.Diagnostics
{
    public class MultiStopwatch
    {
        private readonly IDictionary<String, long> occurrence = new Dictionary<String, long>();
        private readonly IDictionary<String, Stopwatch> stopwatches = new Dictionary<String, Stopwatch>();

        public IDictionary<String, long> Elapseds
        {
            get
            {
                var dictionary = new SortedDictionary<String, long>();
                foreach (var pair in stopwatches)
                    dictionary.Add(pair.Key, pair.Value.ElapsedMilliseconds);

                return dictionary;
            }
        }

        public IDictionary<String, long> Occurrence
        {
            get
            {
                var dictionary = new SortedDictionary<String, long>();
                foreach (var pair in occurrence)
                    dictionary.Add(pair.Key, pair.Value);

                return dictionary;
            }
        }

        public TimeSpan SummaryElapsed
        {
            get
            {
                var result = new TimeSpan();

                foreach (var stopwatch in stopwatches)
                    result += stopwatch.Value.Elapsed;

                return result;
            }
        }

        public void StartOrStop(String key)
        {
            var stopwatch = InitWatch(key);
            if (stopwatch.IsRunning)
                stopwatch.Stop();
            else
            {
                occurrence[key]++;
                stopwatch.Start();
            }
        }

        public void Start(String key)
        {
            var stopwatch = InitWatch(key);
            occurrence[key]++;
            stopwatch.Start();
        }
        public void StartAll()
        {
            foreach (var pair in stopwatches)
            {
                occurrence[pair.Key]++;
                pair.Value.Start();
            }
        }

        public void Stop(String key)
        {
            var stopwatch = InitWatch(key);
            stopwatch.Stop();
        }

        public void StopAll()
        {
            foreach (var stopwatch in stopwatches)
                stopwatch.Value.Stop();
        }

        public void Restart(String key)
        {
            var stopwatch = InitWatch(key);
            stopwatch.Restart();
        }

        public void Reset(String key)
        {
            var stopwatch = InitWatch(key);
            occurrence[key] = 0L;
            stopwatch.Reset();
        }

        public void ResetAll()
        {
            foreach (var pair in stopwatches)
            {
                occurrence[pair.Key] = 0L;
                pair.Value.Reset();
            }
        }

        public bool IsRunning(String key)
        {
            var stopwatch = InitWatch(key);
            return stopwatch.IsRunning;
        }

        private Stopwatch InitWatch(String key)
        {
            Stopwatch stopwatch;
            if (!stopwatches.TryGetValue(key, out stopwatch))
            {
                stopwatch = new Stopwatch();
                stopwatches.Add(key, stopwatch);
                occurrence.Add(key, 0L);
            }

            return stopwatch;
        }
    }
}
