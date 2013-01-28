using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PercentilePerformance
{
    public class RenderPercentileSamplesToImage : RenderPercentile
    {
        private readonly Color[] m_tempColours;

        public RenderPercentileSamplesToImage()
        {
            m_tempColours = CreateColdToHotTempColours();
        }

        public void RenderToImage( IEnumerable<PercentileSample> samplesCollection, string path, ImageFormat format )
        {
            using( var strm = new FileStream( path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None ) ) 
            {
                RenderToImage( samplesCollection, strm, format );
            }
        }

        public void RenderToImage( IEnumerable<PercentileSample> samplesCollection, Stream outStream, ImageFormat format )
        {
            PercentileSample[] samplesArray = samplesCollection.ToArray();

            int cw = 15;
            int ch = 15;
            int xOffset = 30;
            int xTrail = 31;

            int[] bounds = (from x in samplesArray[0].Stats select x.Bound).ToArray();

            using( var img = new Bitmap( (samplesArray.Length * cw) + 1 + xOffset + xTrail, (bounds.Length * ch) + 1 ) )
            {
                var imgWidth = img.Width - xTrail;

                using( var graphics = Graphics.FromImage( img ) )
                {
                    graphics.Clear( Color.White );

                    for( int x = 0; x < samplesArray.Length; ++x )
                    {
                        var buckets = samplesArray[x].Stats.ToArray();

                        int max = (from b in buckets select b.Count).Max();

                        for( int y = 0; y < buckets.Length; ++y )
                        {
                            double p = 0;

                            if( buckets[y].Count != 0 )
                            {
                                p = (double)buckets[y].Count / max;
                            }

                            double idx = Math.Min( m_tempColours.Length * p, m_tempColours.Length - 1 );
                            var colour = buckets[y].Count != 0 ? m_tempColours[(int)idx] : ZeroSampleColour;

                            using( var brush = new SolidBrush( colour ) )
                            {
                                var rect = new Rectangle( x * ((imgWidth - xOffset) / samplesArray.Length) + xOffset, y * (img.Height / bounds.Length), ((imgWidth - xOffset) / samplesArray.Length), (img.Height / bounds.Length) );
                                graphics.FillRectangle( brush, rect );
                            }
                        }
                    }

                    using( var pen = new Pen( Color.Black, 0.5F ) )
                    {
                        int y;

                        for( int ly = 0; ly < img.Height; ly += (img.Height / bounds.Length) )
                        {
                            graphics.DrawLine( pen, xOffset + 0, ly, imgWidth, ly );
                        }

                        for( int lx = 0; lx < (imgWidth - xOffset); lx += ((imgWidth - xOffset) / samplesArray.Length) )
                        {
                            graphics.DrawLine( pen, xOffset + lx, 0, xOffset + lx, img.Height );
                        }

                        using( var font = new Font( "Courier New", 8 ) )
                        {
                            y = 0;

                            for( int i = 0; i < bounds.Length; ++i )
                            {
                                graphics.DrawString( string.Format( "{0,4}", bounds[i] ), font, Brushes.Black, 0, y );

                                y += (img.Height / bounds.Length);
                            }
                        }
                    }

                    for( int y = 0; y < bounds.Length; ++y )
                    {
                        double p = (double)(bounds.Length - y) / bounds.Length;
                        double idx = Math.Min( m_tempColours.Length * p, m_tempColours.Length - 1 );
                        var colour = m_tempColours[(int)idx];

                        var x = imgWidth + 10;

                        using( var brush = new SolidBrush( colour ) )
                        {
                            var rect = new Rectangle( x, y * (img.Height / bounds.Length), 20, (img.Height / bounds.Length) );
                            graphics.FillRectangle( brush, rect );
                        }
                    }

                    using( var pen = new Pen( Color.Black, 0.5F ) )
                    {
                        int x = imgWidth + 10;
                        int y = 0;

                        for( int i = 0; i <= bounds.Length; ++i )
                        {
                            graphics.DrawLine( pen, x, y, x + 20, y );
                            y += (img.Height / bounds.Length);
                        }

                        graphics.DrawLine( pen, x, 0, x, img.Height );
                        graphics.DrawLine( pen, x + 20, 0, x + 20, img.Height );
                    }


                    foreach( var percentile in new[] {50} )
                    {
                        using( var font = new Font( "Courier New", 7 ) )
                        {
                            int x = xOffset;

                            for( int bucketIdx = 0; bucketIdx < samplesArray.Length; ++bucketIdx )
                            {
                                var bucket = samplesArray[bucketIdx].FindBucketForPercentile( percentile );

                                if( bucket != null )
                                {
                                    graphics.DrawString( percentile.ToString( CultureInfo.InvariantCulture ), font, Brushes.Black, x, bucket.Pos * ch );
                                }

                                x += cw;
                            }
                        }
                    }
                }

                img.Save( outStream, format );
            }
        }
    }
}