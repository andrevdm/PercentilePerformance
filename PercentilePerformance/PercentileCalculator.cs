using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PercentilePerformance
{
    public class PercentileCalculator
    {
        private readonly int m_samplesToStore;
        private readonly int[] m_bounds;
        private PercentileBucket[] m_buckets;
        private List<PercentileSample> m_storedStats;

        public DateTime? FirstValueAt { get; private set; }
        public DateTime? LastValueAt { get; private set; }

        public IEnumerable<PercentileSample> StoredStats
        {
            get { return m_storedStats; }
        }

        public PercentileCalculator()
            : this( 0 )
        {
        }

        public PercentileCalculator( int samplesToStore )
            : this( samplesToStore, new[] { 0, 1, 2, 4, 8, 16, 25, 40, 60, 100, 150, 210, 300, 400, 500, 600, 800, 1000, 1500, 2000 } )
        {
        }

        public PercentileCalculator( IEnumerable<int> bucketUpperBounds )
            : this( 0, bucketUpperBounds )
        {
        }

        public PercentileCalculator( int samplesToStore, IEnumerable<int> bucketUpperBounds )
        {
            m_samplesToStore = samplesToStore;
            m_bounds = bucketUpperBounds.ToArray();

            m_buckets = (from i in Enumerable.Range( 0, m_bounds.Length )
                         select new PercentileBucket( i, m_bounds[i], 0, 0 )).ToArray();
        }

        public void AddValue( int value )
        {
            if( FirstValueAt == null )
            {
                FirstValueAt = DateTime.Now;
            }

            LastValueAt = DateTime.Now;

            int index = FindBucketForValue( value );
            m_buckets[index].Count++;
        }

        public int FindBucketForValue( int value )
        {
            var pos = Array.BinarySearch( m_bounds, value );

            if( pos < 0 )
            {
                pos = (~pos) - 1;
            }

            return Math.Max( 0, pos );
        }

        public IEnumerable<PercentileBucket> CalculateStats()
        {
            var total = (from b in m_buckets select b.Count).Sum();

            for( int i = m_buckets.Length - 1; i >= 0; --i )
            {
                if( m_buckets[i].Count > 0 )
                {
                    var percentile = (m_buckets.Skip( i ).Sum( b => b.Count ) / (double)total) * 100;
                    m_buckets[i].Percentile = percentile;
                }
                else
                {
                    m_buckets[i].Percentile = -1;   
                }
            }

            return m_buckets;
        }

        public PercentileSample SampleAndReset()
        {
            var stats = CalculateStats();
            var sample = new PercentileSample( this, stats.ToArray(), FirstValueAt, LastValueAt );

            m_buckets = (from i in Enumerable.Range( 0, m_bounds.Length )
                         select new PercentileBucket( i, m_bounds[i], 0, 0 )).ToArray();

            FirstValueAt = null;
            LastValueAt = null;

            if( m_samplesToStore > 0 )
            {
                if( m_storedStats == null )
                {
                    m_storedStats = new List<PercentileSample>();
                }

                m_storedStats.Add( sample );

                while( m_storedStats.Count > m_samplesToStore )
                {
                    m_storedStats.RemoveAt( 0 );
                }
            }

            return sample;
        }

        public PercentileBucket FindBucketForPercentile( int value, PercentileBucket[] buckets )
        {
            return (from b in buckets where b.Percentile >= value orderby b.Percentile ascending select b).FirstOrDefault();
        }

        public int FindBoundForPercentile( int value, PercentileBucket[] buckets )
        {
            var bucket = FindBucketForPercentile( value, buckets );
            return bucket != null ? bucket.Bound : -1;
        }

        public PercentileBucket FindBucketForPercentile( int value )
        {
            return FindBucketForPercentile( value, m_buckets );
        }

        public int FindBoundForPercentile( int value )
        {
            return FindBoundForPercentile( value, m_buckets );
        }

        public IEnumerable<int> GetBounds()
        {
            return m_bounds;
        }

        private List<object[]> GetStatsArray()
        {
            var stats = CalculateStats();

            var items = new List<object[]>();
            items.Add( new object[] {"bound", "count", "percentile"} );

            foreach( var stat in stats )
            {
                items.Add( new object[] {stat.Bound, stat.Count, stat.Count > 0 ? stat.Percentile.ToString( "F2", CultureInfo.InvariantCulture ) : ""} );
            }
            return items;
        }
    }
}