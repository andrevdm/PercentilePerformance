using System;

namespace PercentilePerformance
{
    public class PercentileSample
    {
        public PercentileSample( PercentileCalculator percentileCalculator, PercentileBucket[] stats, DateTime? firstValueAt, DateTime? lastValueAt )
        {
            PercentileCalculator = percentileCalculator;
            Stats = stats;
            FirstValueAt = firstValueAt;
            LastValueAt = lastValueAt;
        }

        public PercentileCalculator PercentileCalculator { get; private set; }
        public PercentileBucket[] Stats { get; private set; }
        public DateTime? FirstValueAt { get; private set; }
        public DateTime? LastValueAt { get; private set; }

        public PercentileBucket FindBucketForPercentile( int value )
        {
            return PercentileCalculator.FindBucketForPercentile( value, Stats );
        }

        public int FindBoundForPercentile( int value )
        {
            return PercentileCalculator.FindBoundForPercentile( value, Stats );
        }
    }
}