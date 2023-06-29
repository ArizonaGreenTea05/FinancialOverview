using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLogic
{
    public class FinancialOverview
    {
        private readonly DataSet _dataSet = new DataSet();
        private DataTable _allSales;
        private readonly History _history;

        public enum Unit
        {
            Monthly,
            Yearly
        }
        
        public Unit UnitOfAll { get; set; } = Unit.Monthly;

        public string DefaultFilePath => $@"{DefaultDirectory}/{DefaultFilename}";

        public string DefaultDirectory { get; set; } = "D:/OneDrive/Documents";

        public string DefaultFilename { get; set; } = "FinancialOverview.xml";
        
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
            MonthlySales.TableName = nameof(MonthlySales);
            MonthlySales.Columns.Clear();
            MonthlySales.Columns.Add(new DataColumn("Sales"));
            MonthlySales.Columns.Add(new DataColumn("Name"));
            MonthlySales.Columns.Add(new DataColumn("Addition"));
            YearlySales = new DataTable();
            YearlySales.TableName = nameof(YearlySales);
            YearlySales.Columns.Clear();
            YearlySales.Columns.Add(new DataColumn("Sales"));
            YearlySales.Columns.Add(new DataColumn("Name"));
            YearlySales.Columns.Add(new DataColumn("Addition"));
            AllSales = new DataTable();
            AllSales.TableName = nameof(AllSales);
            AllSales.Columns.Clear();
            AllSales.Columns.Add(new DataColumn("Sales"));
            AllSales.Columns.Add(new DataColumn("Name"));
            AllSales.Columns.Add(new DataColumn("Addition"));
            _dataSet.DataSetName = "FinancialOverview";
            _dataSet.Tables.Add(MonthlySales);
            _dataSet.Tables.Add(YearlySales);
            MonthlySales.RowChanged += RowChanged;
            YearlySales.RowChanged += RowChanged;
            _history = new History(new Snapshot(YearlySales, MonthlySales));
        }

        public bool LoadData(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (!Directory.Exists(Path.GetDirectoryName(path))) return false;
            MonthlySales.Clear();
            YearlySales.Clear();
            _dataSet.ReadXml(path);
            return true;
        }

        public bool LoadData()
        {
            return LoadData(DefaultFilePath);
        }

        public bool SaveData(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            _dataSet.WriteXml(path);
            return true;
        }

        public bool SaveData()
        {
            return SaveData(DefaultFilePath);
        }


        public decimal GetRest()
        {
            if (AllSales.Rows.Count <= 0) return 0;
            var rest = Convert.ToDecimal(AllSales.Rows[0][0]);
            for (int i = 1; i < AllSales.Rows.Count; ++i)
                rest += Convert.ToDecimal(AllSales.Rows[i][0]);
            return rest;
        }

        public void Undo()
        {
            if(_history.CurrentIndex == 0) return;
            --_history.CurrentIndex;
            var current = _history.CurrentSnapshot;
            MonthlySales = current.MonthlySales;
            YearlySales = current.YearlySales;
        }

        public void Redo()
        {
            if(_history.CurrentIndex == _history.Length - 1) return;
            ++_history.CurrentIndex;
            var current = _history.CurrentSnapshot;
            MonthlySales = current.MonthlySales;
            YearlySales = current.YearlySales;
        }
        
        private void AddSnapshot()
        {
            _history.Add(new Snapshot(YearlySales, MonthlySales));
        }

        private void RowChanged(object obj, DataRowChangeEventArgs eventArgs)
        {
            AddSnapshot();
        }
    }
}