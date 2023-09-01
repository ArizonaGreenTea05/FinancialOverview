using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinancialOverview;

namespace FinancialOverview.History
{
    public class History : IEnumerable
    {
        private readonly List<Snapshot> _history = new List<Snapshot>();

        public int CurrentIndex { get; set; } = -1;

        public Snapshot CurrentSnapshot => _history[CurrentIndex];

        public int Length => _history.Count;

        public void AddSnapshot(ObservableCollection<SalesObject> salesObjects)
        {
            ++CurrentIndex;
            if (Length >= CurrentIndex) _history.RemoveRange(CurrentIndex, Length-CurrentIndex);
            var tmp = new Snapshot(salesObjects);
            if (_history.Count > 0 && _history[_history.Count - 1].Equals(tmp))
            {
                --CurrentIndex;
                return;
            }
            _history.Add(tmp);
        }

        public Snapshot Get(int index)
        {
            return _history[index];
        }

        public void Clear(ObservableCollection<SalesObject> salesObjects)
        {
            _history.Clear();
            CurrentIndex = -1;
            AddSnapshot(salesObjects);
        }

        public bool Undo(ObservableCollection<SalesObject> salesObjects)
        {
            if (CurrentIndex <= 0) return false;
            var tmp = CurrentSnapshot;
            while (tmp.Equals(CurrentSnapshot) && CurrentIndex > 0)
                --CurrentIndex;
            CurrentSnapshot.TransferTo(salesObjects);
            return true;
        }

        public bool Redo(ObservableCollection<SalesObject> salesObjects)
        {
            if (CurrentIndex >= Length - 1) return false;
            var tmp = CurrentSnapshot;
            while (tmp.Equals(CurrentSnapshot) && CurrentIndex < _history.Count - 1)
                ++CurrentIndex;
            CurrentSnapshot.TransferTo(salesObjects);
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return _history.GetEnumerator();
        }
    }
}