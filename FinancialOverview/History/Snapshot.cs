using BusinessLogic.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.IsolatedStorage;

namespace BusinessLogic.History
{
    public class Snapshot
    {
        private DataRow[] MonthlySales { get; set; }
        private DataRow[] YearlySales { get; set; }
        
        public Snapshot(DataTable yearlySales, DataTable monthlySales)
        {
            YearlySales = new DataRow[yearlySales.Rows.Count];
            yearlySales.Rows.CopyTo(YearlySales,0);
            MonthlySales = new DataRow[monthlySales.Rows.Count];
            monthlySales.Rows.CopyTo(MonthlySales,0);
        }

        public void TransferTo(DataTable yearlySales, DataTable monthlySales)
        {
            yearlySales.Rows.Clear();
            foreach (var row in YearlySales.CopyToDataTable().Rows)
                yearlySales.Rows.Add(row);
            monthlySales.Rows.Clear();
            foreach (var row in MonthlySales.CopyToDataTable().Rows)
                monthlySales.Rows.Add(row);
        }
    }
}