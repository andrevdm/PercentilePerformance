using System;
using System.Linq;
using System.Text;

namespace PercentilePerformance
{
    public class RenderPercentileSamplesToHtml : RenderPercentile
    {
        public string RenderToHtml( PercentileSample[] samples, string baseUrl = "/" )
        {
            var colours = CreateColdToHotTempColours();
            var grid = SamplesToGrid( samples );

            var html = new StringBuilder();

            html.AppendFormat( "<html>" ).AppendLine();
            html.AppendFormat( "<head>" ).AppendLine();
            html.AppendFormat( "<link rel='stylesheet' type='text/css' href='{0}heatMap.css'>", baseUrl ).AppendLine();
            html.AppendFormat( "<script type='text/javascript' src='{0}heatMap.js'></script>", baseUrl ).AppendLine();
            html.AppendFormat( "<script type='text/javascript' src='{0}jquery-1.7.2.min.js'></script>", baseUrl ).AppendLine();
            html.AppendFormat( "</head>" ).AppendLine();

            html.AppendFormat( "<table border='1'>" ).AppendLine();

            for( int rowIdx = 0; rowIdx < grid.Count; ++rowIdx )
            {
                object[] row = grid[rowIdx];

                html.AppendFormat( "   <tr>" ).AppendLine();

                for( int colIdx = 0; colIdx < row.Length; ++colIdx )
                {
                    if( rowIdx == 0 )
                    {
                        string title = "";

                        if( colIdx > 0 )
                        {
                            var count = (from s in samples[colIdx - 1].Stats select s.Count).Sum();
                            title = string.Format( "samples={0}\r\nstarted={1}\r\nfinished={2}", count, samples[colIdx - 1].FirstValueAt, samples[colIdx - 1].LastValueAt );
                        }

                        html.AppendFormat( "      <td class='heatHeader' title='{0}'>", title ).AppendLine();
                        html.AppendFormat( "      {0}", row[colIdx] ).AppendLine();
                        html.AppendFormat( "      </td>" ).AppendLine();
                    }
                    else
                    {
                        if( colIdx == 0 )
                        {
                            var colTotal = (from s in samples
                                            select s.Stats[rowIdx - 1].Count).Sum();

                            html.AppendFormat( "      <td class='boundCol' title='count={0}'>", colTotal ).AppendLine();
                            html.AppendFormat( "      {0}", row[colIdx] ).AppendLine();
                            html.AppendFormat( "      </td>" ).AppendLine();
                        }
                        else
                        {
                            var sample = samples[colIdx - 1];
                            int max = (from b in sample.Stats select b.Count).Max();

                            double p = 0;

                            if( sample.Stats[rowIdx - 1].Count != 0 )
                            {
                                p = (double)sample.Stats[rowIdx - 1].Count / max;
                            }

                            double idx = Math.Min( colours.Length * p, colours.Length - 1 );
                            var colour = sample.Stats[rowIdx - 1].Count != 0 ? colours[(int)idx] : ZeroSampleColour;

                            html.AppendFormat( "      <td  class='heatCol' style='background-color:#{0:X2}{1:X2}{2:X2}'>", colour.R, colour.G, colour.B ).AppendLine();
                            html.AppendFormat( "      <span class='heatCount'>{0}</span>", row[colIdx] ).AppendLine();

                            if( sample.FindBoundForPercentile( 50 ) == int.Parse( grid[rowIdx][0].ToString() ) )
                            {
                                html.AppendFormat( "      <span class='percentile'>50</span>" ).AppendLine();
                            }

                            html.AppendFormat( "      </td>" ).AppendLine();
                        }
                    }
                }

                html.AppendFormat( "   </tr>" ).AppendLine();

            }

            html.AppendFormat( "</table>" ).AppendLine();

            html.AppendFormat( "<a href='#' class='heatCount' onclick='showPercentile()'>percentile</a>" ).AppendLine();
            html.AppendFormat( "<a href='#' class='percentile' onclick='showPercentile()'>count</a>" ).AppendLine();

            return html.ToString();
        }
    }
}
