using System.Collections.ObjectModel;
using System.Linq;
using FinancialOverview;

namespace FinancialOverview.History
{
    public class Snapshot
    {
        private ObservableCollection<SalesObject> SalesObjects { get; }

        public Snapshot(ObservableCollection<SalesObject> salesObjects)
        {
            SalesObjects = new ObservableCollection<SalesObject>();
            foreach (var item in salesObjects) SalesObjects.Add(item.Copy());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Snapshot tmp)) return false;
            return Equals(tmp);
        }

        public bool Equals(Snapshot other)
        {
            if (SalesObjects.Count != other.SalesObjects.Count) return false;
            return !SalesObjects.Where((t, i) => !t.Equals(other.SalesObjects[i])).Any();
        }

        public override int GetHashCode()
        {
            return (SalesObjects != null ? SalesObjects.GetHashCode() : 0);
        }

        public void TransferTo(ObservableCollection<SalesObject> salesObjects)
        {
            salesObjects.Clear();
            foreach (var item in SalesObjects) salesObjects.Add(item);
        }
    }
}