using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace Avdm.PercentilePerformance
{
    public abstract class RenderPercentile
    {
        public List<object[]> SamplesToGrid( PercentileSample[] samples )
        {
            var grid = new List<object[]>();

            if( (samples == null) || (samples.Length == 0) )
            {
                return grid;
            }

            var bounds = samples[0].PercentileCalculator.GetBounds().ToArray();

            var header = new object[1 + samples.Length];
            header[0] = "Bound";

            for( int i = 0; i < samples.Length; ++i )
            {
                header[i + 1] = i;
            }

            grid.Add( header );

            for( int row = 0; row < bounds.Length; ++row )
            {
                var rowData = new object[1 + samples.Length];
                rowData[0] = bounds[row];

                for( int col = 0; col < samples.Length; ++col )
                {
                    var count = samples[col].Stats[row].Count;
                    rowData[1 + col] = count > 0 ? count.ToString( CultureInfo.InvariantCulture ) : " ";
                }

                grid.Add( rowData );
            }

            return grid;
        }

        public Color ZeroSampleColour
        {
            get { return Color.SlateGray; }
        }

        public Color[] CreateColdToHotTempColours()
        {
            return new[]
                {
                    Color.FromArgb( 255, 5, 0, 255 ),
                    Color.FromArgb( 255, 4, 0, 255 ),
                    Color.FromArgb( 255, 3, 0, 255 ),
                    Color.FromArgb( 255, 2, 0, 255 ),
                    Color.FromArgb( 255, 1, 0, 255 ),
                    Color.FromArgb( 255, 0, 0, 255 ),
                    Color.FromArgb( 255, 0, 2, 255 ),
                    Color.FromArgb( 255, 0, 18, 255 ),
                    Color.FromArgb( 255, 0, 34, 255 ),
                    Color.FromArgb( 255, 0, 50, 255 ),
                    Color.FromArgb( 255, 0, 68, 255 ),
                    Color.FromArgb( 255, 0, 84, 255 ),
                    Color.FromArgb( 255, 0, 100, 255 ),
                    Color.FromArgb( 255, 0, 116, 255 ),
                    Color.FromArgb( 255, 0, 132, 255 ),
                    Color.FromArgb( 255, 0, 148, 255 ),
                    Color.FromArgb( 255, 0, 164, 255 ),
                    Color.FromArgb( 255, 0, 180, 255 ),
                    Color.FromArgb( 255, 0, 196, 255 ),
                    Color.FromArgb( 255, 0, 212, 255 ),
                    Color.FromArgb( 255, 0, 228, 255 ),
                    Color.FromArgb( 255, 0, 255, 244 ),
                    Color.FromArgb( 255, 0, 255, 208 ),
                    Color.FromArgb( 255, 0, 255, 168 ),
                    Color.FromArgb( 255, 0, 255, 131 ),
                    Color.FromArgb( 255, 0, 255, 92 ),
                    Color.FromArgb( 255, 0, 255, 54 ),
                    Color.FromArgb( 255, 0, 255, 16 ),
                    Color.FromArgb( 255, 23, 255, 0 ),
                    Color.FromArgb( 255, 62, 255, 0 ),
                    Color.FromArgb( 255, 101, 255, 0 ),
                    Color.FromArgb( 255, 138, 255, 0 ),
                    Color.FromArgb( 255, 176, 255, 0 ),
                    Color.FromArgb( 255, 215, 255, 0 ),
                    Color.FromArgb( 255, 253, 255, 0 ),
                    Color.FromArgb( 255, 255, 250, 0 ),
                    Color.FromArgb( 255, 255, 240, 0 ),
                    Color.FromArgb( 255, 255, 230, 0 ),
                    Color.FromArgb( 255, 255, 220, 0 ),
                    Color.FromArgb( 255, 255, 210, 0 ),
                    Color.FromArgb( 255, 255, 200, 0 ),
                    Color.FromArgb( 255, 255, 190, 0 ),
                    Color.FromArgb( 255, 255, 180, 0 ),
                    Color.FromArgb( 255, 255, 170, 0 ),
                    Color.FromArgb( 255, 255, 160, 0 ),
                    Color.FromArgb( 255, 255, 150, 0 ),
                    Color.FromArgb( 255, 255, 140, 0 ),
                    Color.FromArgb( 255, 255, 130, 0 ),
                    Color.FromArgb( 255, 255, 120, 0 ),
                    Color.FromArgb( 255, 255, 110, 0 ),
                    Color.FromArgb( 255, 255, 100, 0 ),
                    Color.FromArgb( 255, 255, 90, 0 ),
                    Color.FromArgb( 255, 255, 80, 0 ),
                    Color.FromArgb( 255, 255, 70, 0 ),
                    Color.FromArgb( 255, 255, 60, 0 ),
                    Color.FromArgb( 255, 255, 50, 0 ),
                    Color.FromArgb( 255, 255, 40, 0 ),
                    Color.FromArgb( 255, 255, 30, 0 ),
                    Color.FromArgb( 255, 255, 20, 0 ),
                    Color.FromArgb( 255, 255, 10, 0 ),
                    Color.FromArgb( 255, 255, 0, 0 ),
                    Color.FromArgb( 255, 255, 0, 16 ),
                    Color.FromArgb( 255, 255, 0, 32 ),
                    Color.FromArgb( 255, 255, 0, 48 ),
                    Color.FromArgb( 255, 255, 0, 64 ),
                    Color.FromArgb( 255, 255, 0, 80 ),
                    Color.FromArgb( 255, 255, 0, 96 ),
                    Color.FromArgb( 255, 255, 0, 112 ),
                    Color.FromArgb( 255, 255, 0, 128 ),
                    Color.FromArgb( 255, 255, 0, 144 ),
                    Color.FromArgb( 255, 255, 0, 160 ),
                    Color.FromArgb( 255, 255, 0, 176 ),
                    Color.FromArgb( 255, 255, 0, 192 ),
                    Color.FromArgb( 255, 255, 0, 208 ),
                    Color.FromArgb( 255, 255, 0, 224 ),
                    Color.FromArgb( 255, 255, 0, 240 ),
                    Color.FromArgb( 255, 255, 1, 240 ),
                    Color.FromArgb( 255, 255, 2, 240 ),
                    Color.FromArgb( 255, 255, 3, 240 ),
                    Color.FromArgb( 255, 255, 4, 240 ),
                    Color.FromArgb( 255, 255, 5, 240 ),
                    Color.FromArgb( 255, 255, 6, 240 ),
                    Color.FromArgb( 255, 255, 7, 240 ),
                    Color.FromArgb( 255, 255, 8, 240 ),
                    Color.FromArgb( 255, 255, 9, 240 ),
                    Color.FromArgb( 255, 255, 10, 240 ),
                    Color.FromArgb( 255, 255, 11, 240 ),
                    Color.FromArgb( 255, 255, 12, 240 ),
                    Color.FromArgb( 255, 255, 13, 240 ),
                    Color.FromArgb( 255, 255, 14, 240 )
                };
        }
    }
}