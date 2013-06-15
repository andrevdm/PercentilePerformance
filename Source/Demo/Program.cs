using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Avdm.PercentilePerformance;

namespace Demo
{
    class Program
    {
        static void Main( string[] args )
        {
            //Create the calculator and track 10 samples (columns)
            var calc = new PercentileCalculator( 10 );
            var rand = new Random();

            int x = 0;

            //Add some values
            for( int i = 0; i < 10; ++i )
            {
                for( int s = 0; s < 100; ++s )
                {
                    calc.AddValue( rand.Next( 0, 2100 ) );
                }

                //Sample every 100 values
                calc.SampleAndReset();
            }

            //Render as HTML
            var htmler = new RenderPercentileSamplesToHtml();
            string html = htmler.RenderToHtml( calc.StoredStats.ToArray(), "" );
            File.WriteAllText( @"stats.html", html );

            //Render as PNG
            var imgr = new RenderPercentileSamplesToImage();
            imgr.RenderToImage( calc.StoredStats.ToArray(), @"stats.png", ImageFormat.Png );

            //Render as text
            var txt = new RenderPercentileSamplesToText();
            File.WriteAllText( @"stats.txt", txt.RenderToText( calc.StoredStats.ToArray() ) );
        }
    }
}
