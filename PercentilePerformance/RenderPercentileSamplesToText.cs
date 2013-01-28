using System.Linq;
using System.Collections.Generic;
using System.Text;
namespace PercentilePerformance
{
    public class RenderPercentileSamplesToText : RenderPercentile
    {
        public string RenderToText( PercentileSample[] samples )
        {
            var grid = SamplesToGrid( samples );
            return ArrayToTextTable( grid );
        }

        public static string ArrayToTextTable( List<object[]> items )
        {
            var header = items[0];
            string ruler = "";

            for( var col = 0; col < header.Length; ++col )
            {
                int colMax = (from r in items select r[col].ToString().Length).Max();

                ruler += "+--";
                ruler += new string( '-', colMax );

                foreach( var item in items )
                {
                    string format = "{0," + colMax + "}";
                    item[col] = string.Format( format, item[col] );
                }
            }

            ruler += "+";

            var str = new StringBuilder();
            str.AppendLine( ruler );

            for( int index = 0; index < items.Count; index++ )
            {
                var item = items[index];
                str.Append( "| " );

                for( int col = 0; col < item.Length; ++col )
                {
                    str.Append( item[col] );
                    str.Append( " | " );
                }

                str.AppendLine();

                if( index == 0 )
                {
                    str.AppendLine( ruler );
                }
            }

            str.AppendLine( ruler );
            return str.ToString();
        }
    }
}