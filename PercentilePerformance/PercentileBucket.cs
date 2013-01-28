namespace PercentilePerformance

{
    public class PercentileBucket
    {
        public PercentileBucket( int pos, int bound, double percentile, int count )
        {
            Pos = pos;
            Bound = bound;
            Percentile = percentile;
            Count = count;
        }

        public int Pos { get; set; }
        public int Bound { get; set; }
        public int Count { get; set; }
        public double Percentile { get; set; }
    }
}