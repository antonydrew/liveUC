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
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;


// THIS DOES NOT WORK THOUGH IT WOULD BE AN EASIER WAY TO GRAB DATA - IT HANGS on DATA TYPE "DATETIME" (see row 46)

namespace HistYahoo
{
    public class HistoricalStockDownloader
    {
        public static List<Market> DownloadData(string tickers, int start_date)
        {
            List<Market> retval = new List<Market>();
            using (WebClient web = new WebClient())
            {
                string data = web.DownloadString(string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&c={1}", tickers, start_date));
                //string data = web.DownloadString(string.Format("http://www.google.com/finance/historical?q={0}&startdate={1}&enddate={2}&output={3}", ticker, start_date, end_date, format));
                
                data = data.Replace("r", "");
                string[] rows = data.Split('n');
                //First row is headers so Ignore it
                for (int i = 1; i < rows.Length; i++)
                {
                    if (rows[i].Replace("n", "").Trim() == "") continue;
                    string[] cols = rows[i].Split(',');
                    Market hs = new Market();
                    String tdate = cols[0];
                    String topen = cols[1];
                    String thigh = cols[2];
                    String tlow = cols[3];
                    String tclose = cols[4];
                    String tvolume = cols[5];

                    
                    hs.Date = Convert.ToDateTime(tdate);
                    hs.Open = Convert.ToDouble(topen);
                    hs.High = Convert.ToDouble(thigh);
                    hs.Low = Convert.ToDouble(tlow);
                    hs.Close = Convert.ToDouble(tclose);
                    hs.Volume = Convert.ToDouble(tvolume);
                    //hs.AdjClose = Convert.ToDouble(cols[6]);
                    retval.Add(hs);
                }
                return retval;
            }
        }
    }
}

