using System;
using System.Linq;
using Avdm.PercentilePerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PercentilePerformanceTests
{
    [TestClass]
    public class PercentileCalculatorTests
    {
        [TestMethod]
        public void NumberOfBucketsMatchesCtor()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );
            calc.AddValue( 25 );

            var stats = calc.CalculateStats().ToArray();

            Assert.AreEqual( 3, stats.Count(), "Expecting three buckets" );
        }

        [TestMethod]
        public void SingleValueIsCountedInCorrectBucket()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );
            calc.AddValue( 25 );

            var stats = calc.CalculateStats().ToArray();

            Assert.AreEqual( 0, stats[0].Count, "Not expecting any values in the 1st bucket" );
            Assert.AreEqual( 1, stats[1].Count, "Expecting one value in the 2nd bucket" );
            Assert.AreEqual( 0, stats[2].Count, "Not expecting any values in the 3rd bucket" );
        }

        [TestMethod]
        public void NoValues_100PercentileIsMissing()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );

            calc.CalculateStats();

            var bound = calc.FindBoundForPercentile( 33 );

            Assert.AreEqual( -1, bound, "No value" );
        }

        [TestMethod]
        public void SingleValue_PercentileInCorrectBucket()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );
            calc.AddValue( 25 );

            calc.CalculateStats();
            var bound = calc.FindBoundForPercentile( 50 );

            Assert.AreEqual( 20, bound, "50th percentile is in 2nd bucket (20-50)" );
        }

        [TestMethod]
        public void SingleValue_PercentileInFirstBucket()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );
            calc.AddValue( 1 );

            calc.CalculateStats();
            var bound = calc.FindBoundForPercentile( 50 );

            Assert.AreEqual( 10, bound, "50th percentile is in 1st bucket (10-20)" );
        }

        [TestMethod]
        public void MultipleValues_PercentileInCorrectBucket()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 50 } );
            calc.AddValue( 11 );
            calc.AddValue( 12 );

            calc.AddValue( 51 );
            calc.AddValue( 52 );
            calc.AddValue( 53 );
            calc.AddValue( 54 );

            calc.CalculateStats();

            var bound = calc.FindBoundForPercentile( 50 );

            Assert.AreEqual( 50, bound, "50th percentile is in last bucket (50+)" );
        }

        [TestMethod]
        public void MultipleValues_AllPercentileInCorrectBucket()
        {
            var calc = new PercentileCalculator( new[] { 10, 20, 30, 40 } );

            foreach( var x in Enumerable.Range( 0, 5 ) )
            {
                calc.AddValue( 1 );
            }

            foreach( var x in Enumerable.Range( 0, 50 ) )
            {
                calc.AddValue( 21 );
            }

            foreach( var x in Enumerable.Range( 0, 10 ) )
            {
                calc.AddValue( 31 );
            }

            foreach( var x in Enumerable.Range( 0, 2 ) )
            {
                calc.AddValue( 51 );
            }

            calc.CalculateStats();

            var bound = calc.FindBoundForPercentile( 1 );
            Assert.AreEqual( 40, bound, "1st percentile is in 40 bucket" );

            bound = calc.FindBoundForPercentile( 10 );
            Assert.AreEqual( 30, bound, "10th percentile is in 30 bucket" );

            bound = calc.FindBoundForPercentile( 50 );
            Assert.AreEqual( 20, bound, "50th percentile is in 20 bucket" );

            bound = calc.FindBoundForPercentile( 91 );
            Assert.AreEqual( 20, bound, "91st percentile is in 20 bucket" );

            bound = calc.FindBoundForPercentile( 100 );
            Assert.AreEqual( 10, bound, "100th percentile is in 10 bucket" );
        }

        [TestMethod]
        public void FiftiethPercentile()
        {
            var calc = new PercentileCalculator( new[] { 1, 20, 30, 40, 100 } );

            foreach( var x in Enumerable.Range( 0, 81435 ) )
            {
                calc.AddValue( 1 );
            }

            foreach( var x in Enumerable.Range( 0, 58 ) )
            {
                calc.AddValue( 21 );
            }

            foreach( var x in Enumerable.Range( 0, 10 ) )
            {
                calc.AddValue( 31 );
            }

            foreach( var x in Enumerable.Range( 0, 1 ) )
            {
                calc.AddValue( 51 );
            }

            calc.CalculateStats();

            var bound = calc.FindBoundForPercentile( 50 );
            Assert.AreEqual( 1, bound, "50th percentile is in 1st bucket" );
        }

    }
}
