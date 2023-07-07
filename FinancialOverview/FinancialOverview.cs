using System;
using System.Data;
using BusinessLogic.History;
using System.IO;

namespace BusinessLogic
{
    public class FinancialOverview
    {
        private readonly DataSet _dataSet = new DataSet();
        private DataTable _allSales;
        private readonly History.History _history;
        private bool _blockRowChangedHandler = false;

        public event EventHandler<string> OnDefaultFilePathChanged;

        public enum Unit
        {
            Monthly,
            Yearly
        }
        
        public Unit UnitOfAll { get; set; } = Unit.Monthly;

        public string DefaultFilePath
        {
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                DefaultDirectory = Path.GetDirectoryName(value);
                DefaultFilename = Path.GetFileName(value);
                OnDefaultFilePathChanged?.Invoke(this, value);
            }
            get => string.IsNullOrEmpty(DefaultDirectory) || string.IsNullOrEmpty(DefaultFilename)
                ? null
                : $@"{DefaultDirectory.Replace('/', '\\')}\{DefaultFilename.Replace('/', '\\')}";
        }

        public string DefaultDirectory { get; set; } = null;

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
            MonthlySales.RowDeleted += RowChanged;
            YearlySales.RowChanged += RowChanged;
            YearlySales.RowDeleted += RowChanged;
            _history = new History.History(YearlySales, MonthlySales);
        }

        public bool LoadData(string path)
        {
            if (!File.Exists(path)) return false;
            MonthlySales.Clear();
            YearlySales.Clear();
            _dataSet.ReadXml(path);
            DefaultFilePath = path;
            _history.Clear();
            return true;
        }

        public bool LoadData()
        {
            return LoadData(DefaultFilePath);
        }

        public bool SaveData(string path)
        {
            if (!File.Exists(path)) return false;
            _dataSet.WriteXml(path);
            DefaultFilePath = path;
            _history.Clear();
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
            _blockRowChangedHandler = true;
            _history.Undo();
            _blockRowChangedHandler = false;
        }

        public void Redo()
        {
            _blockRowChangedHandler = true;
            _history.Redo();
            _blockRowChangedHandler = false;
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public void AddCurrentStateToHistory()
        {
            _history.AddSnapshot();
        }

        private void RowChanged(object obj, DataRowChangeEventArgs eventArgs)
        {
            if (_blockRowChangedHandler) return;
            AddCurrentStateToHistory();
        }
    }
}