using System;                     // For system functions like Console.
using System.Collections.Generic; // For generic collections like List.
using System.Data.SqlClient;      // For the database connections and objects
using System.Data.Odbc;

namespace HistYahoo {
    
    
    public partial class DataSet1 {
        partial class HistPriceTableDataTable
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
}

namespace HistYahoo.DataSet1TableAdapters {


    public partial class HistPriceTableTableAdapter
    {
    }
}
