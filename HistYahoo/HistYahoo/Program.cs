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
using System.Windows.Forms;
//using System.Windows.Forms.TextBox;



namespace HistYahoo
{
       
    
    public class Program
    {


        static void Main(string[] args)
        {

            // YAHOO EXAMPLE DID NOT WORK DUE TO DATE FORMAT "DATETIME" - see row 46 in HistStocksDownload.cs file - THIS WOULD BE CLEANER, NEATER WAY TO GET DATA THOUGH//

            /*
            List<HistoricalStock> data = HistoricalStockDownloader.DownloadData("AAPL", 2010);
            foreach (HistoricalStock stock in data)
            {
                Console.WriteLine(string.Format("Date={0} High={1} Low={2} Open={3} Close{4}", stock.Date, stock.High, stock.Low, stock.Open, stock.Close));
            }
            Console.Read();
            */


            // THIS BATS EXAMPLE THAT WORKED FOR LIST OF TICKERS VIA XML FILE //

            /*
            string url = "http://www.batstrading.com/market_data/symbol_listing/xml/";
            XDocument doc = XDocument.Load(url);
            var symbols = from s in doc.Root.Element("symbols").Elements("symbol")
                          select new { Name = s.Attribute("name").Value };
            foreach (var symbol in symbols)
            {
                Console.WriteLine(symbol.Name);
            }
            */

            // THIS EXAMPLE WORKS FOR GETTING LIVE DATA FROM GOOGLE BUT NOT FOR HISTORICAL !!!!!!!!!!!  //

            /*
            //string url = "http://www.google.com/ig/api?stock=goog&stock=aapl&start_date=20101231&end_date=20111231&interval=1";
            string url = "http://www.google.com/ig/api?stock=goog&stock=DJI";
            //string url = "http://www.google.com/finance/historical?q=goog&startdate=20101231&enddate=20111231&output=xml";
            XDocument doc = XDocument.Load(url);
            var data = from e in doc.Root.Elements("finance")
                       select new
                       {
                           Symbol = e.Element("symbol").Attribute("data").Value,
                           Last = Convert.ToDecimal(e.Element("last").Attribute("data").Value),
                           High = Convert.ToDecimal(e.Element("high").Attribute("data").Value),
                           Low = Convert.ToDecimal(e.Element("low").Attribute("data").Value),
                           Open = Convert.ToDecimal(e.Element("open").Attribute("data").Value),
                           TDate = Convert.ToInt32(e.Element("trade_date_utc").Attribute("data").Value)
                       };

            foreach (var price in data)
            {
                Console.WriteLine(string.Format("{0} {1} {2}", price.Symbol, price.Last, price.TDate));
            }
            Console.ReadLine();
            */

            // GET HISTY DATA FROM WEB QUERY VIA YAHOO //
            string csvData;
            using (WebClient web = new WebClient())
            {
                //csvData = web.DownloadString(string.Format("http://www.google.com/finance/historical?q={0}&startdate={1}&enddate={2}&output=csv", ticker, sdate, edate));
                csvData = web.DownloadString("http://www.google.com/finance/historical?q=GOOG&startdate=20101231&enddate=20111231&output=csv");
            }

           
            StreamWriter writer;                                                // create CSV file from GOOGLE WEB DOWNLOAD - THIS WILL MAKE IT EASIER FOR FORMATTING AND DATABASE LATER
            writer = new StreamWriter("HistPrice" + ".csv");                    // no need for ARRAYS HERE !!!
            writer.WriteLine(csvData);
            writer.Close();

            string line;

            string[] lines = new string[10000];                                 // created array to store data just in case but we DONT NEED TO DO THIS
            int counter = 0;

            try
            {
                StreamReader file = new StreamReader("HistPrice" + ".csv");     // ARRAY (BUT NOT NECESSARY) - DATA WOULD STILL NEED TO BE PARSED DUE TO CSV FORMAT - CREATING ODBC FROM FILE AVOIDS THIS
                try
                {
                    line = file.ReadLine();
                    do
                    {
                        lines[counter] = line;
                     
                        counter++;
                        
                    } while ((line = file.ReadLine()) != null);

                    file.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            


            try
            {
                List<Market> markets = new List<Market>();
                ConnectionStringSettings settings2;
                settings2 = System.Configuration.ConfigurationManager.ConnectionStrings["HistYahoo.Properties.Settings.masterConnectionString"];
                string connectionString2 = settings2.ConnectionString;

                
                // THIS IS FOR SQL SERVER DATABASE (WHICH NOT EVERYONE HAS) - YOU WOULD HAVE TO CREATE THIS FIRST FOR THIS TO WORK IN SQL SERVER MGMNT STUDIO //
                // CONNECTION STRING IS DYNAMIC SO U DONT HAVE TO WORRY ABOUT UPDATING LOCAL SETTINGS

                using (SqlConnection con = new SqlConnection(connectionString2))
                //"Server=localhost;Integrated security=SSPI;database=master")) 
                //"Data Source=XTREME-PC\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True"))
                {
                    con.Open();


                    masterDataSet.HpriceDataTable table2 = new masterDataSet.HpriceDataTable();


                    HistYahoo.masterDataSetTableAdapters.HpriceTableAdapter tableAdapter2 =
                    new HistYahoo.masterDataSetTableAdapters.HpriceTableAdapter();


                    tableAdapter2.GetData();

                    
                    //  CREATE NEW INSTANCE OF LIST "MARKET" TO HOLD DATA FROM HistPrice.csv file created earlier - see line 100 //
                    using (SqlCommand command = new SqlCommand("SELECT * FROM " + table2, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            String date = reader.GetString(0);
                            String open = reader.GetString(1);
                            String high = reader.GetString(2);
                            String low = reader.GetString(3);
                            String close = reader.GetString(4);
                            String volume = reader.GetString(5);

                            DateTime datef = Convert.ToDateTime(date);
                            Double openf = Convert.ToDouble(open);
                            Double highf = Convert.ToDouble(high);
                            Double lowf = Convert.ToDouble(low);
                            Double closef = Convert.ToDouble(close);
                            Double volumef = Convert.ToDouble(volume);


                            markets.Add(new Market() { Date = datef, High = highf, Low = lowf, Close = closef, Volume = volumef });

                            //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", datef,
                            //openf, highf, lowf, closef, volumef);


                        }
                        reader.Close();
                    }
                    con.Close();

                }
                //foreach (Market market in markets)
                //{
                //    Console.WriteLine(market);
                    
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            try
            {
            // GET LOCAL SETTINGS FOR ODBC DATABASE FROM HistPrice.csv file insert //
            ConnectionStringSettings settings;
            settings = System.Configuration.ConfigurationManager.ConnectionStrings["HistYahoo.Properties.Settings.ConnectionString"];
            string connectionString1 = settings.ConnectionString;

            using (OdbcConnection con2 = new OdbcConnection(connectionString1))
            {
                con2.Open();
                // SEE cs CLASS FILE DataSet1 // this can be used more easily to QUERY DATA LATER ON - NOT BEING USED YET
                HistYahoo.DataSet1TableAdapters.QueriesTableAdapter tableAdapter =
                new HistYahoo.DataSet1TableAdapters.QueriesTableAdapter();

                // SETUP INSTANCE OF DIRECT QUERIES FOR LATER USE IN STATS CALCS - THIS EXECUTES TEST QUERY BUT DOES NOT OUPTUT ANYWHERE YET - WILL BE USED LATER ON FOR STATS Z-SCORE
                tableAdapter.LastPriceQuery();


                DataSet1.HistPriceTableDataTable table = new DataSet1.HistPriceTableDataTable();


                HistYahoo.DataSet1TableAdapters.HistPriceTableTableAdapter tableAdapter1 =
                new HistYahoo.DataSet1TableAdapters.HistPriceTableTableAdapter();


                tableAdapter1.GetData();

                // USING ODBC CONNECTION HERE SINCE IS AVIAL TO EVERYONE VIA LOCAL FILE "HistPrice.csv" // (AS OPPOSED TO SQL SERVER DATABASE)

                using (OdbcCommand command = new OdbcCommand("SELECT * FROM HistPrice.csv", con2))
                {
                    OdbcDataReader reader = command.ExecuteReader();
                    List<Market> hs = new List<Market>();
                    while (reader.Read())
                    {

                        // CSV FILE CAN HAVE NULL VALUES AT END SO WE NEED TO SKIP THESE AND NOT ADD TO MARKET LIST //
                        if (reader.IsDBNull(2))
                        {
                            //Console.Write("<NULL>");
                        }
                        else
                        {
                            try
                            {
                                DateTime date = reader.GetDateTime(0);
                                Double open = reader.GetDouble(1);
                                Double high = reader.GetDouble(2);
                                Double low = reader.GetDouble(3);
                                Double close = reader.GetDouble(4);
                                Double volume = reader.GetDouble(5);

                                hs.Add(new Market() { Date = date, High = high, Low = low, Close = close, Volume = volume });

                                //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", reader.GetDateTime(0),
                                //reader.GetDouble(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetDouble(4), reader.GetDouble(5));
                            }
                            catch (InvalidCastException)
                            {
                                Console.Write("Invalid data type.");
                            }
                        }
                        //Console.WriteLine();
                        


                    }
                    reader.Close();
                    //foreach (Market market in hs)
                    //{
                    //    Console.WriteLine(market);

                    //}
                }
                con2.Close();
               
                
            }
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //Console.Read();                                                             // HIT RETURN TO CLOSE PROGRAM
            Application.Run(new Form1());
        }
    }
}


