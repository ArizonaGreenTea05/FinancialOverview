using System.Collections.Generic;
using System.Data;
using System.IO.IsolatedStorage;

namespace BusinessLogic
{
    public class Snapshot
    {
        public DataTable MonthlySales { get; set; }
        public DataTable YearlySales { get; set; }
        
        public Snapshot(DataTable yearlySales, DataTable monthlySales)
        {
            MonthlySales = monthlySales.Clone();
            YearlySales = yearlySales.Clone();
        }
    }
}