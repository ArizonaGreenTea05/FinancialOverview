using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BusinessLogic
{
    public class History : IEnumerable
    {
        private List<Snapshot> _history;

        public int CurrentIndex { get; set; } = 0;

        public Snapshot CurrentSnapshot => _history[CurrentIndex];

        public int Length => _history.Count;

        public History()
        {
            _history = new List<Snapshot>();
        }

        public History(Snapshot snapshot)
        {
            _history = new List<Snapshot>
            {
                snapshot
            };
        }

        public void Add(Snapshot snapshot)
        {
            _history.Add(snapshot);
            ++CurrentIndex;
        }

        public Snapshot Get(int index)
        {
            return _history[index];
        }
        
        public IEnumerator GetEnumerator()
        {
            return _history.GetEnumerator();
        }
    }
}