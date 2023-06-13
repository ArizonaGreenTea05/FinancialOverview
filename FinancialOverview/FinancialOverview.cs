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
        DataSet _dataSet = new DataSet();
        private DataTable _allSales;

        public enum Unit
        {
            Monthly,
            Yearly
        }
        
        public Unit UnitOfAll { get; set; } = Unit.Monthly;

        public string DefaultPath { get; set; } = "D:/OneDrive/Documents/FinancialOverview.xml";
        
        public DataTable MonthlySales { get; set; }
        public DataTable YearlySales { get; set; }

        public DataTable AllSales
        {
            get
            {
                var isMonthly = UnitOfAll == Unit.Monthly;_allSales.Rows.Clear();
                foreach (DataRow row in MonthlySales.Rows)
                {
                    var newRow = new object[3];
                    newRow[0] = isMonthly ? row[0] : Convert.ToDouble(row[0]) * 12;
                    for (int i = 1; i < _allSales.Columns.Count; ++i)
                        newRow[i] = row[i];
                    _allSales.Rows.Add(newRow);
                }
                foreach (DataRow row in YearlySales.Rows)
                {
                    var newRow = new object[3];
                    newRow[0] = isMonthly ? Math.Round(Convert.ToDouble(row[0]) / 12, 2) : row[0];
                    for (int i = 1; i < _allSales.Columns.Count; ++i)
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
            _dataSet.Tables.Add(MonthlySales);
            _dataSet.Tables.Add(YearlySales);
        }

        public bool LoadData(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            _dataSet.ReadXml(path);
            return true;
        }

        public bool LoadData()
        {
            return LoadData(DefaultPath);
        }

        public bool SaveData(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            _dataSet.WriteXml(path);
            return true;
        }

        public bool SaveData()
        {
            return SaveData(DefaultPath);
        }


        public double GetRest()
        {
            if (AllSales.Rows.Count <= 0) return 0;
            var rest = Convert.ToDouble(AllSales.Rows[0][0]);
            for (int i = 1; i < AllSales.Rows.Count; ++i)
                rest += Convert.ToDouble(AllSales.Rows[i][0]);
            return rest;
        }
    }
}