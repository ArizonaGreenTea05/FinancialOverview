using BusinessLogic.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.IsolatedStorage;

namespace BusinessLogic.History
{
    public class Snapshot
    {
        private DataTable MonthlySales { get; set; }
        private DataTable YearlySales { get; set; }
        
        public Snapshot(DataTable yearlySales, DataTable monthlySales)
        {
            YearlySales = new DataTable();
            foreach (DataColumn column in yearlySales.Columns)
                YearlySales.Columns.Add(column.ColumnName);
            yearlySales.Rows.CopyTo(YearlySales.Rows);
            MonthlySales = new DataTable();
            foreach (DataColumn column in monthlySales.Columns)
                MonthlySales.Columns.Add(column.ColumnName);
            monthlySales.Rows.CopyTo(MonthlySales.Rows);
        }

        public void TransferTo(DataTable yearlySales, DataTable monthlySales)
        {
            yearlySales.Rows.Clear();
            YearlySales.Rows.CopyTo(yearlySales.Rows);
            monthlySales.Rows.Clear();
            MonthlySales.Rows.CopyTo(monthlySales.Rows);
        }
    }
}