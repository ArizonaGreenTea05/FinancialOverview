using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialOverview
{
    public class FinancialOverview
    {
        private DataTable _allSales;
        public DataTable MonthlySales { get; set; }
        public DataTable YearlySales { get; set; }

        public DataTable AllSales
        {
            get
            {
                _allSales.Rows.Clear();
                foreach (DataRow row in MonthlySales.Rows)
                {
                    var newRow = new object[3];
                    for (int i = 0; i < _allSales.Columns.Count; ++i)
                        newRow[i] = row[i];
                    _allSales.Rows.Add(newRow);
                }
                return _allSales;
            }
            private set => _allSales = value;
        }

        public FinancialOverview()
        {
            MonthlySales = new DataTable();
            MonthlySales.Columns.Clear();
            MonthlySales.Columns.Add(new DataColumn("Sales"));
            MonthlySales.Columns.Add(new DataColumn("Name"));
            MonthlySales.Columns.Add(new DataColumn("Addition"));
            YearlySales = new DataTable();
            YearlySales.Columns.Clear();
            YearlySales.Columns.Add(new DataColumn("Sales"));
            YearlySales.Columns.Add(new DataColumn("Name"));
            YearlySales.Columns.Add(new DataColumn("Addition"));
            AllSales = new DataTable();
            AllSales.Columns.Clear();
            AllSales.Columns.Add(new DataColumn("Sales"));
            AllSales.Columns.Add(new DataColumn("Name"));
            AllSales.Columns.Add(new DataColumn("Addition"));
        }
    }
}