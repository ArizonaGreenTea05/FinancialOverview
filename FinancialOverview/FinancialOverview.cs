using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using static FinancialOverview.Enums;

namespace FinancialOverview
{
    public class FinancialOverview
    {
        #region private Members

        private readonly History.History _history;
        private List<string> _fileHistory = new();
        private readonly XmlSerializer _serializer = new(typeof(SaveObject));
        private int _currentFileVersion = -1;
        private readonly Dictionary<int, Action<string>> _dataLoadingMethods = new();

        #endregion

        #region public Event Handlers

        public event EventHandler<string> OnDefaultFilePathChanged;

        #endregion

        #region public Properties

        public static string DefaultFilename { get; } = "FinancialOverview.finance";

        public ObservableCollection<SalesObject> Sales { get; private set; } = new();

        public List<string> FileHistory
        {
            get => _fileHistory;
            set
            {
                _fileHistory = value;
                FilePath = _fileHistory != null && _fileHistory.Count >= 1
                    ? _fileHistory[0]
                    : null;
            }
        }

        public string FilePath
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FileDirectory = Filename = value;
                    return;
                }
                FileHistory.RemoveAll(value.Equals);
                _fileHistory.Insert(0, value);
                FileDirectory = Path.GetDirectoryName(value);
                Filename = Path.GetFileName(value);
                OnDefaultFilePathChanged?.Invoke(this, value);
            }
            get => string.IsNullOrEmpty(FileDirectory) || string.IsNullOrEmpty(Filename)
                ? null
                : $@"{FileDirectory.Replace('/', '\\')}\{Filename.Replace('/', '\\')}";
        }

        public string FileDirectory { get; set; } = null;

        public string Filename { get; set; } = DefaultFilename;

        #endregion

        #region public Constructors

        public FinancialOverview()
        {
            _history = new History.History();
            _dataLoadingMethods.Add(0, LoadDataVersionZero);
            _dataLoadingMethods.Add(1, LoadDataVersionOne);
        }

        #endregion

        #region public Methods

        public IEnumerable<SalesObject> GetSales(int year)
        {
            return Sales.Select(item => item.GetForRange(year)).Where(item => item != null);
        }

        public IEnumerable<SalesObject> GetSales(Month month, int year)
        {
            return month == Month.WholeYear
                ? GetSales(year)
                : Sales.Select(item => item.GetForRange(month, year)).Where(item => item != null);
        }

        public bool LoadData(string path)
        {
            if (!File.Exists(path)) return false;
            try
            {
                _currentFileVersion = path.Replace(".finance", "") == path
                    ? 0
                    : ((SaveObject)_serializer.Deserialize(new FileStream(path, FileMode.Open, FileAccess.Read,
                        FileShare.Read))).FileVersion;
                _dataLoadingMethods[_currentFileVersion](path);
            }
            catch
            {
                return false;
            }
            
            FilePath = path;
            _history.Clear(Sales);
            return true;
        }

        public bool LoadData()
        {
            return LoadData(FilePath);
        }

        public bool SaveData(string path)
        {
            if (!File.Exists(path)) return false;
            if (_currentFileVersion == 0)
            {
                File.Delete(path);
                path = path.Replace(".xml", ".finance");
            }
            _currentFileVersion = 1;
            var writer = new StreamWriter(path);
            var tmp = new SaveObject(_currentFileVersion, Sales);
            _serializer.Serialize(writer, tmp);
            writer.Close();

            FilePath = path;
            return true;
        }

        public bool SaveData()
        {
            return SaveData(FilePath);
        }

        public string GetRestAsString(Month month, int year)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:#,##0.00}", Math.Round(GetRest(month, year), 2));
        }

        public decimal GetRest(Month month, int year)
        {
            return Math.Round(GetSales(month, year).Sum(item => item.Value), 2);
        }

        public bool Undo()
        {
            return _history.Undo(Sales);
        }

        public bool Redo()
        {
            return _history.Redo(Sales);
        }

        public void ClearHistory()
        {
            _history.Clear(Sales);
        }

        public void AddCurrentStateToHistory()
        {
            _history.AddSnapshot(Sales);
        }

        #endregion

        #region private Methods

        private void LoadDataVersionZero(string path)
        {
            var ds = new DataSet("FinancialOverview");
            var dt1 = new DataTable("MonthlySales");
            dt1.Columns.Add(new DataColumn("Sales"));
            dt1.Columns.Add(new DataColumn("Name"));
            dt1.Columns.Add(new DataColumn("Addition"));
            var dt2 = new DataTable("YearlySales");
            dt2.Columns.Add(new DataColumn("Sales"));
            dt2.Columns.Add(new DataColumn("Name"));
            dt2.Columns.Add(new DataColumn("Addition"));
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            ds.ReadXml(path);

            Sales.Clear();
            foreach (DataRow row in dt1.Rows)
                Sales.Add(new SalesObject(Convert.ToDecimal(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), DateTime.MinValue, DateTime.MaxValue, SaleRepeatCycle.Monthly, 1));

            foreach (DataRow row in dt2.Rows)
                Sales.Add(new SalesObject(Convert.ToDecimal(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), DateTime.MinValue, DateTime.MaxValue, SaleRepeatCycle.Yearly, 1));
        }

        private void LoadDataVersionOne(string path)
        {
           Sales = ((SaveObject)_serializer.Deserialize(
               new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                ).Sales;
        }

        #endregion
    }
}