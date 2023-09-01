using System.Collections.ObjectModel;

namespace FinancialOverview
{
    public class SaveObject
    {
        public int FileVersion { get; set; }
        public ObservableCollection<SalesObject> Sales { get; set; }

        public SaveObject()
        {
        }

        public SaveObject(int fileVersion, ObservableCollection<SalesObject> sales)
        {
            FileVersion = fileVersion;
            Sales = sales;
        }
    }
}
