using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;

namespace HistYahoo
{
    public class Market
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public override string ToString()
        {
            return string.Format("Date: {0}, Open: {1}, High: {2}, Low {3}, Close {4}, Volume {5}",
                Date, Open, High, Low, Close, Volume);
        }

    }
}
