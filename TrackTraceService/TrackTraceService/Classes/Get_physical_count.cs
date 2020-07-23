using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace TrackTraceService.Classes
{
    public class Get_physical_count
    {
        public string DESCRIPTION { get; set; }
        public string SLNO { get; set; }
        public DateTime CALIBRATED_ON { get; set; }
        public string REMARK { get; set; }
        public string Result { get; set; }
    }

    public class SavePhysicalCount
    {
        public string BARCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string SLNO { get; set; }
        public DateTime CALIBRATED_ON { get; set; }
        public string REMARK { get; set; }
        public string CRUSER { get; set; }
        //public DateTime CRDATE { get; set; }
        // public string MDUSER { get; set; }
        // public DateTime MDDATE { get; set; }
         public string PLANT { get; set; }
       // public string LOCATION { get; set; }
       
    }

    public class inData
    {
        public string name { get; set; }
        public List<Get_physical_count> datalst { get; set; }
    }


    public class ListtoDataTableConverter
    {

        public static DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)
            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }

    }


    public class online_physical_count
    {

        public string CATEGORY { get; set; }
        public string DESCRIPTION { get; set; }
        public string SLNO { get; set; }
        public string CALIBRATED_ON { get; set; }
        public string REMARK { get; set; }
        public string Result { get; set; }
    }

    public class offline_physical_count
    {
        public string BARCODE { get; set; }
        public string USERNAME { get; set; }
    }

    public class dtPackage
    {
        public string BARCODE { get; set; }
    }


    public class ESDateTimeConverter : IsoDateTimeConverter
    {
        public ESDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        }
    }
}