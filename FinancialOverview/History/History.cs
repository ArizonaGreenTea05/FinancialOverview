using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic.History
{
    public class History : IEnumerable
    {
        private readonly List<Snapshot> _history;

        private DataTable YearlySales { get; set; }

        private DataTable MonthlySales { get; set; }

        public int CurrentIndex { get; set; } = -1;

        public Snapshot CurrentSnapshot => _history[CurrentIndex];

        public int Length => _history.Count;

        public History()
        {
            _history = new List<Snapshot>();
        }

        public History(DataTable yearlySales, DataTable monthlySales)
        {
            _history = new List<Snapshot>();
            YearlySales = yearlySales;
            MonthlySales = monthlySales;
        }

        public void AddSnapshot()
        {
            ++CurrentIndex;
            if (Length >= CurrentIndex) _history.RemoveRange(CurrentIndex, Length-CurrentIndex);
            _history.Add(new Snapshot(YearlySales, MonthlySales));
        }

        public Snapshot Get(int index)
        {
            return _history[index];
        }

        public void Clear()
        {
            _history.Clear();
            CurrentIndex = -1;
            AddSnapshot();
        }

        public void Undo()
        {;
            if (CurrentIndex <= 0) return;
            --CurrentIndex;
            CurrentSnapshot.TransferTo(YearlySales, MonthlySales);
        }

        public void Redo()
        {
            if (CurrentIndex >= Length - 1) return;
            ++CurrentIndex;
            CurrentSnapshot.TransferTo(YearlySales, MonthlySales);
        }

        public IEnumerator GetEnumerator()
        {
            return _history.GetEnumerator();
        }
    }
}