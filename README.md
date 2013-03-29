
[Permalink](http://andrevdm.blogspot.com/2013/03/efficiently-tracking-response-time.html "Permalink to Efficiently Tracking Response Time Percentiles (in C#)")

# Efficiently Tracking Response Time Percentiles (in C#)

&nbsp;

When looking for a better way to track response times than a simple min/max/average statistic recently I found a great [article][1] that had a clever solution. This article shows how to efficiently track the n-th percentile performance while storing only a small amount of data. See the full original article here [http://techblog.molindo.at/2009/11/efficiently-tracking-response-time-percentiles.html][2]

The original code is in Java but I needed it in .NET so I’ve created a .net version. I chose to do a complete rewrite rather than porting the Java code so the class names etc will be different. The idea however is the same.

My .net version can generate output in three formats

PNG

&nbsp;&nbsp;&nbsp;![PNG](http://lh4.ggpht.com/-Q4PQNybDBhw/UVXo-mNMuUI/AAAAAAAAAIg/-CCY4TZ7oxw/clip_image002_thumb%25255B3%25255D.gif?imgmax=800) 

HTML

&nbsp;&nbsp;&nbsp;![HTML](http://lh3.ggpht.com/-w7s9FgD-dsw/UVXpBNxxRJI/AAAAAAAAAIw/opaN7kTIV-o/image_thumb%25255B1%25255D.png?imgmax=800)    

&nbsp;

Text

&nbsp;&nbsp;&nbsp;![Text](http://lh4.ggpht.com/-HQEW-E76l-Q/UVXpCr24k3I/AAAAAAAAAJA/-E2CIkEooy0/image_thumb%25255B3%25255D.png?imgmax=800)


&nbsp;

I hope this proves useful to someone.
